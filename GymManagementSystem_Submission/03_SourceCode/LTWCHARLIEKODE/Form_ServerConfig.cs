using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;

namespace LTWCHARLIEKODE
{
    public partial class Form_ServerConfig : Form
    {
        public bool IsConfigSaved { get; private set; } = false;

        public Form_ServerConfig()
        {
            InitializeComponent();
        }

        private void Form_ServerConfig_Load(object sender, EventArgs e)
        {
            LoadCurrentConfiguration();
        }

        private void LoadCurrentConfiguration()
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["GymManagementDBEntities1"]?.ConnectionString
                              ?? ConfigurationManager.ConnectionStrings["GymManagementDBEntities"]?.ConnectionString;

                if (!string.IsNullOrEmpty(connStr))
                {
                    // Lấy provider connection string bên trong chuỗi EntityClient
                    string providerConn = ExtractProviderConnectionString(connStr);
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(providerConn);

                    txtServerName.Text = string.IsNullOrEmpty(builder.DataSource) ? "." : builder.DataSource;
                    txtDatabaseName.Text = string.IsNullOrEmpty(builder.InitialCatalog) ? "GymManagementDB" : builder.InitialCatalog;

                    if (builder.IntegratedSecurity)
                    {
                        rbWindowsAuth.Checked = true;
                    }
                    else
                    {
                        rbSqlAuth.Checked = true;
                        txtUsername.Text = builder.UserID;
                        txtPassword.Text = builder.Password;
                    }
                }
                else
                {
                    txtServerName.Text = ".";
                    txtDatabaseName.Text = "GymManagementDB";
                    rbWindowsAuth.Checked = true;
                }
            }
            catch (Exception ex)
            {
                txtServerName.Text = ".";
                txtDatabaseName.Text = "GymManagementDB";
                rbWindowsAuth.Checked = true;
                lblStatus.Text = "Không đọc được cấu hình cũ: " + ex.Message;
            }

            ToggleAuthFields();
        }

        private string ExtractProviderConnectionString(string entityConnStr)
        {
            int startIndex = entityConnStr.IndexOf("provider connection string=\"", StringComparison.OrdinalIgnoreCase);
            if (startIndex >= 0)
            {
                startIndex += "provider connection string=\"".Length;
                int endIndex = entityConnStr.LastIndexOf("\"");
                if (endIndex > startIndex)
                {
                    return entityConnStr.Substring(startIndex, endIndex - startIndex);
                }
            }
            return entityConnStr;
        }

        private string BuildSqlConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = txtServerName.Text.Trim(),
                InitialCatalog = txtDatabaseName.Text.Trim(),
                MultipleActiveResultSets = true,
                TrustServerCertificate = true,
                Encrypt = false
            };

            if (rbWindowsAuth.Checked)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.IntegratedSecurity = false;
                builder.UserID = txtUsername.Text.Trim();
                builder.Password = txtPassword.Text;
            }

            return builder.ConnectionString;
        }

        private void rbWindowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            ToggleAuthFields();
        }

        private void rbSqlAuth_CheckedChanged(object sender, EventArgs e)
        {
            ToggleAuthFields();
        }

        private void ToggleAuthFields()
        {
            bool isSqlAuth = rbSqlAuth.Checked;
            lblUsername.Enabled = isSqlAuth;
            txtUsername.Enabled = isSqlAuth;
            lblPassword.Enabled = isSqlAuth;
            txtPassword.Enabled = isSqlAuth;
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServerName.Text))
            {
                MessageBox.Show("Vui lòng nhập Server Name!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServerName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDatabaseName.Text))
            {
                MessageBox.Show("Vui lòng nhập Database Name!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDatabaseName.Focus();
                return;
            }

            lblStatus.Text = "Đang kiểm tra kết nối tới SQL Server...";
            lblStatus.ForeColor = System.Drawing.Color.Blue;
            Application.DoEvents();

            try
            {
                string connStr = BuildSqlConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM sys.tables";
                        int tableCount = (int)cmd.ExecuteScalar();
                        lblStatus.Text = $"✓ Kết nối thành công! (Tìm thấy {tableCount} bảng trong CSDL '{txtDatabaseName.Text.Trim()}')";
                        lblStatus.ForeColor = System.Drawing.Color.ForestGreen;
                        MessageBox.Show("Kiểm tra kết nối thành công!\nCơ sở dữ liệu sẵn sàng hoạt động.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "✗ Kết nối thất bại: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Crimson;
                MessageBox.Show("Không thể kết nối đến SQL Server!\nChi tiết: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string providerConn = BuildSqlConnectionString();
                string newEntityConn1 = $"metadata=res://*/GymModel.csdl|res://*/GymModel.ssdl|res://*/GymModel.msl;provider=System.Data.SqlClient;provider connection string=\"{providerConn};App=EntityFramework\"";
                string newEntityConn0 = $"metadata=res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl;provider=System.Data.SqlClient;provider connection string=\"{providerConn};App=EntityFramework\"";

                // 1. Cập nhật file App.config (nếu có trong thư mục chạy hoặc thư mục dự án)
                UpdateConfigFile(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, newEntityConn0, newEntityConn1);

                // 2. Cập nhật App.config ở thư mục source (nếu đang debug)
                string projectDirAppConfig = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\App.config");
                if (System.IO.File.Exists(projectDirAppConfig))
                {
                    try
                    {
                        UpdateConfigFile(System.IO.Path.GetFullPath(projectDirAppConfig), newEntityConn0, newEntityConn1);
                    }
                    catch { }
                }

                // 3. Làm mới ConfigurationManager
                ConfigurationManager.RefreshSection("connectionStrings");

                IsConfigSaved = true;
                MessageBox.Show("Đã lưu cấu hình kết nối thành công!\nChuỗi kết nối mới đã được cập nhật vào file cấu hình.",
                                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu cấu hình: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateConfigFile(string configFilePath, string conn0, string conn1)
        {
            if (!System.IO.File.Exists(configFilePath)) return;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configFilePath);

            XmlNodeList addNodes = xmlDoc.SelectNodes("//connectionStrings/add");
            if (addNodes != null)
            {
                foreach (XmlNode node in addNodes)
                {
                    string name = node.Attributes["name"]?.Value;
                    if (name == "GymManagementDBEntities")
                    {
                        node.Attributes["connectionString"].Value = conn0;
                    }
                    else if (name == "GymManagementDBEntities1")
                    {
                        node.Attributes["connectionString"].Value = conn1;
                    }
                }
            }

            xmlDoc.Save(configFilePath);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
