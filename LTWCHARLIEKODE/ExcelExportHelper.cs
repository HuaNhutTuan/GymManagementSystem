using System;
using System.Data;
using System.Drawing;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace LTWCHARLIEKODE
{
    public static class ExcelExportHelper
    {
        /// <summary>
        /// Xuất dữ liệu từ DataTable ra file Excel .xlsx bằng thư viện EPPlus
        /// Không yêu cầu cài đặt ứng dụng Microsoft Excel trên máy tính.
        /// </summary>
        public static void ExportToExcel(DataTable dt, string filePath, string sheetName, string titleHeader)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new InvalidOperationException("Không có dữ liệu để xuất file Excel!");
            }

            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete(); // Xóa file cũ nếu đã tồn tại
            }

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add(string.IsNullOrWhiteSpace(sheetName) ? "DanhSach" : sheetName);
                ws.View.ShowGridLines = true;

                int colCount = dt.Columns.Count;

                // 1. Dòng Tiêu Đề Lớn
                ws.Cells[1, 1, 1, colCount].Merge = true;
                ws.Cells[1, 1].Value = titleHeader.ToUpper();
                ws.Cells[1, 1].Style.Font.Bold = true;
                ws.Cells[1, 1].Style.Font.Size = 16;
                ws.Cells[1, 1].Style.Font.Color.SetColor(Color.FromArgb(24, 43, 73));
                ws.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Row(1).Height = 35;

                // 2. Dòng Thông Tin Thời Gian & Số Lượng
                ws.Cells[2, 1, 2, colCount].Merge = true;
                ws.Cells[2, 1].Value = $"Thời gian xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss} | Tổng số bản ghi: {dt.Rows.Count}";
                ws.Cells[2, 1].Style.Font.Italic = true;
                ws.Cells[2, 1].Style.Font.Size = 10;
                ws.Cells[2, 1].Style.Font.Color.SetColor(Color.Gray);
                ws.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Row(2).Height = 20;

                // 3. Tiêu Đề Cột (Headers) - Bắt đầu từ dòng 4
                int startRow = 4;
                for (int c = 0; c < colCount; c++)
                {
                    var cell = ws.Cells[startRow, c + 1];
                    cell.Value = dt.Columns[c].ColumnName;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Color.SetColor(Color.White);
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(24, 43, 73)); // Navy Blue sang trọng
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.FromArgb(180, 180, 180));
                }
                ws.Row(startRow).Height = 26;

                // 4. Nạp Dữ Liệu Các Dòng
                int currentRow = startRow + 1;
                foreach (DataRow row in dt.Rows)
                {
                    for (int c = 0; c < colCount; c++)
                    {
                        var cell = ws.Cells[currentRow, c + 1];
                        object val = row[c];

                        if (val is DateTime dtVal)
                        {
                            cell.Value = dtVal.ToString("dd/MM/yyyy");
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        else if (val is decimal decVal)
                        {
                            cell.Value = (double)decVal;
                            cell.Style.Numberformat.Format = "#,##0";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        }
                        else if (val is int intVal)
                        {
                            cell.Value = intVal;
                            cell.Style.Numberformat.Format = "#,##0";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        }
                        else
                        {
                            cell.Value = val?.ToString() ?? "";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        }

                        // Tô màu xen kẽ
                        if (currentRow % 2 == 0)
                        {
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(248, 250, 252));
                        }

                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.FromArgb(220, 224, 230));
                        cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }
                    ws.Row(currentRow).Height = 22;
                    currentRow++;
                }

                // 5. Tự động co giãn độ rộng cột
                ws.Cells.AutoFitColumns();
                for (int c = 1; c <= colCount; c++)
                {
                    ws.Column(c).Width = Math.Max(ws.Column(c).Width + 4, 15);
                }

                package.Save();
            }
        }

        /// <summary>
        /// Mở SaveFileDialog và xuất toàn bộ nội dung hiển thị trong DataGridView ra file Excel .xlsx bằng EPPlus
        /// </summary>
        public static void ExportDataGridViewWithDialog(System.Windows.Forms.DataGridView dgv, string defaultFileNamePrefix, string sheetName, string titleHeader)
        {
            if (dgv == null || dgv.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Không có dữ liệu trong bảng để xuất ra file Excel!", "Thông báo", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (var sfd = new System.Windows.Forms.SaveFileDialog())
                {
                    sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                    sfd.FileName = $"{defaultFileNamePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                    if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        DataTable dt = new DataTable();
                        foreach (System.Windows.Forms.DataGridViewColumn col in dgv.Columns)
                        {
                            if (col.Visible)
                            {
                                dt.Columns.Add(col.HeaderText, typeof(string));
                            }
                        }

                        foreach (System.Windows.Forms.DataGridViewRow row in dgv.Rows)
                        {
                            if (row.IsNewRow) continue;
                            DataRow dRow = dt.NewRow();
                            int cIndex = 0;
                            foreach (System.Windows.Forms.DataGridViewColumn col in dgv.Columns)
                            {
                                if (col.Visible)
                                {
                                    dRow[cIndex] = row.Cells[col.Index].FormattedValue?.ToString() ?? "";
                                    cIndex++;
                                }
                            }
                            dt.Rows.Add(dRow);
                        }

                        ExportToExcel(dt, sfd.FileName, sheetName, titleHeader);

                        var result = System.Windows.Forms.MessageBox.Show(
                            "Xuất dữ liệu ra file Excel thành công bằng EPPlus!\nBạn có muốn mở file vừa xuất ngay bây giờ không?",
                            "Thành công",
                            System.Windows.Forms.MessageBoxButtons.YesNo,
                            System.Windows.Forms.MessageBoxIcon.Question);

                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(sfd.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Lỗi khi xuất file Excel: {ex.Message}", "Lỗi", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
    }
}
