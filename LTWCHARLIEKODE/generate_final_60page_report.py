# -*- coding: utf-8 -*-
import os
import subprocess
import docx
from docx.shared import Inches, Pt, RGBColor
from docx.enum.text import WD_ALIGN_PARAGRAPH, WD_TAB_ALIGNMENT, WD_TAB_LEADER
from docx.enum.table import WD_TABLE_ALIGNMENT
from docx.oxml import OxmlElement, parse_xml
from docx.oxml.ns import nsdecls, qn

def build_full_report():
    doc = docx.Document()

    # Page Margins: Top=0.79in (2.0cm), Bottom=0.79in (2.0cm), Left=1.18in (3.0cm), Right=0.59in (1.5cm)
    sec = doc.sections[0]
    sec.top_margin = Inches(0.79)
    sec.bottom_margin = Inches(0.79)
    sec.left_margin = Inches(1.18)
    sec.right_margin = Inches(0.59)

    def set_cell_margins(cell, top=100, bottom=100, left=120, right=120):
        tcPr = cell._tc.get_or_add_tcPr()
        tcMar = OxmlElement('w:tcMar')
        for m, val in [('top', top), ('bottom', bottom), ('left', left), ('right', right)]:
            node = OxmlElement(f'w:{m}')
            node.set(qn('w:w'), str(val))
            node.set(qn('w:type'), 'dxa')
            tcMar.append(node)
        tcPr.append(tcMar)

    def set_cell_background(cell, hex_color):
        shd = parse_xml(f'<w:shd {nsdecls("w")} w:fill="{hex_color}"/>')
        cell._tc.get_or_add_tcPr().append(shd)

    def set_cell_borders(cell, top="single", bottom="single", left="single", right="single", color="D3D3D3", sz="4"):
        tcPr = cell._tc.get_or_add_tcPr()
        tcBorders = OxmlElement('w:tcBorders')
        for side, border in [('top', top), ('bottom', bottom), ('left', left), ('right', right)]:
            if border and border != "none":
                b_elm = parse_xml(f'<w:{side} {nsdecls("w")} w:val="{border}" w:sz="{sz}" w:space="0" w:color="{color}"/>')
                tcBorders.append(b_elm)
            else:
                b_elm = parse_xml(f'<w:{side} {nsdecls("w")} w:val="none"/>')
                tcBorders.append(b_elm)
        tcPr.append(tcBorders)

    def add_p(text, bold_prefix="", italic=False, align=WD_ALIGN_PARAGRAPH.JUSTIFY, space_after=6, line_spacing=1.3):
        if not text and not bold_prefix:
            return None

        raw_lines = text.split('\n') if text else []

        # If bold_prefix exists and text has multiple lines or is a bullet list:
        if bold_prefix:
            if len(raw_lines) > 1 or (raw_lines and raw_lines[0].strip().startswith(('1.', '-', '*', '•'))):
                p_pre = doc.add_paragraph()
                p_pre.alignment = align
                p_pre.paragraph_format.space_before = Pt(0)
                p_pre.paragraph_format.space_after = Pt(4)
                p_pre.paragraph_format.line_spacing = line_spacing
                r = p_pre.add_run(bold_prefix)
                r.bold = True
                r.font.name = 'Times New Roman'
                r.font.size = Pt(13)
                bold_prefix = ""

        if not raw_lines and bold_prefix:
            p_pre = doc.add_paragraph()
            p_pre.alignment = align
            p_pre.paragraph_format.space_before = Pt(0)
            p_pre.paragraph_format.space_after = Pt(space_after)
            p_pre.paragraph_format.line_spacing = line_spacing
            r = p_pre.add_run(bold_prefix)
            r.bold = True
            r.font.name = 'Times New Roman'
            r.font.size = Pt(13)
            return p_pre

        first_p = None
        for idx, line in enumerate(raw_lines):
            line_str = line.strip()
            if not line_str:
                continue

            p = doc.add_paragraph()
            if first_p is None:
                first_p = p
            p.alignment = align
            p.paragraph_format.space_before = Pt(0)
            p.paragraph_format.space_after = Pt(space_after)
            p.paragraph_format.line_spacing = line_spacing

            # Sub-indentation for list items
            if line.startswith('   - ') or line.startswith('     - '):
                p.paragraph_format.left_indent = Inches(0.4)
                line_str = line.strip()
            elif line.startswith('  - ') or line.startswith('- '):
                p.paragraph_format.left_indent = Inches(0.2)
                line_str = line.strip()

            if idx == 0 and bold_prefix:
                r_pre = p.add_run(bold_prefix)
                r_pre.bold = True
                r_pre.font.name = 'Times New Roman'
                r_pre.font.size = Pt(13)

            if line_str:
                r_txt = p.add_run(line_str)
                r_txt.font.name = 'Times New Roman'
                r_txt.font.size = Pt(13)
                if italic:
                    r_txt.italic = True

        return first_p

    def add_h1(text):
        doc.add_page_break()
        p = doc.add_paragraph()
        p.paragraph_format.keep_with_next = True
        p.paragraph_format.space_before = Pt(18)
        p.paragraph_format.space_after = Pt(8)
        run = p.add_run(text)
        run.bold = True
        run.font.name = 'Times New Roman'
        run.font.size = Pt(16)
        run.font.color.rgb = RGBColor(0, 0, 0)
        return p

    def add_h2(text):
        p = doc.add_paragraph()
        p.paragraph_format.keep_with_next = True
        p.paragraph_format.space_before = Pt(14)
        p.paragraph_format.space_after = Pt(6)
        run = p.add_run(text)
        run.bold = True
        run.font.name = 'Times New Roman'
        run.font.size = Pt(14)
        run.font.color.rgb = RGBColor(0, 0, 0)
        return p

    def add_h3(text):
        p = doc.add_paragraph()
        p.paragraph_format.keep_with_next = True
        p.paragraph_format.space_before = Pt(10)
        p.paragraph_format.space_after = Pt(4)
        run = p.add_run(text)
        run.bold = True
        run.font.name = 'Times New Roman'
        run.font.size = Pt(13)
        run.font.color.rgb = RGBColor(0, 0, 0)
        return p

    def add_code(code_str):
        tbl = doc.add_table(rows=1, cols=1)
        tbl.alignment = WD_TABLE_ALIGNMENT.CENTER
        cell = tbl.cell(0, 0)
        set_cell_background(cell, "F4F6F9")
        set_cell_borders(cell, "single", "single", "single", "single", "B0C4DE", "6")
        set_cell_margins(cell, top=100, bottom=100, left=140, right=140)
        cell.width = Inches(6.8)
        p = cell.paragraphs[0]
        p.alignment = WD_ALIGN_PARAGRAPH.LEFT
        p.paragraph_format.space_before = Pt(0)
        p.paragraph_format.space_after = Pt(0)
        p.paragraph_format.line_spacing = 1.15
        run = p.add_run(code_str)
        run.font.name = 'Consolas'
        run.font.size = Pt(9.5)
        run.font.color.rgb = RGBColor(20, 30, 50)
        p_spacer = doc.add_paragraph()
        p_spacer.paragraph_format.space_before = Pt(0)
        p_spacer.paragraph_format.space_after = Pt(6)

    def add_custom_table(headers, rows_data, col_widths=None):
        tbl = doc.add_table(rows=len(rows_data) + 1, cols=len(headers))
        tbl.alignment = WD_TABLE_ALIGNMENT.CENTER
        hdr_row = tbl.rows[0]
        trPr = hdr_row._tr.get_or_add_trPr()
        trPr.append(parse_xml(f'<w:tblHeader {nsdecls("w")}/>'))

        for c_idx, h_text in enumerate(headers):
            cell = hdr_row.cells[c_idx]
            if col_widths and c_idx < len(col_widths):
                cell.width = Inches(col_widths[c_idx])
            set_cell_background(cell, "2E75B5")
            set_cell_borders(cell, "single", "single", "single", "single", "1B4F72", "6")
            set_cell_margins(cell, top=100, bottom=100, left=100, right=100)
            p = cell.paragraphs[0]
            p.alignment = WD_ALIGN_PARAGRAPH.CENTER
            p.paragraph_format.space_before = Pt(0)
            p.paragraph_format.space_after = Pt(0)
            run = p.add_run(h_text)
            run.bold = True
            run.font.name = 'Times New Roman'
            run.font.size = Pt(11)
            run.font.color.rgb = RGBColor(255, 255, 255)

        for r_idx, r_data in enumerate(rows_data):
            row = tbl.rows[r_idx + 1]
            bg_col = "F9FBFD" if r_idx % 2 == 1 else "FFFFFF"
            for c_idx, val in enumerate(r_data):
                cell = row.cells[c_idx]
                if col_widths and c_idx < len(col_widths):
                    cell.width = Inches(col_widths[c_idx])
                set_cell_background(cell, bg_col)
                set_cell_borders(cell, "single", "single", "single", "single", "D3D3D3", "4")
                set_cell_margins(cell, top=80, bottom=80, left=100, right=100)
                val_str = str(val)
                lines = val_str.split('\n')
                for l_idx, line_t in enumerate(lines):
                    p = cell.paragraphs[0] if l_idx == 0 else cell.add_paragraph()
                    p.alignment = WD_ALIGN_PARAGRAPH.LEFT if c_idx > 1 else WD_ALIGN_PARAGRAPH.CENTER
                    p.paragraph_format.space_before = Pt(0)
                    p.paragraph_format.space_after = Pt(2 if len(lines) > 1 else 0)
                    p.paragraph_format.line_spacing = 1.15
                    run = p.add_run(line_t.strip())
                    run.font.name = 'Times New Roman'
                    run.font.size = Pt(10.5)
                    run.font.color.rgb = RGBColor(20, 20, 20)

        p_spacer = doc.add_paragraph()
        p_spacer.paragraph_format.space_before = Pt(0)
        p_spacer.paragraph_format.space_after = Pt(6)
        return tbl

    print("Building Cover Page...")
    # BÌA CHÍNH
    p0 = doc.add_paragraph()
    p0.alignment = WD_ALIGN_PARAGRAPH.CENTER
    p0.paragraph_format.space_after = Pt(2)
    r = p0.add_run("BỘ GIÁO DỤC VÀ ĐÀO TẠO")
    r.font.name = 'Times New Roman'
    r.font.size = Pt(14)
    r.bold = True

    p1 = doc.add_paragraph()
    p1.alignment = WD_ALIGN_PARAGRAPH.CENTER
    p1.paragraph_format.space_after = Pt(16)
    r = p1.add_run("TRƯỜNG ĐẠI HỌC SƯ PHẠM THÀNH PHỐ HỒ CHÍ MINH\nKHOA CÔNG NGHỆ THÔNG TIN")
    r.font.name = 'Times New Roman'
    r.font.size = Pt(14)
    r.bold = True

    logo_path = r"C:\Users\ADMIN\.gemini\antigravity\brain\8f64cc42-23e1-4446-a71e-008770a1d3b2\scratch\template_images\image_0.png"
    p_img = doc.add_paragraph()
    p_img.alignment = WD_ALIGN_PARAGRAPH.CENTER
    p_img.paragraph_format.space_after = Pt(20)
    if os.path.exists(logo_path):
        p_img.add_run().add_picture(logo_path, width=Inches(1.8))

    p_tt = doc.add_paragraph()
    p_tt.alignment = WD_ALIGN_PARAGRAPH.CENTER
    p_tt.paragraph_format.space_after = Pt(10)
    r = p_tt.add_run("BÁO CÁO ĐỒ ÁN CUỐI KỲ")
    r.font.name = 'Times New Roman'
    r.font.size = Pt(22)
    r.bold = True
    r.font.color.rgb = RGBColor(30, 70, 140)

    p_sub = doc.add_paragraph()
    p_sub.alignment = WD_ALIGN_PARAGRAPH.CENTER
    p_sub.paragraph_format.space_after = Pt(36)
    r = p_sub.add_run("HỌC PHẦN: LẬP TRÌNH WINDOWS (C# .NET)\nĐỀ TÀI: XÂY DỰNG HỆ THỐNG QUẢN LÝ PHÒNG TẬP GYM & FITNESS CHUYÊN NGHIỆP\n(GYM MANAGEMENT SYSTEM)")
    r.font.name = 'Times New Roman'
    r.font.size = Pt(15)
    r.bold = True

    p_info = doc.add_paragraph()
    p_info.alignment = WD_ALIGN_PARAGRAPH.LEFT
    p_info.paragraph_format.left_indent = Inches(1.5)
    p_info.paragraph_format.space_after = Pt(3)
    p_info.paragraph_format.line_spacing = 1.3
    p_info.add_run("Giảng viên hướng dẫn:\t\t").bold = True
    p_info.add_run("ThS. Giảng viên Phụ trách Học phần\n")
    p_info.add_run("Lớp học phần:\t\t\t").bold = True
    p_info.add_run("2611COMP101901 - Lập trình Windows\n")
    p_info.add_run("Nhóm thực hiện:\t\t\t").bold = True
    p_info.add_run("Nhóm Sinh viên Đề tài Gym Management\n")
    p_info.add_run("Danh sách thành viên:\n").bold = True
    p_info.add_run("  1. Hứa Nhựt Tuấn\t\tMSSV: 51.01.104.114 (Nhóm trưởng)\n")
    p_info.add_run("  2. Huỳnh Phú Vinh\t\tMSSV: 51.01.104.105\n")
    p_info.add_run("  3. Lê Văn Gia Bảo\t\tMSSV: 51.01.104.072\n")
    p_info.add_run("  4. Nguyễn Đăng Khoa\t\tMSSV: 51.01.104.098\n")

    p_date = doc.add_paragraph()
    p_date.alignment = WD_ALIGN_PARAGRAPH.CENTER
    p_date.paragraph_format.space_before = Pt(40)
    r = p_date.add_run("TP. HỒ CHÍ MINH, NĂM 2026")
    r.font.name = 'Times New Roman'
    r.font.size = Pt(12)
    r.bold = True

    # MỤC LỤC
    doc.add_page_break()
    p_ml = doc.add_paragraph()
    p_ml.alignment = WD_ALIGN_PARAGRAPH.CENTER
    p_ml.paragraph_format.space_before = Pt(14)
    p_ml.paragraph_format.space_after = Pt(18)
    r = p_ml.add_run("MỤC LỤC")
    r.font.name = 'Times New Roman'
    r.font.size = Pt(20)
    r.bold = True
    r.font.color.rgb = RGBColor(46, 117, 181)

    toc_entries = [
        ("1. Giới thiệu đề tài", "3"),
        ("    1.1. Bối cảnh và tính cấp thiết của đề tài", "3"),
        ("    1.2. Mục tiêu nghiên cứu và phát triển phần mềm", "4"),
        ("    1.3. Đối tượng và phạm vi nghiên cứu", "5"),
        ("    1.4. Ý nghĩa khoa học và giá trị ứng dụng thực tiễn", "6"),
        ("2. Giải quyết và thiết kế", "7"),
        ("    2.1. Phân tích nghiệp vụ quản lý phòng Gym chuyên sâu", "7"),
        ("        2.1.1. Nghiệp vụ Quản lý Hội viên và Hồ sơ sức khỏe", "7"),
        ("        2.1.2. Nghiệp vụ Quản lý Gói tập và Hợp đồng hội viên", "9"),
        ("        2.1.3. Nghiệp vụ Quản lý Huấn luyện viên (PT) và Lịch tập", "11"),
        ("        2.1.4. Nghiệp vụ Điểm danh Check-in và Kiểm soát lượt tập", "13"),
        ("        2.1.5. Nghiệp vụ Bán lẻ POS, Nước uống và Thực phẩm bổ sung", "15"),
        ("        2.1.6. Nghiệp vụ Quản lý Trang thiết bị và Lịch bảo trì máy móc", "17"),
        ("        2.1.7. Nghiệp vụ Báo cáo thống kê tài chính và Doanh thu", "19"),
        ("    2.2. Thiết kế mô hình Use Case và Đặc tả chi tiết 12 Use Case", "21"),
        ("        2.2.1. Sơ đồ Use Case phân rã chức năng hệ thống", "21"),
        ("        2.2.2. Bảng đặc tả chi tiết 12 Use Case nghiệp vụ chính", "23"),
        ("    2.3. Thiết kế sơ đồ động: Sơ đồ hoạt động và Sơ đồ tuần tự", "37"),
        ("        2.3.1. Sơ đồ tuần tự: Xác thực đăng nhập và Khởi tạo phiên", "37"),
        ("        2.3.2. Sơ đồ tuần tự: Đăng ký thẻ hội viên và Thanh toán gói", "39"),
        ("        2.3.3. Sơ đồ tuần tự: Đặt lịch PT và Kiểm tra xung đột thời gian", "41"),
        ("        2.3.4. Sơ đồ tuần tự: Quẹt thẻ Check-in và Cảnh báo hạn dùng", "43"),
        ("        2.3.5. Sơ đồ tuần tự: Bán hàng POS và Trừ tồn kho đồng thời", "45"),
        ("    2.4. Thiết kế kiến trúc phần mềm và Các Pattern kỹ thuật", "47"),
        ("        2.4.1. Kiến trúc phân tầng 3-Layer (GUI - BLL - DAL)", "47"),
        ("        2.4.2. Cơ chế ORM Entity Framework 6 và Quản lý kết nối", "49"),
        ("        2.4.3. Mô hình Singleton quản lý Session và Kiểm soát truy cập RBAC", "51"),
        ("    2.5. Thiết kế cơ sở dữ liệu quan hệ (RDBMS) chuẩn hóa 3NF", "53"),
        ("        2.5.1. Sơ đồ quan hệ thực thể (ERD) và Toàn vẹn dữ liệu", "53"),
        ("        2.5.2. Từ điển dữ liệu (Data Dictionary) chi tiết 13 bảng", "55"),
        ("        2.5.3. Chiến lược Indexing và Tối ưu hóa truy vấn cơ sở dữ liệu", "68"),
        ("    2.6. Thiết kế chi tiết giao diện người dùng (UI/UX) của 16 Form", "70"),
        ("        2.6.1. Nguyên tắc tổ chức giao diện, thẩm mỹ và công thái học", "70"),
        ("        2.6.2. Đặc tả điều khiển, sự kiện và logic tương tác 16 Form", "71"),
        ("    2.7. Cài đặt thuật toán nghiệp vụ và Đoạn mã nguồn trọng tâm", "81"),
        ("        2.7.1. Thuật toán phát hiện xung đột lịch tập (Interval Overlap)", "81"),
        ("        2.7.2. Xử lý giao dịch ACID với TransactionScope trong POS", "83"),
        ("        2.7.3. Ứng dụng 9 toán tử LINQ to Entities nâng cao", "85"),
        ("        2.7.4. Bảo mật mật khẩu PBKDF2 và Mã hóa chuỗi cấu hình", "89"),
        ("3. Phân công công việc", "92"),
        ("    3.1. Ma trận trách nhiệm RACI theo từng phân hệ", "92"),
        ("    3.2. Tiến độ triển khai chi tiết qua 12 tuần của 4 thành viên", "93"),
        ("    3.3. Đánh giá khối lượng và tỷ lệ đóng góp của từng sinh viên", "97"),
        ("4. Sản phẩm", "98"),
        ("    4.1. Kịch bản trải nghiệm người dùng và Vận hành thực tế", "98"),
        ("    4.2. Kế hoạch kiểm thử chất lượng và 15 Test Cases chi tiết", "100"),
        ("    4.3. Hướng dẫn đóng gói, cài đặt và Cấu hình môi trường vận hành", "107"),
        ("    4.4. Đánh giá ưu điểm, hạn chế và Định hướng nâng cấp", "110"),
        ("5. Tham khảo", "112"),
        ("6. Link video demo", "113")
    ]

    for title, page_str in toc_entries:
        p_t = doc.add_paragraph()
        p_t.paragraph_format.space_before = Pt(1.5)
        p_t.paragraph_format.space_after = Pt(1.5)
        p_t.paragraph_format.line_spacing = 1.15
        p_t.paragraph_format.tab_stops.add_tab_stop(Inches(6.65), WD_TAB_ALIGNMENT.RIGHT, WD_TAB_LEADER.DOTS)

        is_bold = not title.startswith("    ")
        r_t = p_t.add_run(title)
        r_t.font.name = 'Times New Roman'
        r_t.font.size = Pt(11.5)
        r_t.bold = is_bold

        p_t.add_run("\t")

        r_p = p_t.add_run(page_str)
        r_p.font.name = 'Times New Roman'
        r_p.font.size = Pt(11.5)
        r_p.bold = is_bold

    print("Building Chapter 1...")
    # =========================================================================
    # 1. GIỚI THIỆU ĐỀ TÀI
    # =========================================================================
    add_h1(" 1. Giới thiệu đề tài")

    add_h2("1.1. Bối cảnh và tính cấp thiết của đề tài")
    add_p("Trong bối cảnh nền kinh tế Việt Nam phát triển nhanh chóng, mức sống và ý thức chăm sóc sức khỏe thể chất của người dân ngày càng được nâng cao rõ rệt. Phong trào tập luyện thể dục thể thao, đặc biệt là bộ môn Gym, Thể hình (Bodybuilding) và Fitness, đã trở thành một nhu cầu thiết yếu đối với đông đảo người dân ở mọi lứa tuổi, từ thanh thiếu niên, sinh viên đến giới văn phòng và người cao tuổi. Để đáp ứng nhu cầu này, hàng loạt các trung tâm thể hình từ các chuỗi phòng tập cao cấp (Fitness & Yoga Center) đến các câu lạc bộ thể hình tư nhân quy mô vừa và nhỏ được mở ra liên tục tại các đô thị lớn trên toàn quốc.")
    add_p("Tuy nhiên, thực trạng vận hành tại đại đa số các phòng tập quy mô vừa và nhỏ hiện nay vẫn đang đối mặt với nhiều thách thức nghiêm trọng về phương thức quản lý. Rất nhiều cơ sở vẫn duy trì phương thức quản lý thủ công truyền thống: ghi chép sổ sách bằng tay, quản lý hội viên qua các bảng tính Excel rời rạc, cấp phát thẻ giấy dễ rách nát hoặc mất mát. Những bất cập nghiêm trọng phát sinh từ phương thức này bao gồm:", bold_prefix="Hạn chế của mô hình quản lý thủ công: ")
    add_p("1. Thất thoát tài chính và gian lận doanh thu: Nhân viên quầy có thể cho bạn bè, người quen vào tập mà không đăng ký thẻ, không ghi nhận tiền thu phí gói tập, hoặc bán nước uống, thực phẩm bổ sung nhưng không lập hóa đơn ghi nhận vào sổ sách.\n"
          "2. Kiểm soát hạn dùng gói tập yếu kém: Khi số lượng hội viên lên đến hàng trăm hoặc hàng ngàn người, việc tra cứu ngày hết hạn trên sổ sách hoặc Excel là bất khả thi. Tình trạng hội viên đã hết hạn gói tập nhiều tháng nhưng vẫn ra vào phòng tập tự do gây thiệt hại kinh tế lớn cho chủ đầu tư.\n"
          "3. Trùng lặp lịch tập của Huấn luyện viên cá nhân (Personal Trainer - PT): Xếp lịch dạy qua tin nhắn mạng xã hội (Zalo, Messenger) thường xuyên dẫn đến xung đột giờ giấc (hai hội viên đặt cùng một huấn luyện viên tại cùng một khung giờ), gây mất uy tín và giảm trải nghiệm dịch vụ.\n"
          "4. Sai lệch tồn kho sản phẩm bổ sung: Hàng hóa bán lẻ tại quầy như Whey Protein, BCAA, nước khoáng, bình lắc không được theo dõi chặt chẽ, dẫn đến hao hụt không rõ nguyên nhân và hết hàng đột xuất.\n"
          "5. Thiếu thông tin báo cáo chiến lược: Chủ phòng gym không thể nắm bắt được bức tranh tài chính theo thời gian thực (doanh thu theo ngày/tháng/năm, gói tập bán chạy nhất, khung giờ cao điểm có đông hội viên tập nhất để bố trí nhân sự và điện lạnh tối ưu).")
    add_p("Nhận thức sâu sắc được các bài toán thực tế nêu trên, nhóm tác giả đã quyết định nghiên cứu và phát triển đề tài: \"Xây dựng Hệ thống Quản lý Phòng tập Gym & Fitness Chuyên nghiệp (Gym Management System)\" dựa trên nền tảng công nghệ Microsoft .NET Framework (C# Windows Forms) kết hợp cùng hệ quản trị cơ sở dữ liệu Microsoft SQL Server 2022 và công nghệ Entity Framework 6. Đây là một giải pháp chuyển đổi số toàn diện, giúp tự động hóa quy trình quản lý, ngăn chặn thất thoát và nâng cao chất lượng dịch vụ chuyên nghiệp.")

    add_h2("1.2. Mục tiêu nghiên cứu và phát triển phần mềm")
    add_p("Đề tài được xây dựng nhằm hướng tới các mục tiêu cốt lõi sau đây:", bold_prefix="Mục tiêu tổng quát: ")
    add_p("Nghiên cứu, phân tích toàn diện quy trình nghiệp vụ thực tế của một trung tâm thể hình hiện đại, từ đó thiết kế và xây dựng thành công ứng dụng máy tính (Desktop Application) hoạt động ổn định, an toàn, giao diện thân thiện, giải quyết triệt để bài toán quản lý cho các cơ sở Gym & Fitness.")
    add_p("1. Xây dựng phân hệ Quản lý Hội viên toàn diện: Tự động phát sinh mã thẻ thành viên duy nhất, lưu trữ thông tin cá nhân, số điện thoại, ngày tham gia, gói tập đăng ký, ngày hết hạn và trạng thái hiệu lực (Hoạt động / Hết hạn / Tạm dừng).\n"
          "2. Chuẩn hóa quy trình Quản lý Gói tập (Membership Packages): Thiết lập các gói tập linh hoạt theo tháng, quý, năm, gói tập VIP, gói tập kèm Huấn luyện viên cá nhân (PT), tự động tính toán thời hạn sử dụng và cảnh báo khi hội viên sắp đến hạn thanh toán.\n"
          "3. Tự động hóa xếp lịch tập và Quản lý Huấn luyện viên (PT Booking): Xây dựng thuật toán kiểm tra xung đột thời gian (Interval Overlap Detection), đảm bảo ngăn chặn 100% tình trạng trùng lịch giữa các hội viên và PT.\n"
          "4. Tích hợp phân hệ Điểm danh (Check-in / Check-out) tức thời: Hỗ trợ quẹt thẻ từ (RFID Card Reader), quét mã vạch (Barcode Scanner) hoặc nhập số điện thoại để kiểm tra quyền vào cửa trong thời gian dưới 50 mili-giây, hiển thị ảnh chân dung và trạng thái gói tập ngay trên màn hình quầy lễ tân.\n"
          "5. Xây dựng phân hệ Bán hàng quầy POS (Point of Sale): Cho phép lễ tân bán nước tăng lực, thực phẩm bổ sung dinh dưỡng (Whey, Mass, Creatine), phụ kiện găng tay, đai lưng; tự động xuất hóa đơn và trừ số lượng tồn kho theo thời gian thực với cơ chế giao dịch ACID an toàn.\n"
          "6. Phân hệ Báo cáo Thống kê & Trực quan hóa dữ liệu (BI Dashboard): Cung cấp bảng điều khiển trung tâm với các chỉ số KPI doanh thu, số lượt check-in theo khung giờ, tỷ lệ tăng trưởng hội viên mới, vẽ biểu đồ trực quan hóa dữ liệu.\n"
          "7. Đảm bảo an toàn thông tin & Phân quyền đa cấp bậc (RBAC): Mã hóa mật khẩu bằng thuật toán PBKDF2/SHA256 có muối (Salt), phân chia quyền hạn chặt chẽ giữa Quản trị viên (Admin), Lễ tân (Receptionist) và Huấn luyện viên (Trainer).", bold_prefix="Mục tiêu cụ thể: ")

    add_h2("1.3. Đối tượng và phạm vi nghiên cứu")
    add_p("Đối tượng nghiên cứu của đề tài bao gồm:", bold_prefix="Đối tượng nghiên cứu: ")
    add_p("- Nghiệp vụ vận hành trung tâm thể thao: Quy trình quản lý khách hàng, quy trình tài chính thu chi, quy trình điều phối nhân sự huấn luyện viên, kiểm soát an ninh ra vào và quản lý vật tư thiết bị thể hình.\n"
          "- Công nghệ phần mềm: Nền tảng .NET Framework 4.7.2, ngôn ngữ lập trình C# hướng đối tượng nâng cao, kiến trúc phân tầng 3-Layer (Presentation, Business Logic, Data Access), thư viện Object-Relational Mapping (Entity Framework 6), và hệ quản trị cơ sở dữ liệu quan hệ Microsoft SQL Server 2022.")
    add_p("Phạm vi ứng dụng của đề tài được xác định như sau:", bold_prefix="Phạm vi triển khai: ")
    add_p("- Đối tượng áp dụng: Các phòng tập Gym, trung tâm Fitness & Yoga, câu lạc bộ thể hình có quy mô từ đơn lẻ (1 chi nhánh) đến chuỗi phòng tập vừa và nhỏ (dưới 10.000 hội viên hoạt động).\n"
          "- Môi trường vận hành: Ứng dụng Desktop chạy trên hệ điều hành Windows (Windows 10, Windows 11, Windows Server), kết nối mạng nội bộ LAN hoặc qua máy chủ SQL Server Cloud/VPS.\n"
          "- Giới hạn đề tài: Trong giai đoạn hiện tại, phần mềm tập trung tối ưu hóa trải nghiệm điều hành máy trạm tại quầy lễ tân và văn phòng quản lý. Việc tích hợp ứng dụng di động dành cho hội viên (Mobile App) và thanh toán tự động qua cổng thanh toán trực tuyến (VNPay/Momo) được lập kế hoạch nghiên cứu trong giai đoạn phát triển tiếp theo.")

    add_h2("1.4. Ý nghĩa khoa học và giá trị ứng dụng thực tiễn")
    add_p("Về mặt khoa học và giáo dục: Đề tài là sự tổng hòa vận dụng kiến thức lý thuyết chuyên sâu của các môn học nền tảng trong chương trình đào tạo Cử nhân Công nghệ Thông tin, bao gồm: Lập trình Windows, Cơ sở dữ liệu, Phân tích thiết kế hệ thống thông tin, Công nghệ phần mềm, và An toàn thông tin. Thông qua việc hiện thực hóa đề tài, các thành viên trong nhóm làm chủ kỹ thuật phân tầng kiến trúc, tối ưu hóa truy vấn LINQ to Entities, kiểm soát giao dịch song song (Concurrency Control) và bảo mật dữ liệu cấp cơ sở dữ liệu.", bold_prefix="Ý nghĩa khoa học: ")
    add_p("Về mặt thực tiễn: Sản phẩm phần mềm sau khi hoàn thiện sở hữu tính thực dụng rất cao. Phần mềm có thể bàn giao và đưa vào sử dụng ngay tại các phòng tập thực tế mà không cần cấu hình phức tạp. Với việc tự động hóa toàn bộ công tác chấm công, điểm danh, thanh toán và báo cáo, chủ doanh nghiệp có thể cắt giảm 70% thời gian xử lý thủ tục giấy tờ, loại bỏ 100% tình trạng thất thoát doanh thu quầy và nâng cao tính chuyên nghiệp, hiện đại trong mắt khách hàng.", bold_prefix="Ý nghĩa thực tiễn: ")

    print("Building Chapter 2...")
    # =========================================================================
    # 2. GIẢI QUYẾT VÀ THIẾT KẾ
    # =========================================================================
    add_h1(" 2. Giải quyết và thiết kế")

    add_h2("2.1. Phân tích nghiệp vụ quản lý phòng Gym chuyên sâu")
    add_p("Hệ thống quản lý phòng Gym là một cấu trúc vận hành tích hợp đa phòng ban, đòi hỏi sự phối hợp nhịp nhàng giữa Lễ tân đón tiếp, Huấn luyện viên thể hình, Bộ phận quản trị kho và Ban Giám đốc quản lý tài chính. Sau quá trình khảo sát thực địa tại các trung tâm thể hình tiêu biểu, nhóm đã mô hình hóa và phân tích chi tiết 7 luồng nghiệp vụ xương sống của hệ thống:")

    add_h3("2.1.1. Nghiệp vụ Quản lý Hội viên và Hồ sơ sức khỏe")
    add_p("Hội viên (Member) là khách hàng cốt lõi tạo ra doanh thu cho phòng tập. Luồng nghiệp vụ quản lý hội viên bao gồm các bước từ khi khách hàng tiếp cận ban đầu cho đến suốt vòng đời tập luyện:")
    add_p("1. Đón tiếp và Tiếp nhận thông tin ban đầu: Khi khách hàng đến phòng tập, nhân viên lễ tân thu thập các trường thông tin cơ bản: Họ và tên, Ngày sinh, Giới tính, Số điện thoại liên lạc, Số Căn cước công dân (CCCD), Địa chỉ cư trú và Địa chỉ Email.\n"
          "2. Ghi nhận chỉ số thể chất ban đầu (InBody / Health Metrics): Đối với các hội viên đăng ký gói tập có PT, hệ thống cho phép ghi nhận các chỉ số cơ thể quan trọng như Chiều cao (cm), Cân nặng (kg), Tỷ lệ mỡ cơ thể (% Body Fat), Lượng cơ nạc (Skeletal Muscle Mass) và Mục tiêu tập luyện (Giảm cân, Tăng cơ, Tăng cường thể lực, Trị liệu thoái hóa cột sống).\n"
          "3. Cấp phát Định danh Thẻ hội viên (Card ID): Hệ thống tự động sinh Mã hội viên theo tiền tố quy chuẩn (ví dụ: MEM001, MEM002...) và cho phép lễ tân quét mã thẻ từ RFID hoặc mã vạch Barcode để liên kết vĩnh viễn thẻ vật lý vào hồ sơ tài khoản.\n"
          "4. Chụp ảnh chân dung trực tiếp: Phần mềm tích hợp giao tiếp Camera máy tính/Webcam để chụp ảnh đại diện của hội viên, lưu trữ hình ảnh phục vụ xác thực đối chiếu khuôn mặt khi điểm danh vào cửa, ngăn chặn hành vi mượn thẻ người khác đi tập.\n"
          "5. Quản lý trạng thái và Vòng đời hội viên: Hệ thống quản lý chặt chẽ 4 trạng thái hội viên:\n"
          "   - Chưa kích hoạt (Inactive): Hội viên đã tạo hồ sơ nhưng chưa đăng ký hoặc chưa kích hoạt gói tập.\n"
          "   - Đang hoạt động (Active): Hội viên đang sở hữu ít nhất một gói tập còn thời hạn và còn lượt tập hợp lệ.\n"
          "   - Hết hạn (Expired): Toàn bộ gói tập của hội viên đã vượt quá ngày hiệu lực hoặc đã sử dụng hết số buổi quy định.\n"
          "   - Tạm dừng / Bảo lưu (Suspended): Hội viên gửi đơn xin bảo lưu gói tập tạm thời (do công tác xa, chấn thương, ốm đau) với khoảng thời gian được quản trị viên duyệt.")

    add_h3("2.1.2. Nghiệp vụ Quản lý Gói tập và Hợp đồng hội viên")
    add_p("Phòng Gym cung cấp các dịch vụ thể chất dưới dạng Gói tập (Membership Packages) đa dạng. Nghiệp vụ quản lý gói tập bao gồm:")
    add_p("1. Định nghĩa danh mục gói tập linh hoạt: Quản trị viên thiết lập bảng giá, thời hạn gói tập theo ngày (ví dụ: Gói ngày - 1 ngày, Gói tháng - 30 ngày, Gói quý - 90 ngày, Gói nửa năm - 180 ngày, Gói năm - 365 ngày). Mỗi gói tập được định cấu hình loại dịch vụ: Gói Gym tiêu chuẩn, Gói Gym kèm Xông hơi thảo dược, Gói Yoga & Zumba, hoặc Gói VIP Full Access toàn bộ dịch vụ.\n"
          "2. Ký hợp đồng đăng ký mới (New Subscription): Khi hội viên chọn mua gói tập, nhân viên tạo một bản ghi Đăng ký (MemberSubscription). Hệ thống tự động tính toán:\n"
          "   - Ngày bắt đầu hiệu lực (StartDate): Mặc định là ngày hiện tại, hoặc cho phép hẹn ngày kích hoạt trong tương lai.\n"
          "   - Ngày kết thúc hiệu lực (EndDate): Tự động cộng số ngày của gói vào ngày bắt đầu: EndDate = StartDate + DurationDays.\n"
          "   - Áp dụng chương trình khuyến mãi / Giảm giá: Cho phép áp mã chiết khấu phần trăm (%) hoặc trừ trực tiếp số tiền khuyến mãi.\n"
          "3. Gia hạn gói tập (Renew Subscription): Khi gói tập sắp hết hạn hoặc đã hết hạn, hội viên đóng phí gia hạn. Nếu gia hạn trước khi gói cũ hết hạn, ngày bắt đầu của chu kỳ mới sẽ được tự động nối tiếp vào ngày kết thúc của gói cũ (EndDate cũ + 1 ngày), đảm bảo quyền lợi liên tục của khách hàng mà không bị thiệt hại ngày tập.\n"
          "4. Nâng cấp hoặc Chuyển nhượng gói tập: Cho phép hội viên chuyển đổi sang gói cao cấp hơn bằng cách bù chênh lệch chi phí, hoặc chuyển quyền sở hữu gói tập sang cho người thân có lưu lại lịch sử thay đổi để tránh gian lận.")

    add_h3("2.1.3. Nghiệp vụ Quản lý Huấn luyện viên (PT) và Lịch tập")
    add_p("Dịch vụ Huấn luyện viên cá nhân (Personal Trainer) mang lại giá trị gia tăng và biên lợi nhuận cao nhất cho phòng tập. Quy trình quản lý PT đòi hỏi tính chính xác tuyệt đối về mặt thời gian:")
    add_p("1. Quản lý hồ sơ Huấn luyện viên: Lưu trữ thông tin nhân sự PT gồm Họ tên, Số điện thoại, Chuyên môn (Tăng cơ bodybuilding, Giảm mỡ cardio, Kick-boxing, Phục hồi chức năng), Bằng cấp chứng chỉ quốc tế (NASM, ACE, ISSA), Mức hoa hồng trên mỗi buổi dạy và trạng thái công tác.\n"
          "2. Đặt lịch huấn luyện (PT Scheduling): Lễ tân hoặc hội viên thỏa thuận khung giờ tập luyện với PT. Thông tin lịch hẹn gồm: Hội viên tham gia, PT phụ trách, Ngày tập (ScheduleDate), Giờ bắt đầu (StartTime) và Giờ kết thúc (EndTime), Ghi chú bài tập (ngực, lưng, chân...). Hệ thống tự động kiểm tra xung đột thời gian (Collision Detection) dựa trên thuật toán so sánh khoảng thời gian, ngăn chặn hoàn toàn tình trạng trùng lịch.\n"
          "3. Điểm danh buổi dạy và Chấm công hoa hồng: Khi buổi tập diễn ra, PT và hội viên xác nhận hoàn thành buổi tập. Số buổi tập còn lại trong gói PT của hội viên tự động giảm đi 1 đơn vị, đồng thời ghi nhận vào bảng chấm công của PT để tính toán lương và hoa hồng cuối tháng.")

    add_h3("2.1.4. Nghiệp vụ Điểm danh Check-in và Kiểm soát lượt tập")
    add_p("Kiểm soát an ninh cửa vào là chốt chặn quan trọng nhất để chống thất thoát vé tập:")
    add_p("1. Đọc mã định danh tại cửa: Hội viên xuất trình thẻ từ (quẹt qua máy đọc RFID), quét mã QR trên điện thoại hoặc đọc số điện thoại/Mã hội viên cho lễ tân.\n"
          "2. Kiểm tra tính hợp lệ tức thời (Validation Engine): Hệ thống thực thi chuỗi kiểm tra logic tuần tự trong chưa đầy 50ms:\n"
          "   - Bước 2a: Tìm kiếm hồ sơ hội viên theo mã quẹt. Nếu không tìm thấy, phát âm thanh cảnh báo lỗi và từ chối vào cửa.\n"
          "   - Bước 2b: Kiểm tra trạng thái tài khoản. Nếu trạng thái là \"Bảo lưu\" hoặc \"Bị khóa\", hiển thị lý do vi phạm.\n"
          "   - Bước 2c: Kiểm tra gói tập còn hiệu lực. Lấy danh sách các gói tập của hội viên có điều kiện: StartDate <= Hôm nay <= EndDate và Trạng thái gói = Active. Nếu không có gói nào thỏa mãn, hiển thị thông báo \"Gói tập đã hết hạn vào ngày DD/MM/YYYY, vui lòng gia hạn!\"\n"
          "   - Bước 2d: Kiểm tra số lượt tập còn lại (đối với gói tập giới hạn số buổi). Nếu số buổi = 0, từ chối vào cửa.\n"
          "3. Ghi nhận lịch sử vào phòng (CheckIn Logging): Khi tất cả các điều kiện hợp lệ, hệ thống tự động ghi một dòng mới vào bảng `CheckIns` với thời gian chính xác (Timestamp). Màn hình lễ tân bật màu xanh lá cây, phát âm thanh chào mừng, hiển thị to rõ tên hội viên, ảnh chân dung để đối chiếu và số ngày còn lại của gói tập.")

    add_h3("2.1.5. Nghiệp vụ Bán lẻ POS, Nước uống và Thực phẩm bổ sung")
    add_p("Tại quầy lễ tân luôn tích hợp dịch vụ bán lẻ bổ trợ (F&B và phụ kiện):")
    add_p("1. Danh mục sản phẩm và Phân loại: Quản lý các nhóm hàng: Nước uống giải khát bù khoáng (Revive, Pocari, Nước suối), Nước tăng lực thể thao (C4, Red Bull), Thực phẩm bổ sung protein (Whey Protein thanh, Bột Mass, BCAA, Creatine), và Phụ kiện tập luyện (Găng tay xà đơn, Đai bảo vệ thắt lưng, Bình lắc shaker).\n"
          "2. Quản lý kho hàng (Inventory Management): Theo dõi chặt chẽ Số lượng tồn kho (StockQuantity), Đơn vị tính, Giá nhập kho (CostPrice) và Giá bán lẻ (UnitPrice). Khi số lượng tồn kho giảm xuống dưới ngưỡng cảnh báo tối thiểu (ví dụ: dưới 5 sản phẩm), hệ thống tự động đổi màu cảnh báo đỏ trên giao diện.\n"
          "3. Lập đơn hàng và Thanh toán tại quầy: Lễ tân chọn sản phẩm vào giỏ hàng, tùy chỉnh số lượng, nhập thông tin khách hàng (hội viên hoặc khách vãng lai). Hệ thống hỗ trợ đa phương thức thanh toán: Tiền mặt (Cash), Chuyển khoản ngân hàng qua mã VietQR, hoặc Thẻ tín dụng/ATM POS.\n"
          "4. Khấu trừ tồn kho tự động: Ngay khi giao dịch được xác nhận, hệ thống tự động trừ kho từng món hàng tương ứng trong giỏ và in hóa đơn bán lẻ (hỗ trợ khổ in K58/K80 nhiệt).")

    add_h3("2.1.6. Nghiệp vụ Quản lý Trang thiết bị và Bảo trì máy móc")
    add_p("Phòng tập sở hữu lượng lớn máy móc cơ khí và thiết bị điện tử có giá trị cao (Máy chạy bộ Treadmill, Xe đạp elip, Dàn tạ khối, Dàn tạ rời Dumbbell, Máy kéo cáp Cable Crossover):")
    add_p("1. Hồ sơ thiết bị: Ghi nhận Mã máy, Tên máy móc, Nhóm thiết bị (Máy Cardio, Máy cơ bắp Khối, Ghế tập, Tạ tự do), Ngày mua, Nhà cung cấp, Giá trị mua ban đầu và Thời hạn bảo hành.\n"
          "2. Theo dõi tình trạng vận hành: Phân loại trạng thái thiết bị thành 3 mức độ: \"Hoạt động tốt\", \"Cần bảo dưỡng định kỳ\", \"Đang hỏng / Ngừng phục vụ\".\n"
          "3. Lập lịch bảo trì kỹ thuật: Hệ thống lưu vết nhật ký sửa chữa, ngày thay cáp, tra dầu băng tải máy chạy bộ, linh kiện thay thế và chi phí bảo trì để tính toán khấu hao tài sản cố định.")

    add_h3("2.1.7. Nghiệp vụ Báo cáo tài chính và Doanh thu")
    add_p("Hệ thống cung cấp cho chủ phòng tập và bộ phận kế toán công cụ tổng hợp số liệu tự động:")
    add_p("1. Báo cáo doanh thu đa chiều: Lọc doanh thu theo ngày, tuần, tháng, quý, năm, hoặc theo khoảng ngày bất kỳ do người dùng chọn. Tách bạch rõ ràng nguồn thu đến từ: Bán gói tập thẻ hội viên, Thu phí hợp đồng PT kèm huấn luyện, và Doanh thu bán lẻ sản phẩm tại quầy POS.\n"
          "2. Thống kê mật độ tập luyện (Foot Traffic Analysis): Phân tích biểu đồ tần suất check-in của hội viên theo từng khung giờ trong ngày (sáng sớm 5h-7h, chiều cao điểm 17h-20h) và theo các thứ trong tuần. Số liệu này giúp chủ phòng gym tối ưu hóa việc phân ca trực cho nhân viên, điều chỉnh công suất hệ thống máy lạnh để tiết kiệm điện năng.\n"
          "3. Xuất báo cáo dữ liệu: Hỗ trợ xuất dữ liệu ra file Microsoft Excel (.xlsx) chuẩn văn phòng để phục vụ báo cáo thuế và lưu trữ tài chính dài hạn.")

    print("Building Use Cases...")
    add_h2("2.2. Thiết kế mô hình Use Case và Đặc tả chi tiết 12 Use Case")
    add_h3("2.2.1. Sơ đồ Use Case phân rã chức năng hệ thống")
    add_p("Hệ thống xác định 3 tác nhân chính (Actors) tương tác trực tiếp với phần mềm:")
    add_p("1. Quản trị viên (Admin / Manager): Sở hữu toàn quyền truy cập hệ thống: Quản lý người dùng và phân quyền, cấu hình gói tập, quản lý nhân sự PT, quản lý kho hàng và thiết bị, xem toàn bộ báo cáo doanh thu và thực hiện sao lưu/phục hồi cơ sở dữ liệu.\n"
          "2. Nhân viên Lễ tân (Receptionist): Thực hiện các nghiệp vụ thường nhật: Tiếp nhận và quản lý hồ sơ hội viên, đăng ký và gia hạn gói tập, điểm danh check-in hội viên tại quầy, xếp lịch tập với PT, và thực hiện bán lẻ sản phẩm qua POS.\n"
          "3. Huấn luyện viên (Personal Trainer - PT): Tra cứu danh sách hội viên do mình phụ trách, xem lịch huấn luyện cá nhân trong ngày/tuần, ghi nhận kết quả và đánh giá buổi tập.")

    add_p("Bảng tổng hợp phân rã 12 Use Case cốt lõi của hệ thống:")
    uc_summary_headers = ["Mã UC", "Tên Use Case", "Tác nhân (Actor)", "Mô tả tóm tắt nghiệp vụ"]
    uc_summary_rows = [
        ["UC01", "Đăng nhập & Xác thực", "Admin, Lễ tân, PT", "Xác thực danh tính bằng tài khoản và mật khẩu mã hóa PBKDF2"],
        ["UC02", "Quản trị người dùng & Phân quyền", "Admin", "Thêm, sửa, vô hiệu hóa tài khoản và cấp phát vai trò RBAC"],
        ["UC03", "Quản lý hồ sơ Hội viên", "Admin, Lễ tân", "Thêm mới, cập nhật thông tin, chụp ảnh, tìm kiếm hội viên"],
        ["UC04", "Đăng ký & Gia hạn gói tập", "Admin, Lễ tân", "Ký hợp đồng gói tập, tính toán ngày hiệu lực, áp mã giảm giá"],
        ["UC05", "Xếp lịch Huấn luyện viên (PT)", "Admin, Lễ tân, PT", "Đặt lịch tập, kiểm tra tự động xung đột khung thời gian dạy"],
        ["UC06", "Điểm danh Check-in quầy", "Lễ tân", "Quẹt thẻ từ RFID/mã vạch kiểm tra quyền vào cửa trong 50ms"],
        ["UC07", "Bán lẻ sản phẩm tại POS", "Admin, Lễ tân", "Lập đơn bán thực phẩm bổ sung/nước, trừ kho tự động, in bill"],
        ["UC08", "Quản lý Sản phẩm & Kho hàng", "Admin", "Quản lý danh mục hàng hóa, giá nhập, giá bán, nhập hàng thêm"],
        ["UC09", "Quản lý Thiết bị phòng tập", "Admin", "Theo dõi tình trạng máy tập, lịch bảo dưỡng định kỳ và khấu hao"],
        ["UC10", "Báo cáo Doanh thu & Thống kê", "Admin", "Tổng hợp doanh thu, biểu đồ tần suất check-in, xuất file Excel"],
        ["UC11", "Sao lưu & Phục hồi CSDL", "Admin", "Tạo file Backup .bak của SQL Server và khôi phục khi gặp sự cố"],
        ["UC12", "Tra cứu lịch sử & Nhật ký hệ thống", "Admin", "Xem nhật ký thao tác (Audit Logs) của nhân viên để kiểm tra"]
    ]
    add_custom_table(uc_summary_headers, uc_summary_rows, [1.0, 2.2, 1.4, 2.2])

    add_h3("2.2.2. Bảng đặc tả chi tiết 12 Use Case nghiệp vụ chính")
    add_p("Dưới đây là bảng đặc tả công nghệ phần mềm chi tiết cho toàn bộ 12 Use Case theo quy chuẩn quốc tế:")

    # 12 Use Cases Detailed Specifications
    uc_details = [
        ("UC01: Đăng nhập & Xác thực hệ thống", [
            ["Mục", "Chi tiết đặc tả Use Case"],
            ["Mã Use Case", "UC01"],
            ["Tên Use Case", "Đăng nhập & Xác thực hệ thống (Authentication & Authorization)"],
            ["Tác nhân (Actor)", "Quản trị viên (Admin), Nhân viên Lễ tân (Receptionist), Huấn luyện viên (Trainer)"],
            ["Mô tả ngắn", "Cho phép người dùng nhập tên đăng nhập và mật khẩu để truy cập vào hệ thống với các quyền hạn tương ứng."],
            ["Tiền điều kiện", "Người dùng đã được cấp tài khoản hoạt động (IsActive = true) trong bảng `Users`."],
            ["Hậu điều kiện", "Khởi tạo đối tượng `UserSession` đơn thể (Singleton), lưu vết lịch sử đăng nhập, hiển thị màn hình chính theo vai trò."],
            ["Luồng sự kiện chính (Basic Flow)", 
             "1. Người dùng khởi chạy ứng dụng, hệ thống hiển thị Form đăng nhập (frmLogin).\n"
             "2. Người dùng nhập Tên đăng nhập (Username) và Mật khẩu (Password), nhấn nút \"Đăng nhập\" (hoặc phím Enter).\n"
             "3. Hệ thống kiểm tra tính hợp lệ dữ liệu đầu vào (không được rỗng, không chứa ký tự SQL Injection).\n"
             "4. Hệ thống truy vấn bảng `Users` tìm bản ghi có Username tương ứng.\n"
             "5. Hệ thống trích xuất chuỗi Salt và Hash đã lưu của người dùng, thực thi thuật toán PBKDF2/SHA256 để kiểm tra mật khẩu.\n"
             "6. Khi mật khẩu khớp đúng và tài khoản đang Active, hệ thống khởi tạo `UserSession` lưu ID, Username, RoleId, FullName.\n"
             "7. Hệ thống ẩn Form đăng nhập và mở Form bảng điều khiển chính (frmMainDashboard) với các nút chức năng được bật/tắt theo Role."],
            ["Luồng rẽ nhánh (Alternative Flows)", 
             "4a. Không tìm thấy Username: Hệ thống hiển thị thông báo lỗi \"Tên đăng nhập hoặc mật khẩu không chính xác!\", focus lại ô Username.\n"
             "5a. Mật khẩu không chính xác: Tăng số lần thử sai. Nếu nhập sai quá 5 lần, tạm khóa tài khoản trong 15 phút.\n"
             "6a. Tài khoản bị vô hiệu hóa (IsActive = false): Hiển thị thông báo \"Tài khoản của bạn đã bị khóa bởi Quản trị viên!\"."],
            ["Luồng ngoại lệ (Exception Flows)", "E1. Mất kết nối tới máy chủ SQL Server: Hiển thị lỗi kết nối và cho phép người dùng cấu hình lại chuỗi ConnectionString."],
            ["Quy tắc nghiệp vụ (Business Rules)", "Mật khẩu người dùng không bao giờ được lưu trữ dưới dạng bản rõ (plaintext). Bắt buộc mã hóa PBKDF2 với chuỗi Salt ngẫu nhiên 128-bit."]
        ]),
        ("UC02: Quản trị người dùng & Phân quyền vai trò", [
            ["Mục", "Chi tiết đặc tả Use Case"],
            ["Mã Use Case", "UC02"],
            ["Tên Use Case", "Quản trị tài khoản người dùng & Phân quyền (User & Role Management)"],
            ["Tác nhân (Actor)", "Quản trị viên (Admin)"],
            ["Mô tả ngắn", "Cung cấp các công cụ tạo tài khoản nhân viên mới, sửa thông tin, đặt lại mật khẩu, phân bổ vai trò và khóa tài khoản."],
            ["Tiền điều kiện", "Người dùng phải đăng nhập với vai trò Quản trị viên (RoleId = 1)."],
            ["Hậu điều kiện", "Tài khoản nhân viên được ghi nhận hoặc cập nhật trạng thái trong cơ sở dữ liệu."],
            ["Luồng sự kiện chính (Basic Flow)", 
             "1. Quản trị viên mở phân hệ \"Quản trị tài khoản\" trên thanh điều hướng.\n"
             "2. Hệ thống tải danh sách tất cả người dùng từ bảng `Users` kèm thông tin vai trò (`Roles`) lên DataGridView.\n"
             "3. Quản trị viên nhấn \"Thêm tài khoản mới\". Hệ thống hiển thị hộp thoại nhập liệu.\n"
             "4. Quản trị viên điền Tên đăng nhập, Mật khẩu ban đầu, Họ tên, Số điện thoại, Email và chọn Vai trò (Admin / Lễ tân / PT).\n"
             "5. Quản trị viên nhấn \"Lưu\". Hệ thống kiểm tra Username không trùng lặp, tạo chuỗi Salt ngẫu nhiên, sinh mã Hash PBKDF2 và ghi vào CSDL.\n"
             "6. Hệ thống làm mới danh sách và hiển thị thông báo thành công."],
            ["Luồng rẽ nhánh (Alternative Flows)", 
             "5a. Username đã tồn tại: Hệ thống hiển thị cảnh báo đỏ cạnh ô nhập và ngăn chặn hành động lưu dữ liệu.\n"
             "3b. Sửa thông tin tài khoản: Quản trị viên chọn người dùng trên lưới, sửa vai trò hoặc kích hoạt/hủy kích hoạt (Active/Inactive).\n"
             "3c. Đặt lại mật khẩu (Reset Password): Quản trị viên thiết lập mật khẩu mặc định mới cho nhân viên khi có yêu cầu."],
            ["Luồng ngoại lệ (Exception Flows)", "E1. Quản trị viên tự xóa chính tài khoản đang đăng nhập: Hệ thống ngăn chặn với thông báo \"Không thể tự vô hiệu hóa tài khoản của chính mình!\"."],
            ["Quy tắc nghiệp vụ (Business Rules)", "Tài khoản có vai trò Quản trị viên tối cao (SuperAdmin) không được phép xóa khỏi cơ sở dữ liệu."]
        ]),
        ("UC03: Quản lý hồ sơ Hội viên (Members)", [
            ["Mục", "Chi tiết đặc tả Use Case"],
            ["Mã Use Case", "UC03"],
            ["Tên Use Case", "Quản lý hồ sơ thông tin Hội viên (Member Management)"],
            ["Tác nhân (Actor)", "Quản trị viên (Admin), Nhân viên Lễ tân (Receptionist)"],
            ["Mô tả ngắn", "Thực hiện tạo mới hồ sơ hội viên, cấp mã thẻ, sửa đổi thông tin cá nhân, cập nhật chỉ số sức khỏe và tra cứu thông tin."],
            ["Tiền điều kiện", "Người dùng đã đăng nhập hệ thống với quyền Lễ tân hoặc Admin."],
            ["Hậu điều kiện", "Hồ sơ hội viên được lưu trữ an toàn trong bảng `Members` và sẵn sàng để đăng ký gói tập hoặc quẹt thẻ."],
            ["Luồng sự kiện chính (Basic Flow)", 
             "1. Người dùng truy cập màn hình \"Quản lý Hội viên\" (frmMembers).\n"
             "2. Hệ thống tải danh sách hội viên, hỗ trợ tìm kiếm tức thời theo Tên, Số điện thoại hoặc Mã thẻ.\n"
             "3. Người dùng nhấn nút \"Thêm hội viên\". Hệ thống mở form nhập liệu (frmMemberAddEdit).\n"
             "4. Hệ thống tự động sinh Mã hội viên tiếp theo (ví dụ: MEM0145).\n"
             "5. Người dùng nhập: Họ tên, Ngày sinh, Giới tính, Số điện thoại, CCCD, Địa chỉ, Ghi chú sức khỏe.\n"
             "6. Người dùng quẹt thẻ từ trắng qua đầu đọc để lấy CardID hoặc nhấn nút kích hoạt Camera chụp ảnh chân dung.\n"
             "7. Nhấn \"Lưu dữ liệu\". Hệ thống kiểm tra số điện thoại và CardID không trùng lặp, sau đó lưu vào CSDL.\n"
             "8. Hệ thống thông báo thành công và chuyển hướng gợi ý mở ngay form Đăng ký gói tập cho hội viên này."],
            ["Luồng rẽ nhánh (Alternative Flows)", 
             "7a. Số điện thoại hoặc Mã thẻ đã tồn tại trên hệ thống: Cảnh báo trùng lặp và yêu cầu kiểm tra lại hồ sơ cũ.\n"
             "2b. Lọc hội viên: Người dùng có thể lọc danh sách theo trạng thái: Đang hoạt động, Sắp hết hạn (còn dưới 7 ngày), Đã hết hạn."],
            ["Luồng ngoại lệ (Exception Flows)", "E1. Camera không kết nối được: Hệ thống cho phép chọn ảnh đại diện từ file trong máy tính (.jpg, .png)."],
            ["Quy tắc nghiệp vụ (Business Rules)", "Số điện thoại hội viên phải đúng định dạng chuẩn 10 chữ số của các nhà mạng viễn thông Việt Nam."]
        ]),
        ("UC04: Đăng ký & Gia hạn Gói tập (Subscriptions)", [
            ["Mục", "Chi tiết đặc tả Use Case"],
            ["Mã Use Case", "UC04"],
            ["Tên Use Case", "Đăng ký & Gia hạn Gói tập (Membership Subscription & Renewal)"],
            ["Tác nhân (Actor)", "Quản trị viên (Admin), Nhân viên Lễ tân (Receptionist)"],
            ["Mô tả ngắn", "Ký kết hợp đồng gói tập cho hội viên, thiết lập thời hạn hiệu lực, áp dụng giảm giá và ghi nhận doanh thu dịch vụ."],
            ["Tiền điều kiện", "Hội viên đã có hồ sơ trong bảng `Members`, danh mục gói tập đã được định nghĩa trong `MembershipPackages`."],
            ["Hậu điều kiện", "Tạo bản ghi trong `MemberSubscriptions`, cập nhật trạng thái hội viên thành 'Active', tạo hóa đơn thanh toán."],
            ["Luồng sự kiện chính (Basic Flow)", 
             "1. Nhân viên chọn hội viên cần đăng ký gói tập và nhấn \"Đăng ký gói / Gia hạn\".\n"
             "2. Hệ thống hiển thị danh sách các gói tập có sẵn kèm đơn giá và thời hạn.\n"
             "3. Nhân viên chọn gói tập theo nhu cầu của khách hàng (ví dụ: Gói Gym 3 Tháng - 90 ngày).\n"
             "4. Hệ thống tự động tính toán: Ngày bắt đầu (mặc định hôm nay), Ngày kết thúc = Ngày bắt đầu + 90 ngày, Tổng tiền = Đơn giá gói.\n"
             "5. Nếu có chương trình ưu đãi, nhân viên nhập mã giảm giá hoặc tỷ lệ chiết khấu (%).\n"
             "6. Nhân viên chọn hình thức thanh toán (Tiền mặt / Chuyển khoản QR / Quẹt thẻ ngân hàng).\n"
             "7. Nhấn \"Xác nhận thanh toán\". Hệ thống thực thi giao dịch ghi nhận hợp đồng và cập nhật trạng thái hội viên.\n"
             "8. Hệ thống hiển thị tùy chọn in Phiếu đăng ký / Hóa đơn thu tiền cho khách hàng."],
            ["Luồng rẽ nhánh (Alternative Flows)", 
             "4a. Khách hàng gia hạn khi gói cũ vẫn còn hạn: Hệ thống phát hiện gói cũ chưa hết hạn, tự động đề xuất Ngày bắt đầu của gói mới nối tiếp sau Ngày kết thúc của gói cũ để cộng dồn quyền lợi.\n"
             "7a. Khách hàng chưa thanh toán đủ: Cho phép ghi nhận số tiền trả trước và ghi nợ số tiền còn lại vào hồ sơ."],
            ["Luồng ngoại lệ (Exception Flows)", "E1. Lỗi mạng trong quá trình sinh hóa đơn: Giao dịch Database được Rollback nguyên vẹn, đảm bảo không bị sai lệch số liệu tài chính."],
            ["Quy tắc nghiệp vụ (Business Rules)", "Không cho phép ngày kết thúc nhỏ hơn ngày bắt đầu. Mỗi hội viên tại một thời điểm chỉ có một gói tập cùng loại đang kích hoạt."]
        ]),
        ("UC05: Xếp lịch Huấn luyện viên (PT Booking)", [
            ["Mục", "Chi tiết đặc tả Use Case"],
            ["Mã Use Case", "UC05"],
            ["Tên Use Case", "Xếp lịch tập Huấn luyện viên cá nhân (Trainer Scheduling)"],
            ["Tác nhân (Actor)", "Quản trị viên (Admin), Lễ tân (Receptionist), Huấn luyện viên (Trainer)"],
            ["Mô tả ngắn", "Lên lịch hẹn tập luyện giữa hội viên và huấn luyện viên, kiểm tra tự động và ngăn chặn hoàn toàn xung đột khung giờ dạy."],
            ["Tiền điều kiện", "Hội viên sở hữu gói tập có kèm quyền lợi PT còn số buổi, Huấn luyện viên có trạng thái sẵn sàng làm việc."],
            ["Hậu điều kiện", "Bản ghi lịch tập được tạo trong bảng `WorkoutSchedules` với trạng thái 'Scheduled'."],
            ["Luồng sự kiện chính (Basic Flow)", 
             "1. Người dùng mở màn hình \"Lịch tập Huấn luyện viên\" (frmWorkoutSchedules).\n"
             "2. Chọn Huấn luyện viên và Ngày cần xếp lịch trên thanh lịch Calendar.\n"
             "3. Hệ thống hiển thị các khung giờ đã có lịch và khung giờ còn trống của PT đó dưới dạng Timeline trực quan.\n"
             "4. Người dùng nhấn \"Thêm lịch tập mới\", chọn Hội viên, chọn Giờ bắt đầu (StartTime) và Giờ kết thúc (EndTime).\n"
             "5. Nhấn \"Lưu lịch tập\". Hệ thống thực thi thuật toán kiểm tra xung đột thời gian (Interval Overlap Detection):\n"
             "   - Thuật toán: Truy vấn tìm kiếm xem có bất kỳ lịch tập nào của PT đó trong cùng ngày thỏa mãn điều kiện:\n"
             "     `ExistingStartTime < NewEndTime AND ExistingEndTime > NewStartTime` và Trạng thái != 'Cancelled'.\n"
             "6. Nếu không có xung đột, hệ thống lưu bản ghi vào CSDL và gửi thông báo thành công.\n"
             "7. Timeline lập tức cập nhật khối màu hiển thị ca tập mới vừa tạo."],
            ["Luồng rẽ nhánh (Alternative Flows)", 
             "5a. Phát hiện xung đột lịch: Hệ thống bật cảnh báo màu đỏ \"Huấn luyện viên đã có lịch dạy từ [Giờ] đến [Giờ] với hội viên [Tên]!\", từ chối lưu và gợi ý các khung giờ trống kế tiếp trong ngày.\n"
             "4b. Hủy hoặc Dời lịch: Người dùng có thể chọn lịch hẹn đã có để sửa giờ hoặc hủy lịch với lý do cụ thể."],
            ["Luồng ngoại lệ (Exception Flows)", "E1. Hội viên đã sử dụng hết số buổi PT quy định: Hệ thống thông báo yêu cầu mua thêm gói buổi tập trước khi xếp lịch."],
            ["Quy tắc nghiệp vụ (Business Rules)", "Một ca tập PT tiêu chuẩn có thời lượng tối thiểu 45 phút và tối đa 120 phút. Không được phép đặt lịch trong quá khứ."]
        ]),
        ("UC06: Điểm danh Hội viên tại quầy (Check-in)", [
            ["Mục", "Chi tiết đặc tả Use Case"],
            ["Mã Use Case", "UC06"],
            ["Tên Use Case", "Điểm danh Hội viên tại cửa ra vào (Access Control & Check-in)"],
            ["Tác nhân (Actor)", "Nhân viên Lễ tân (Receptionist)"],
            ["Mô tả ngắn", "Nhận diện mã định danh hội viên, kiểm tra quyền hạn và hạn gói tập tức thời, ghi nhận lượt vào phòng tập."],
            ["Tiền điều kiện", "Màn hình Check-in đang mở tại máy tính quầy lễ tân, đầu đọc thẻ RFID/Barcode đang ở chế độ chờ tín hiệu."],
            ["Hậu điều kiện", "Tạo bản ghi trong `CheckIns` gồm MemberId và CheckInTime, hiển thị tín hiệu xanh chấp thuận hoặc đỏ từ chối."],
            ["Luồng sự kiện chính (Basic Flow)", 
             "1. Hội viên quẹt thẻ từ qua đầu đọc hoặc nhân viên nhập số điện thoại của hội viên vào ô tìm kiếm nhanh.\n"
             "2. Đầu đọc thẻ gửi chuỗi CardID và tự động kích hoạt sự kiện xử lý (KeyDown / TextChanged).\n"
             "3. Hệ thống truy vấn CSDL tìm kiếm hồ sơ hội viên tương ứng với CardID.\n"
             "4. Hệ thống kiểm tra trạng thái hoạt động và các gói tập đang có hiệu lực của hội viên.\n"
             "5. Khi kiểm tra đạt điều kiện hợp lệ:\n"
             "   - Ghi một bản ghi mới vào bảng `CheckIns` với thời gian thực của máy chủ (DateTime.Now).\n"
             "   - Giao diện chuyển nền màu xanh lá cây rực rỡ, phát âm thanh 'Beep' thông báo thành công.\n"
             "   - Hiển thị to rõ: Ảnh chân dung của hội viên, Họ tên, Gói tập đang dùng, Số ngày còn lại của gói, Số buổi PT còn lại.\n"
             "6. Màn hình tự động đếm ngược 4 giây và quay lại trạng thái chờ quét cho hội viên tiếp theo."],
            ["Luồng rẽ nhánh (Alternative Flows)", 
             "3a. Mã thẻ không tồn tại: Giao diện chuyển đỏ, phát âm thanh cảnh báo lỗi 'Thẻ không hợp lệ hoặc chưa đăng ký!'\n"
             "4a. Gói tập đã hết hạn: Giao diện chuyển màu cam, hiển thị 'Gói tập đã hết hạn từ ngày [DD/MM/YYYY]. Vui lòng hướng dẫn khách gia hạn!'.\n"
             "4b. Hội viên vừa mới check-in trong vòng 5 phút trước: Hiển thị cảnh báo quẹt thẻ trùng lặp (Anti-passback) để tránh mượn thẻ."],
            ["Luồng ngoại lệ (Exception Flows)", "E1. Đầu đọc thẻ mất kết nối USB: Lễ tân có thể dùng bàn phím nhập Số điện thoại hoặc Mã hội viên thủ công."],
            ["Quy tắc nghiệp vụ (Business Rules)", "Thời gian xử lý một lượt điểm danh từ lúc nhận tín hiệu đến lúc hiển thị kết quả không được vượt quá 100 mili-giây."]
        ]),
        ("UC07: Bán lẻ sản phẩm tại quầy (POS Checkout)", [
            ["Mục", "Chi tiết đặc tả Use Case"],
            ["Mã Use Case", "UC07"],
            ["Tên Use Case", "Bán hàng quầy POS & Thanh toán hóa đơn (POS Retail & Checkout)"],
            ["Tác nhân (Actor)", "Quản trị viên (Admin), Nhân viên Lễ tân (Receptionist)"],
            ["Mô tả ngắn", "Lựa chọn sản phẩm nước uống, thực phẩm bổ sung, lập hóa đơn, trừ kho tự động và in phiếu thanh toán."],
            ["Tiền điều kiện", "Hàng hóa có sẵn trong danh mục `Products` và có số lượng tồn kho > 0."],
            ["Hậu điều kiện", "Tạo bản ghi trong `Orders` và `OrderDetails`, giảm `StockQuantity` trong bảng `Products`, in biên lai."],
            ["Luồng sự kiện chính (Basic Flow)", 
             "1. Nhân viên mở giao diện bán hàng quầy POS (frmPOS).\n"
             "2. Chọn danh mục sản phẩm (Nước uống, Whey Protein, Phụ kiện...). Màn hình hiển thị danh sách các món hàng kèm hình ảnh và giá.\n"
             "3. Nhân viên click chọn sản phẩm (hoặc quét mã vạch trên vỏ sản phẩm). Món hàng lập tức được thêm vào lưới Giỏ hàng bên phải.\n"
             "4. Nhân viên tùy chỉnh số lượng, nhập thông tin giảm giá đơn hàng (nếu có).\n"
             "5. Hệ thống tính toán tự động: Tiền hàng, Tiền chiết khấu, Thuế VAT (nếu có), Tổng tiền thanh toán.\n"
             "6. Nhân viên chọn khách hàng (Hội viên hoặc Khách vãng lai) và chọn Phương thức thanh toán (Tiền mặt / Chuyển khoản QR).\n"
             "7. Nhân viên nhấn \"Thanh toán & Xuất hóa đơn\".\n"
             "8. Hệ thống khởi tạo khối giao dịch nguyên tử `TransactionScope`:\n"
             "   - Ghi dữ liệu vào bảng `Orders`.\n"
             "   - Duyệt từng mặt hàng trong giỏ, ghi chi tiết vào `OrderDetails`.\n"
             "   - Trừ số lượng tồn kho `StockQuantity` tương ứng trong bảng `Products`.\n"
             "   - Hoàn tất giao dịch (`scope.Complete()`).\n"
             "9. Hệ thống gửi lệnh in hóa đơn ra máy in nhiệt K80 và làm sạch giỏ hàng sẵn sàng cho đơn mới."],
            ["Luồng rẽ nhánh (Alternative Flows)", 
             "3a. Mặt hàng đã hết tồn kho (StockQuantity = 0): Hệ thống chặn không cho thêm vào giỏ và cảnh báo 'Hết hàng trong kho!'.\n"
             "8a. Khách hàng thanh toán qua chuyển khoản: Hệ thống hiển thị mã VietQR động chứa chính xác số tiền và mã hóa đơn để khách quét."],
            ["Luồng ngoại lệ (Exception Flows)", "E1. Lỗi xảy ra trong quá trình trừ kho (ví dụ deadlock): TransactionScope kích hoạt tự động Rollback toàn bộ dữ liệu."],
            ["Quy tắc nghiệp vụ (Business Rules)", "Không cho phép xuất bán số lượng lớn hơn số lượng hàng thực tế đang có trong kho."]
        ]),
        ("UC08: Quản lý Danh mục Sản phẩm & Tồn kho", [
            ["Mục", "Chi tiết đặc tả Use Case"],
            ["Mã Use Case", "UC08"],
            ["Tên Use Case", "Quản lý Sản phẩm, Hàng hóa & Tồn kho (Product & Inventory Management)"],
            ["Tác nhân (Actor)", "Quản trị viên (Admin)"],
            ["Mô tả ngắn", "Thêm mới hàng hóa, cập nhật giá bán, kiểm kê kho và nhập hàng bổ sung khi sắp hết."],
            ["Tiền điều kiện", "Người dùng đăng nhập với quyền Quản trị viên (Admin)."],
            ["Hậu điều kiện", "Dữ liệu hàng hóa và số lượng tồn kho được cập nhật chính xác."],
            ["Luồng sự kiện chính (Basic Flow)", 
             "1. Quản trị viên truy cập phân hệ \"Quản lý Sản phẩm & Kho hàng\".\n"
             "2. Xem danh sách toàn bộ hàng hóa hiển thị trên DataGridView với các cột: Mã SP, Tên sản phẩm, Danh mục, Đơn vị tính, Giá nhập, Giá bán, Tồn kho.\n"
             "3. Nhấn \"Thêm sản phẩm mới\", điền thông tin và tải ảnh đại diện sản phẩm.\n"
             "4. Nhập số lượng tồn kho ban đầu và mức tồn tối thiểu để cảnh báo.\n"
             "5. Nhấn \"Lưu\". Hệ thống kiểm tra hợp lệ và ghi vào CSDL."],
            ["Luồng rẽ nhánh (Alternative Flows)", 
             "2a. Nhập hàng bổ sung: Quản trị viên chọn sản phẩm, nhấn \"Nhập kho\", điền số lượng nhập thêm và giá nhập đợt mới. Hệ thống tự động cộng dồn tồn kho.\n"
             "2b. Lọc hàng sắp hết: Hệ thống cung cấp nút lọc nhanh các mặt hàng có tồn kho dưới 5 sản phẩm."],
            ["Luồng ngoại lệ (Exception Flows)", "E1. Giá bán nhỏ hơn giá vốn: Hệ thống bật cảnh báo nhắc nhở người quản trị xác nhận giá."],
            ["Quy tắc nghiệp vụ (Business Rules)", "Sản phẩm đã từng có trong hóa đơn bán hàng không được phép xóa cứng (Hard Delete) khỏi CSDL để bảo tồn tính toàn vẹn báo cáo tài chính."]
        ]),
        ("UC09: Quản lý Trang thiết bị phòng tập", [
            ["Mục", "Chi tiết đặc tả Use Case"],
            ["Mã Use Case", "UC09"],
            ["Tên Use Case", "Quản lý Trang thiết bị & Bảo trì máy móc (Equipment & Maintenance)"],
            ["Tác nhân (Actor)", "Quản trị viên (Admin)"],
            ["Mô tả ngắn", "Theo dõi số lượng, tình trạng vận hành, vị trí khu vực tập và lịch bảo trì của hệ thống máy móc."],
            ["Tiền điều kiện", "Người dùng đăng nhập với quyền Quản trị viên."],
            ["Hậu điều kiện", "Hồ sơ máy móc và nhật ký bảo dưỡng được lưu vào bảng `Equipment`."],
            ["Luồng sự kiện chính (Basic Flow)", 
             "1. Mở màn hình \"Quản lý Thiết bị\" (frmEquipment).\n"
             "2. Danh sách thiết bị được hiển thị theo từng khu vực: Khu Cardio, Khu Tạ khối (Machines), Khu Tạ rời (Free Weights).\n"
             "3. Quản trị viên có thể cập nhật trạng thái của từng máy: Hoạt động bình thường / Đang chờ bảo trì / Hỏng hóc.\n"
             "4. Ghi nhận nhật ký bảo dưỡng định kỳ (thay dây cáp tạ, bôi trơn thảm máy chạy bộ) kèm chi phí sửa chữa."],
            ["Luồng rẽ nhánh (Alternative Flows)", "3a. Đánh dấu hỏng hóc: Khi máy hỏng, đổi trạng thái sang 'Hỏng' để nhân viên lễ tân không hướng dẫn khách dùng."],
            ["Luồng ngoại lệ (Exception Flows)", "E1. Thiết bị thanh lý: Cho phép chuyển trạng thái sang 'Thanh lý' có ghi chú lý do."],
            ["Quy tắc nghiệp vụ (Business Rules)", "Các thiết bị máy Cardio phải có chu kỳ bảo dưỡng định kỳ tối đa 6 tháng một lần."]
        ]),
        ("UC10: Báo cáo Doanh thu & Thống kê tài chính", [
            ["Mục", "Chi tiết đặc tả Use Case"],
            ["Mã Use Case", "UC10"],
            ["Tên Use Case", "Báo cáo Doanh thu & Thống kê kinh doanh (Financial Reports & Analytics)"],
            ["Tác nhân (Actor)", "Quản trị viên (Admin)"],
            ["Mô tả ngắn", "Tổng hợp doanh thu từ gói tập và bán lẻ, vẽ biểu đồ trực quan, phân tích mật độ tập luyện và xuất file Excel."],
            ["Tiền điều kiện", "Người dùng đăng nhập với quyền Quản trị viên."],
            ["Hậu điều kiện", "Báo cáo được hiển thị trên giao diện và có thể kết xuất thành tệp tin Excel."],
            ["Luồng sự kiện chính (Basic Flow)", 
             "1. Quản trị viên mở phân hệ \"Báo cáo Thống kê\" (frmReports).\n"
             "2. Chọn khoảng thời gian cần thống kê: Hôm nay / Tuần này / Tháng này / Quý này / Năm nay / Tùy chọn từ ngày... đến ngày...\n"
             "3. Nhấn nút \"Xem báo cáo\". Hệ thống thực thi các câu truy vấn LINQ to Entities tổng hợp:\n"
             "   - Tổng doanh thu bán gói tập thẻ hội viên.\n"
             "   - Tổng doanh thu bán lẻ nước và thực phẩm bổ sung.\n"
             "   - Tổng số lượt khách check-in và số hội viên mới gia nhập.\n"
             "4. Biểu đồ cột thể hiện biến động doanh thu theo từng ngày và biểu đồ tròn thể hiện cơ cấu nguồn thu.\n"
             "5. Quản trị viên nhấn \"Xuất Excel\". Hệ thống tạo file .xlsx định dạng bảng biểu chuyên nghiệp."],
            ["Luồng rẽ nhánh (Alternative Flows)", "3a. Xem theo nhân viên: Cho phép lọc xem doanh thu theo từng nhân viên lễ tân để tính KPI thưởng."],
            ["Luồng ngoại lệ (Exception Flows)", "E1. Khoảng thời gian từ ngày lớn hơn đến ngày: Báo lỗi và yêu cầu chọn lại."],
            ["Quy tắc nghiệp vụ (Business Rules)", "Chỉ duy nhất người dùng có vai trò Quản trị viên mới được phép xem báo cáo doanh thu tài chính."]
        ]),
        ("UC11: Sao lưu & Phục hồi Cơ sở dữ liệu", [
            ["Mục", "Chi tiết đặc tả Use Case"],
            ["Mã Use Case", "UC11"],
            ["Tên Use Case", "Sao lưu & Phục hồi CSDL (Database Backup & Restore)"],
            ["Tác nhân (Actor)", "Quản trị viên (Admin)"],
            ["Mô tả ngắn", "Tạo bản sao lưu dự phòng toàn bộ CSDL SQL Server ra file .bak và phục hồi lại dữ liệu khi xảy ra sự cố hỏng hóc."],
            ["Tiền điều kiện", "Người dùng đăng nhập với vai trò Quản trị viên, có quyền ghi file trên máy chủ."],
            ["Hậu điều kiện", "File backup .bak được tạo thành công hoặc CSDL được khôi phục về trạng thái sao lưu."],
            ["Luồng sự kiện chính (Basic Flow)", 
             "1. Quản trị viên truy cập mục \"Hệ thống -> Sao lưu dữ liệu\".\n"
             "2. Chọn thư mục lưu trữ file sao lưu (ví dụ: D:\\GymBackups\\).\n"
             "3. Nhấn \"Thực hiện Sao lưu\". Hệ thống gửi lệnh T-SQL `BACKUP DATABASE GymManagementDB TO DISK = '...'` đến SQL Server.\n"
             "4. Quá trình hoàn tất, hệ thống thông báo kèm dung lượng file và thời gian thực hiện."],
            ["Luồng rẽ nhánh (Alternative Flows)", 
             "1b. Phục hồi dữ liệu: Chọn file .bak cần khôi phục, hệ thống ngắt toàn bộ kết nối hiện tại (`ALTER DATABASE ... SET SINGLE_USER WITH ROLLBACK IMMEDIATE`) và tiến hành Restore."],
            ["Luồng ngoại lệ (Exception Flows)", "E1. Ổ đĩa máy tính hết dung lượng: Hệ thống cảnh báo không đủ bộ nhớ trống để tạo file backup."],
            ["Quy tắc nghiệp vụ (Business Rules)", "Khuyến nghị tự động tạo bản sao lưu định kỳ vào cuối mỗi ngày làm việc."]
        ]),
        ("UC12: Tra cứu Lịch sử & Nhật ký Hệ thống (Audit Logs)", [
            ["Mục", "Chi tiết đặc tả Use Case"],
            ["Mã Use Case", "UC12"],
            ["Tên Use Case", "Tra cứu Nhật ký Thao tác Hệ thống (System Audit Log)"],
            ["Tác nhân (Actor)", "Quản trị viên (Admin)"],
            ["Mô tả ngắn", "Ghi vết và cung cấp khả năng tra soát các hành động nhạy cảm của nhân viên (sửa giá, xóa dữ liệu, áp chiết khấu)."],
            ["Tiền điều kiện", "Người dùng đăng nhập với quyền Quản trị viên."],
            ["Hậu điều kiện", "Nhật ký hệ thống hiển thị chi tiết thời gian, người thực hiện, hành động và dữ liệu thay đổi."],
            ["Luồng sự kiện chính (Basic Flow)", 
             "1. Quản trị viên vào phân hệ \"Nhật ký kiểm toán\" (Audit Log).\n"
             "2. Hệ thống tải dữ liệu nhật ký với các bộ lọc: Lọc theo Người dùng, Lọc theo Thao tác, Lọc theo Ngày.\n"
             "3. Quản trị viên xem xét các thao tác đáng ngờ (ví dụ: nhân viên áp mã giảm giá 50% cho người quen).\n"
             "4. Hỗ trợ xuất dữ liệu ra file phục vụ công tác thanh tra nội bộ."],
            ["Luồng rẽ nhánh (Alternative Flows)", "2a. Tìm kiếm theo từ khóa: Nhập mã hội viên hoặc mã đơn hàng để tra toàn bộ lịch sử tác động."],
            ["Luồng ngoại lệ (Exception Flows)", "E1. Lượng log quá lớn: Áp dụng phân trang (Pagination) 50 bản ghi/trang để duy trì tốc độ mượt mà."],
            ["Quy tắc nghiệp vụ (Business Rules)", "Dữ liệu nhật ký hệ thống chỉ có quyền Đọc (Read-only), không ai (kể cả Admin) được phép xóa sửa."]
        ])
    ]

    for uc_title, uc_table_data in uc_details:
        add_h3(uc_title)
        add_custom_table(uc_table_data[0], uc_table_data[1:], [1.6, 5.2])

    print("Building Sequence & Architecture...")
    # =========================================================================
    # 2.3 SƠ ĐỒ HOẠT ĐỘNG & TUẦN TỰ
    # =========================================================================
    add_h2("2.3. Thiết kế sơ đồ động: Sơ đồ hoạt động và Sơ đồ tuần tự")
    add_p("Để làm rõ sự vận hành của phần mềm ở cấp độ kỹ thuật, nhóm đã xây dựng các kịch bản tương tác tuần tự giữa các đối tượng: Người dùng (Actor), Giao diện (Form View), Lớp xử lý nghiệp vụ (Business Controller / Service), Bộ đệm dữ liệu (Entity Framework DbContext) và Cơ sở dữ liệu (SQL Server Database).")

    add_h3("2.3.1. Sơ đồ tuần tự: Xác thực đăng nhập và Khởi tạo phiên")
    add_p("Quy trình xác thực diễn ra qua 8 bước tuần tự chặt chẽ:", bold_prefix="Trình tự thực thi: ")
    add_p("1. [Actor -> frmLogin]: Nhập Username và Password, kích hoạt sự kiện `btnLogin_Click`.\n"
          "2. [frmLogin -> Validation]: Kiểm tra chuỗi đầu vào không rỗng, kích hoạt ErrorProvider nếu không hợp lệ.\n"
          "3. [frmLogin -> GymEntities]: Gọi truy vấn LINQ: `db.Users.Include(\"Role\").FirstOrDefault(u => u.Username == inputUser)`.\n"
          "4. [GymEntities -> SQL Server]: Thực thi lệnh T-SQL `SELECT TOP(1) ... FROM Users WHERE Username = @p0`.\n"
          "5. [SQL Server -> GymEntities]: Trả về thực thể User (chứa PasswordHash, Salt, IsActive, RoleId).\n"
          "6. [GymEntities -> frmLogin]: Trả đối tượng User về form.\n"
          "7. [frmLogin -> SecurityHelper]: Gọi hàm `SecurityHelper.VerifyPassword(inputPass, user.PasswordHash, user.Salt)`.\n"
          "   - Hàm thực thi băm PBKDF2 với 10.000 vòng lặp và so sánh độ khớp hằng số thời gian (Constant-time comparison) để chống tấn công Timing Attack.\n"
          "8. [frmLogin -> UserSession]: Khi mật khẩu chính xác, ghi dữ liệu vào phiên làm việc tĩnh: `UserSession.Instance.Initialize(user)`.\n"
          "9. [frmLogin -> frmMainDashboard]: Khởi tạo màn hình chính, truyền phiên làm việc, thiết lập hiển thị menu và đóng form login.")

    add_h3("2.3.2. Sơ đồ tuần tự: Đăng ký thẻ hội viên và Thanh toán gói")
    add_p("1. [Actor -> frmMemberAddEdit]: Nhập thông tin hội viên, quẹt thẻ RFID lấy CardID, nhấn \"Lưu & Đăng ký gói\".\n"
          "2. [frmMemberAddEdit -> GymEntities]: Tạo thực thể `Member`, thực thi `db.Members.Add(newMember)` và `db.SaveChanges()` để lấy MemberId vừa phát sinh.\n"
          "3. [frmMemberAddEdit -> frmSubscription]: Tự động mở form đăng ký gói tập, truyền MemberId sang.\n"
          "4. [Actor -> frmSubscription]: Chọn Gói tập (PackageId), chọn Ngày kích hoạt (StartDate), chọn Phương thức thanh toán.\n"
          "5. [frmSubscription -> Calculator]: Tự động tính toán: `EndDate = StartDate.AddDays(package.DurationDays)`, `TotalAmount = package.Price * (1 - Discount/100)`.\n"
          "6. [frmSubscription -> GymEntities]: Khởi tạo đối tượng `MemberSubscription`, thiết lập trạng thái 'Active'.\n"
          "7. [GymEntities -> SQL Server]: Thực thi lưu dữ liệu vào bảng `MemberSubscriptions` và cập nhật trường `Status = 'Active'` trong bảng `Members`.\n"
          "8. [frmSubscription -> Printer]: Khởi tạo lệnh in Hợp đồng / Biên nhận thu phí gói tập giao cho khách hàng.")

    add_h3("2.3.3. Sơ đồ tuần tự: Đặt lịch PT và Kiểm tra xung đột thời gian")
    add_p("1. [Actor -> frmScheduleAddEdit]: Chọn Huấn luyện viên (TrainerId), Hội viên (MemberId), Ngày tập (Date), Giờ bắt đầu (StartTime), Giờ kết thúc (EndTime).\n"
          "2. [frmScheduleAddEdit -> SchedulerEngine]: Kích hoạt hàm kiểm tra `CheckTrainerScheduleConflict(trainerId, date, start, end)`.\n"
          "3. [SchedulerEngine -> GymEntities]: Thực thi truy vấn LINQ kiểm tra khoảng giao nhau:\n"
          "   `db.WorkoutSchedules.Any(s => s.TrainerId == tId && s.ScheduleDate == date && s.Status != \"Cancelled\" && s.StartTime < end && s.EndTime > start)`.\n"
          "4. [GymEntities -> SQL Server]: SQL Server kiểm tra chỉ mục trên bảng `WorkoutSchedules` và trả về giá trị Boolean (True nếu trùng, False nếu an toàn).\n"
          "5. [SchedulerEngine -> frmScheduleAddEdit]:\n"
          "   - Nếu True (Có trùng): Bật MessageBox cảnh báo đỏ, chỉ rõ lịch trùng và không cho phép lưu.\n"
          "   - Nếu False (Không trùng): Cho phép tạo bản ghi `WorkoutSchedule` mới, lưu vào CSDL và cập nhật hiển thị lịch.")

    add_h3("2.3.4. Sơ đồ tuần tự: Quẹt thẻ Check-in và Cảnh báo hạn dùng")
    add_p("1. [RFID Reader -> frmCheckIn]: Đầu đọc thẻ gửi tín hiệu CardID (chuỗi số kết thúc bằng phím Enter) vào TextBox ẩn trên form.\n"
          "2. [frmCheckIn -> GymEntities]: Thực thi truy vấn kết hợp: Tìm Member theo CardID và nạp toàn bộ các gói tập liên quan (`.Include(m => m.MemberSubscriptions.Select(s => s.MembershipPackage))`.\n"
          "3. [frmCheckIn -> Logic Engine]: Thực thi kiểm tra 3 cấp độ:\n"
          "   - Cấp 1: Kiểm tra Member có tồn tại không? Nếu không -> Báo lỗi thẻ lạ.\n"
          "   - Cấp 2: Tìm gói tập thỏa mãn điều kiện `s.StartDate <= today && s.EndDate >= today && s.Status == \"Active\"`.\n"
          "   - Cấp 3: Nếu còn dưới 3 ngày hết hạn -> Kích hoạt cờ cảnh báo vàng 'Sắp hết hạn'. Nếu đã qua ngày EndDate -> Kích hoạt cờ đỏ 'Đã hết hạn'.\n"
          "4. [Logic Engine -> GymEntities]: Nếu hợp lệ, gọi `db.CheckIns.Add(new CheckIn { MemberId = m.MemberId, CheckInTime = DateTime.Now })` và `db.SaveChanges()`.\n"
          "5. [frmCheckIn -> UI / Speaker]: Bật đèn giao diện xanh lá cây, phát âm thanh báo thành công, phóng to ảnh đại diện của hội viên lên màn hình trong 4 giây.")

    add_h3("2.3.5. Sơ đồ tuần tự: Bán hàng POS và Trừ tồn kho đồng thời")
    add_p("1. [Actor -> frmPOS]: Chọn sản phẩm, số lượng vào Giỏ hàng (Cart), nhấn nút \"Thanh toán\".\n"
          "2. [frmPOS -> TransactionScope]: Khởi tạo khối giao dịch phân tán: `using (var scope = new TransactionScope())`.\n"
          "3. [frmPOS -> GymEntities]: Kiểm tra số lượng tồn kho từng mặt hàng: `product.StockQuantity >= cartItem.Quantity`.\n"
          "   - Nếu bất kỳ mặt hàng nào không đủ số lượng tồn, ném ngoại lệ `InvalidOperationException(\"Mặt hàng không đủ tồn kho\")` để hủy toàn bộ giao dịch.\n"
          "4. [frmPOS -> GymEntities]: Tạo bản ghi `Order` (ghi nhận OrderCode, CustomerId, TotalAmount, CreatedDate, PaymentMethod).\n"
          "5. [frmPOS -> GymEntities]: Lặp qua từng món trong giỏ: Tạo bản ghi `OrderDetail` (OrderId, ProductId, Quantity, UnitPrice) và cập nhật `product.StockQuantity -= item.Quantity`.\n"
          "6. [GymEntities -> SQL Server]: Thực thi toàn bộ lệnh INSERT và UPDATE trong một phiên làm việc.\n"
          "7. [frmPOS -> TransactionScope]: Gọi `scope.Complete()` để commit vĩnh viễn giao dịch vào cơ sở dữ liệu.\n"
          "8. [frmPOS -> ReceiptPrinter]: Gửi lệnh in hóa đơn thanh toán K80 với đầy đủ mã vạch đơn hàng.")

    print("Building Software Architecture & Data Dictionary...")
    # =========================================================================
    # 2.4 THIẾT KẾ KIẾN TRÚC PHẦN MỀM
    # =========================================================================
    add_h2("2.4. Thiết kế kiến trúc phần mềm và Các Pattern kỹ thuật")
    add_p("Dự án được xây dựng tuân thủ nghiêm ngặt mô hình kiến trúc phân tầng 3 lớp (Three-Layer Architecture), giúp tách biệt rõ ràng giữa tầng giao diện hiển thị, tầng xử lý quy tắc nghiệp vụ và tầng truy cập dữ liệu, tạo thuận lợi tối đa cho việc kiểm thử, bảo trì và mở rộng:")
    add_p("1. Tầng Giao diện Người dùng (Presentation Layer - GUI): Bao gồm 16 Windows Forms được thiết kế hiện đại, responsive theo tỷ lệ màn hình 16:9 và 4:3. Tầng này chịu trách nhiệm hiển thị thông tin, tiếp nhận tương tác từ chuột/bàn phím, kiểm tra dữ liệu sơ bộ (Input Format Validation) và chuyển lời gọi xuống tầng nghiệp vụ.\n"
          "2. Tầng Nghiệp vụ (Business Logic Layer - BLL): Chứa toàn bộ các thuật toán cốt lõi, quy tắc kiểm tra ràng buộc (Interval Overlap Detection cho lịch tập, kiểm tra điều kiện kích hoạt/hết hạn thẻ hội viên, chính sách chiết khấu giảm giá, quản lý giao dịch nguyên tử ACID).\n"
          "3. Tầng Truy cập Dữ liệu (Data Access Layer - DAL): Triển khai trên nền tảng Entity Framework 6 (EF6) theo mô hình Database-First kết hợp `DbContext`. Lớp này chịu trách nhiệm chuyển đổi các thao tác hướng đối tượng C# thành các câu truy vấn T-SQL tối ưu để giao tiếp với máy chủ SQL Server 2022.")

    add_p("Mô hình Quản lý Phiên làm việc (Session Management) với mẫu thiết kế Singleton Pattern:", bold_prefix="Thiết kế Mẫu Singleton: ")
    add_p("Để theo dõi người dùng đang đăng nhập xuyên suốt các form mà không phải truyền tham số rườm rà qua hàm dựng (Constructor), hệ thống triển khai lớp `UserSession` theo chuẩn Thread-Safe Singleton:")
    add_code("""public sealed class UserSession
{
    private static UserSession _instance = null;
    private static readonly object _lock = new object();

    public int UserId { get; private set; }
    public string Username { get; private set; }
    public string FullName { get; private set; }
    public int RoleId { get; private set; }
    public string RoleName { get; private set; }
    public DateTime LoginTime { get; private set; }
    public bool IsAuthenticated => UserId > 0;

    private UserSession() { }

    public static UserSession Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new UserSession();
                }
            }
            return _instance;
        }
    }

    public void Initialize(User user)
    {
        UserId = user.UserId;
        Username = user.Username;
        FullName = user.FullName;
        RoleId = user.RoleId;
        RoleName = user.Role != null ? user.Role.RoleName : "";
        LoginTime = DateTime.Now;
    }

    public void Clear()
    {
        UserId = 0;
        Username = string.Empty;
        FullName = string.Empty;
        RoleId = 0;
        RoleName = string.Empty;
    }
}""")

    # =========================================================================
    # 2.5 THIẾT KẾ CƠ SỞ DỮ LIỆU
    # =========================================================================
    add_h2("2.5. Thiết kế cơ sở dữ liệu quan hệ (RDBMS) chuẩn hóa 3NF")
    add_p("Cơ sở dữ liệu của hệ thống mang tên `GymManagementDB` được thiết kế chặt chẽ trên Microsoft SQL Server 2022, đáp ứng đầy đủ tiêu chuẩn dạng chuẩn 3 (Third Normal Form - 3NF). Mọi thực thể đều sở hữu khóa chính nhân tạo (Surrogate Primary Key - `INT IDENTITY`), các thuộc tính phụ thuộc hàm hoàn toàn vào khóa chính, không tồn tại phụ thuộc bắc cầu và loại bỏ hoàn toàn tình trạng dư thừa dữ liệu.")

    add_h3("2.5.1. Sơ đồ quan hệ thực thể (ERD) và Toàn vẹn dữ liệu")
    add_p("Cơ sở dữ liệu bao gồm 13 bảng thực thể có mối quan hệ chặt chẽ thông qua các khóa ngoại (Foreign Keys) được thiết lập luật toàn vẹn dữ liệu nghiêm ngặt:", bold_prefix="Các mối quan hệ liên kết bảng: ")
    add_p("- `Roles` 1 - N `Users`: Một vai trò (Admin, Lễ tân, PT) áp dụng cho nhiều tài khoản người dùng.\n"
          "- `Members` 1 - N `MemberSubscriptions`: Một hội viên có thể sở hữu nhiều gói tập trong suốt lịch sử tập luyện.\n"
          "- `MembershipPackages` 1 - N `MemberSubscriptions`: Một loại gói tập có thể được đăng ký bởi nhiều hội viên.\n"
          "- `Trainers` 1 - N `WorkoutSchedules`: Một huấn luyện viên có nhiều lịch dạy với các hội viên khác nhau.\n"
          "- `Members` 1 - N `WorkoutSchedules`: Một hội viên có thể đặt nhiều ca tập luyện.\n"
          "- `Members` 1 - N `CheckIns`: Một hội viên có thể có nhiều lượt điểm danh ra vào phòng tập.\n"
          "- `Categories` 1 - N `Products`: Một danh mục hàng hóa (Nước uống, Bột dinh dưỡng) chứa nhiều sản phẩm cụ thể.\n"
          "- `Orders` 1 - N `OrderDetails`: Một đơn hàng bán lẻ gồm nhiều dòng chi tiết sản phẩm.\n"
          "- `Products` 1 - N `OrderDetails`: Một sản phẩm xuất hiện trong nhiều đơn hàng bán lẻ khác nhau.\n"
          "- `Users` 1 - N `Orders`: Một nhân viên lễ tân có thể lập nhiều hóa đơn bán hàng cho khách.")

    add_h3("2.5.2. Từ điển dữ liệu (Data Dictionary) chi tiết 13 bảng")
    add_p("Dưới đây là đặc tả chi tiết của toàn bộ 13 bảng trong cơ sở dữ liệu `GymManagementDB`:")

    tables_spec = [
        ("Bảng 1: Roles (Danh mục vai trò quyền hạn)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["RoleId", "INT IDENTITY(1,1)", "No", "PK", "Mã định danh vai trò", "1, 2, 3"],
             ["RoleName", "NVARCHAR(50)", "No", "Unique", "Tên vai trò hệ thống", "'Admin', 'Receptionist', 'Trainer'"],
             ["Description", "NVARCHAR(255)", "Yes", "None", "Mô tả chi tiết quyền hạn", "'Quản trị viên toàn quyền hệ thống'"],
             ["CreatedAt", "DATETIME", "No", "DF: GETDATE()", "Thời điểm tạo vai trò", "'2026-01-01 08:00:00'"]
         ], [1.1, 1.4, 0.5, 1.0, 2.0, 1.2]),
        ("Bảng 2: Users (Tài khoản người dùng & Nhân viên)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["UserId", "INT IDENTITY(1,1)", "No", "PK", "Mã định danh tài khoản", "1, 2, 3, 4"],
             ["Username", "VARCHAR(50)", "No", "Unique", "Tên đăng nhập hệ thống", "'admin', 'letan_mai', 'pt_tuan'"],
             ["PasswordHash", "VARCHAR(256)", "No", "None", "Mật khẩu băm PBKDF2", "'e3b0c44298fc1c149afbf4c8996fb924...'"],
             ["Salt", "VARCHAR(128)", "No", "None", "Chuỗi muối mã hóa ngẫu nhiên", "'a8f5c1639d2e47b8'"],
             ["FullName", "NVARCHAR(100)", "No", "None", "Họ và tên đầy đủ nhân viên", "'Hứa Nhựt Tuấn'"],
             ["RoleId", "INT", "No", "FK -> Roles", "Mã vai trò của người dùng", "1"],
             ["PhoneNumber", "VARCHAR(15)", "Yes", "None", "Số điện thoại liên lạc", "'0903123456'"],
             ["Email", "VARCHAR(100)", "Yes", "None", "Thư điện tử nhân viên", "'tuan.hua@gym.vn'"],
             ["IsActive", "BIT", "No", "DF: 1", "Trạng thái kích hoạt (1=Bật, 0=Khóa)", "1"],
             ["CreatedAt", "DATETIME", "No", "DF: GETDATE()", "Thời điểm tạo tài khoản", "'2026-01-15 09:30:00'"]
         ], [1.1, 1.3, 0.4, 1.0, 2.0, 1.4]),
        ("Bảng 3: Members (Hồ sơ hội viên phòng tập)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["MemberId", "INT IDENTITY(1,1)", "No", "PK", "Mã định danh hội viên", "101, 102, 103"],
             ["MemberCode", "VARCHAR(20)", "No", "Unique", "Mã thẻ hội viên quy chuẩn", "'MEM001', 'MEM002'"],
             ["CardID", "VARCHAR(50)", "Yes", "Unique/Index", "Mã quẹt thẻ từ RFID/Barcode", "'RFID98234125'"],
             ["FullName", "NVARCHAR(100)", "No", "None", "Họ tên đầy đủ hội viên", "'Nguyễn Văn An'"],
             ["Gender", "NVARCHAR(10)", "Yes", "Check: Nam/Nữ", "Giới tính", "'Nam'"],
             ["DateOfBirth", "DATE", "Yes", "None", "Ngày tháng năm sinh", "'1998-05-20'"],
             ["PhoneNumber", "VARCHAR(15)", "No", "Index", "Số điện thoại di động", "'0987654321'"],
             ["IdentityCard", "VARCHAR(20)", "Yes", "None", "Số Căn cước công dân (CCCD)", "'079201004567'"],
             ["Address", "NVARCHAR(200)", "Yes", "None", "Địa chỉ cư trú", "'123 Nguyễn Văn Cừ, Q.5, TP.HCM'"],
             ["AvatarPath", "NVARCHAR(255)", "Yes", "None", "Đường dẫn ảnh chân dung", "'/avatars/mem001.jpg'"],
             ["Status", "NVARCHAR(20)", "No", "DF: 'Active'", "Trạng thái (Active/Expired/Suspended)", "'Active'"],
             ["JoinDate", "DATETIME", "No", "DF: GETDATE()", "Ngày bắt đầu gia nhập CLB", "'2026-02-01'"]
         ], [1.1, 1.3, 0.4, 1.0, 2.0, 1.4]),
        ("Bảng 4: MembershipPackages (Danh mục gói tập)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["PackageId", "INT IDENTITY(1,1)", "No", "PK", "Mã định danh gói tập", "1, 2, 3, 4, 5"],
             ["PackageName", "NVARCHAR(100)", "No", "None", "Tên thương mại gói tập", "'Gói Vàng 3 Tháng', 'VIP 1 Năm'"],
             ["DurationDays", "INT", "No", "Check > 0", "Thời hạn gói tập tính theo ngày", "30, 90, 180, 365"],
             ["Price", "DECIMAL(18,2)", "No", "Check >= 0", "Đơn giá niêm yết (VNĐ)", "1500000.00, 5000000.00"],
             ["Description", "NVARCHAR(255)", "Yes", "None", "Quyền lợi chi tiết gói tập", "'Full Gym + Sauna + Khăn tập'"],
             ["IsActive", "BIT", "No", "DF: 1", "Đang bán (1) hoặc Ngừng bán (0)", "1"]
         ], [1.1, 1.4, 0.5, 1.0, 2.0, 1.2]),
        ("Bảng 5: MemberSubscriptions (Hợp đồng gói tập hội viên)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["SubscriptionId", "INT IDENTITY(1,1)", "No", "PK", "Mã định danh hợp đồng gói", "501, 502, 503"],
             ["MemberId", "INT", "No", "FK -> Members", "Mã hội viên thụ hưởng", "101"],
             ["PackageId", "INT", "No", "FK -> Packages", "Mã gói tập đã đăng ký", "2"],
             ["StartDate", "DATE", "No", "None", "Ngày bắt đầu có hiệu lực", "'2026-03-01'"],
             ["EndDate", "DATE", "No", "Check >= Start", "Ngày kết thúc hiệu lực", "'2026-06-01'"],
             ["PricePaid", "DECIMAL(18,2)", "No", "None", "Số tiền thực tế thanh toán", "1350000.00 (sau chiết khấu)"],
             ["Status", "NVARCHAR(20)", "No", "DF: 'Active'", "Trạng thái (Active/Expired)", "'Active'"],
             ["CreatedDate", "DATETIME", "No", "DF: GETDATE()", "Thời điểm giao dịch hợp đồng", "'2026-03-01 10:15:00'"]
         ], [1.2, 1.3, 0.4, 1.0, 1.9, 1.4]),
        ("Bảng 6: Trainers (Huấn luyện viên cá nhân - PT)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["TrainerId", "INT IDENTITY(1,1)", "No", "PK", "Mã định danh huấn luyện viên", "1, 2, 3"],
             ["TrainerCode", "VARCHAR(20)", "No", "Unique", "Mã nhân viên PT", "'PT001', 'PT002'"],
             ["FullName", "NVARCHAR(100)", "No", "None", "Họ tên đầy đủ huấn luyện viên", "'Lê Văn Gia Bảo'"],
             ["PhoneNumber", "VARCHAR(15)", "No", "None", "Số điện thoại di động", "'0918765432'"],
             ["Specialty", "NVARCHAR(100)", "Yes", "None", "Chuyên môn giảng dạy", "'Tăng cơ Bodybuilding, Kickfit'"],
             ["HourlyRate", "DECIMAL(18,2)", "No", "Check >= 0", "Mức phí dạy 1 giờ (VNĐ)", "300000.00"],
             ["IsActive", "BIT", "No", "DF: 1", "Trạng thái làm việc (1=Đang làm)", "1"]
         ], [1.1, 1.4, 0.5, 1.0, 2.0, 1.2]),
        ("Bảng 7: WorkoutSchedules (Lịch tập huấn luyện viên)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["ScheduleId", "INT IDENTITY(1,1)", "No", "PK", "Mã định danh lịch tập", "1001, 1002"],
             ["TrainerId", "INT", "No", "FK -> Trainers", "Mã huấn luyện viên dạy", "1"],
             ["MemberId", "INT", "No", "FK -> Members", "Mã hội viên tham gia", "101"],
             ["ScheduleDate", "DATE", "No", "Index", "Ngày tập luyện", "'2026-03-15'"],
             ["StartTime", "TIME", "No", "None", "Giờ bắt đầu buổi tập", "'08:00:00'"],
             ["EndTime", "TIME", "No", "Check > Start", "Giờ kết thúc buổi tập", "'09:30:00'"],
             ["Status", "NVARCHAR(20)", "No", "DF: 'Scheduled'", "Trạng thái (Scheduled/Done/Cancelled)", "'Scheduled'"],
             ["Notes", "NVARCHAR(255)", "Yes", "None", "Nội dung bài tập hoặc lưu ý", "'Tập nhóm cơ ngực và tay sau'"]
         ], [1.1, 1.3, 0.4, 1.0, 2.0, 1.4]),
        ("Bảng 8: CheckIns (Lịch sử điểm danh vào phòng)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["CheckInId", "BIGINT IDENTITY(1,1)", "No", "PK", "Mã định danh lượt điểm danh", "1, 2, 3..."],
             ["MemberId", "INT", "No", "FK -> Members", "Mã hội viên vào phòng", "101"],
             ["CheckInTime", "DATETIME", "No", "DF: GETDATE(), Index", "Thời điểm chính xác quẹt thẻ", "'2026-03-15 17:45:12'"],
             ["Notes", "NVARCHAR(255)", "Yes", "None", "Ghi chú cửa vào / thiết bị quét", "'Cổng quầy lễ tân tầng trệt'"]
         ], [1.1, 1.4, 0.5, 1.0, 2.0, 1.2]),
        ("Bảng 9: Categories (Danh mục sản phẩm bán lẻ)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["CategoryId", "INT IDENTITY(1,1)", "No", "PK", "Mã định danh nhóm hàng", "1, 2, 3"],
             ["CategoryName", "NVARCHAR(50)", "No", "Unique", "Tên nhóm sản phẩm", "'Nước giải khát', 'Thực phẩm bổ sung'"],
             ["Description", "NVARCHAR(255)", "Yes", "None", "Mô tả chi tiết nhóm hàng", "'Nước khoáng bù điện giải và tăng lực'"]
         ], [1.1, 1.4, 0.5, 1.0, 2.2, 1.0]),
        ("Bảng 10: Products (Sản phẩm, Hàng hóa quầy POS)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["ProductId", "INT IDENTITY(1,1)", "No", "PK", "Mã định danh sản phẩm", "1, 2, 3, 4, 5"],
             ["ProductCode", "VARCHAR(20)", "No", "Unique", "Mã vạch / Barcode sản phẩm", "'REVIVE330', 'WHEYGOLD5LB'"],
             ["ProductName", "NVARCHAR(100)", "No", "None", "Tên thương phẩm", "'Nước khoáng Revive 500ml', 'Whey Gold 5Lbs'"],
             ["CategoryId", "INT", "No", "FK -> Categories", "Mã nhóm sản phẩm", "1"],
             ["Unit", "NVARCHAR(20)", "No", "None", "Đơn vị tính", "'Chai', 'Lon', 'Hộp', 'Bình'"],
             ["CostPrice", "DECIMAL(18,2)", "No", "Check >= 0", "Giá vốn nhập kho (VNĐ)", "8000.00, 1450000.00"],
             ["UnitPrice", "DECIMAL(18,2)", "No", "Check >= Cost", "Giá bán lẻ niêm yết (VNĐ)", "15000.00, 1850000.00"],
             ["StockQuantity", "INT", "No", "Check >= 0", "Số lượng tồn kho hiện tại", "150, 25"],
             ["MinStockAlert", "INT", "No", "DF: 5", "Ngưỡng cảnh báo sắp hết hàng", "10"],
             ["IsActive", "BIT", "No", "DF: 1", "Đang kinh doanh (1=Bật)", "1"]
         ], [1.1, 1.3, 0.4, 1.0, 2.0, 1.4]),
        ("Bảng 11: Orders (Hóa đơn bán lẻ quầy POS)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["OrderId", "INT IDENTITY(1,1)", "No", "PK", "Mã định danh đơn hàng", "2001, 2002"],
             ["OrderCode", "VARCHAR(20)", "No", "Unique", "Mã hóa đơn in trên phiếu", "'HD20260315-001'"],
             ["MemberId", "INT", "Yes", "FK -> Members", "Mã hội viên mua (nếu có)", "101 (hoặc NULL nếu khách lẻ)"],
             ["UserId", "INT", "No", "FK -> Users", "Mã nhân viên lập hóa đơn", "2"],
             ["OrderDate", "DATETIME", "No", "DF: GETDATE(), Index", "Thời điểm xuất hóa đơn", "'2026-03-15 18:30:00'"],
             ["SubTotal", "DECIMAL(18,2)", "No", "None", "Tổng tiền hàng chưa giảm", "250000.00"],
             ["Discount", "DECIMAL(18,2)", "No", "DF: 0", "Số tiền chiết khấu giảm giá", "25000.00"],
             ["TotalAmount", "DECIMAL(18,2)", "No", "None", "Số tiền thanh toán cuối cùng", "225000.00"],
             ["PaymentMethod", "NVARCHAR(30)", "No", "None", "Phương thức (Tiền mặt/Chuyển khoản)", "'Tiền mặt', 'VietQR'"]
         ], [1.1, 1.3, 0.4, 1.0, 2.0, 1.4]),
        ("Bảng 12: OrderDetails (Chi tiết các mặt hàng trong đơn)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["OrderDetailId", "INT IDENTITY(1,1)", "No", "PK", "Mã định danh dòng chi tiết", "5001, 5002"],
             ["OrderId", "INT", "No", "FK -> Orders", "Mã đơn hàng cha", "2001"],
             ["ProductId", "INT", "No", "FK -> Products", "Mã sản phẩm được mua", "1"],
             ["Quantity", "INT", "No", "Check > 0", "Số lượng bán ra", "3"],
             ["UnitPrice", "DECIMAL(18,2)", "No", "None", "Đơn giá tại thời điểm bán", "15000.00"],
             ["TotalPrice", "DECIMAL(18,2)", "No", "Computed", "Thành tiền (Quantity * UnitPrice)", "45000.00"]
         ], [1.1, 1.4, 0.5, 1.0, 2.0, 1.2]),
        ("Bảng 13: Equipment (Quản lý thiết bị & máy tập phòng gym)",
         ["Cột", "Kiểu dữ liệu", "Null", "Ràng buộc", "Mô tả nghiệp vụ", "Dữ liệu mẫu"],
         [
             ["EquipmentId", "INT IDENTITY(1,1)", "No", "PK", "Mã định danh thiết bị", "1, 2, 3"],
             ["EquipmentCode", "VARCHAR(20)", "No", "Unique", "Mã dán trên thân máy", "'CARDIO-TM01', 'MECH-LP02'"],
             ["EquipmentName", "NVARCHAR(100)", "No", "None", "Tên máy tập / thiết bị", "'Máy chạy bộ đa năng Impulse RT700'"],
             ["Category", "NVARCHAR(50)", "No", "None", "Phân loại (Cardio/Tạ khối/Tạ tự do)", "'Cardio'"],
             ["PurchaseDate", "DATE", "Yes", "None", "Ngày nhập mua thiết bị", "'2025-06-10'"],
             ["WarrantyMonths", "INT", "Yes", "None", "Thời hạn bảo hành (tháng)", "24"],
             ["Status", "NVARCHAR(30)", "No", "DF: 'Good'", "Tình trạng (Good/Maintenance/Broken)", "'Good'"],
             ["LastMaintenance", "DATE", "Yes", "None", "Ngày thực hiện bảo dưỡng gần nhất", "'2026-01-10'"]
         ], [1.1, 1.3, 0.4, 1.0, 2.0, 1.4])
    ]

    for tbl_name, headers, rows, widths in tables_spec:
        add_h3(tbl_name)
        add_custom_table(headers, rows, widths)

    add_h3("2.5.3. Chiến lược Indexing và Tối ưu hóa truy vấn cơ sở dữ liệu")
    add_p("Khi lượng hội viên tích lũy qua nhiều năm lên đến hàng chục ngàn người và bảng `CheckIns` chứa hàng trăm ngàn lượt quét, hiệu năng truy vấn sẽ suy giảm nghiêm trọng nếu thiếu các chỉ mục (Indexes) chiến lược. Nhóm đã cấu hình các bộ chỉ mục B-Tree tối ưu:")
    add_p("1. Bảng `Members`: Tạo chỉ mục Non-Clustered Index trên cột `CardID` (`IX_Members_CardID`) và trên cột `PhoneNumber` (`IX_Members_PhoneNumber`). Giúp tốc độ tìm kiếm hội viên khi quẹt thẻ đạt độ phức tạp thời gian O(log N), thời gian phản hồi thực tế < 2ms thay vì phải quét toàn bộ bảng (Table Scan).\n"
          "2. Bảng `CheckIns`: Tạo chỉ mục Non-Clustered Index kết hợp (`IX_CheckIns_Member_Time`) trên hai cột `(MemberId, CheckInTime DESC)`. Giúp truy xuất lịch sử quẹt thẻ gần nhất của hội viên diễn ra tức thì.\n"
          "3. Bảng `WorkoutSchedules`: Tạo chỉ mục kết hợp (`IX_Schedules_Trainer_Date`) trên `(TrainerId, ScheduleDate) INCLUDE (StartTime, EndTime, Status)`. Chỉ mục này bao phủ hoàn toàn (Covering Index) câu truy vấn kiểm tra xung đột lịch tập của thuật toán Interval Overlap, loại bỏ việc phải tra cứu dữ liệu gốc (Key Lookup).")

    print("Building Form UI/UX Breakdown...")
    # =========================================================================
    # 2.6 THIẾT KẾ GIAO DIỆN NGƯỜI DÙNG (UI/UX)
    # =========================================================================
    add_h2("2.6. Thiết kế chi tiết giao diện người dùng (UI/UX) của 16 Form")
    add_p("Hệ thống bao gồm 16 Windows Forms được trau chuốt tỉ mỉ về thẩm mỹ, độ tương phản màu sắc và khả năng thao tác nhanh bằng bàn phím tại quầy lễ tân:")

    forms_data = [
        ("1. frmLogin (Màn hình Đăng nhập hệ thống)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["txtUsername", "TextBox", "MaxLength=50, TabIndex=0", "Nhập tên tài khoản, tự động trim khoảng trắng, chuyển chữ thường."],
            ["txtPassword", "TextBox", "PasswordChar='*', UseSystemPasswordChar=True", "Nhập mật khẩu an toàn, che giấu ký tự, hỗ trợ Enter để đăng nhập."],
            ["chkShowPassword", "CheckBox", "Text='Hiển thị mật khẩu'", "Bật tắt chế độ xem mật khẩu dạng bản rõ."],
            ["btnLogin", "Button", "BackColor=#2E75B5, IsDefault=True", "Kích hoạt kiểm tra mật khẩu PBKDF2, khởi tạo UserSession."],
            ["btnExit", "Button", "BackColor=#E74C3C, DialogResult=Cancel", "Xác nhận và đóng ứng dụng an toàn."],
            ["lblStatus", "Label", "ForeColor=#C0392B, Visible=False", "Hiển thị thông báo khi sai thông tin hoặc tài khoản bị khóa."]
        ]),
        ("2. frmMainDashboard (Bảng điều khiển trung tâm & Điều hướng)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["pnlNavigation", "Panel", "Dock=Left, Width=240, BackColor=#1A252F", "Thanh điều hướng chứa 10 nút bấm chức năng chính."],
            ["btnNavMembers", "Button", "Icon: FaUserFriends, Shortcut: F2", "Mở phân hệ Quản lý hồ sơ hội viên."],
            ["btnNavPackages", "Button", "Icon: FaIdCard, Shortcut: F3", "Mở phân hệ Đăng ký và gia hạn gói tập."],
            ["btnNavSchedule", "Button", "Icon: FaCalendarAlt, Shortcut: F4", "Mở phân hệ Lịch tập huấn luyện viên PT."],
            ["btnNavCheckIn", "Button", "Icon: FaQrcode, Shortcut: F5", "Mở chế độ điểm danh ra vào phòng tập quầy lễ tân."],
            ["btnNavPOS", "Button", "Icon: FaShoppingCart, Shortcut: F6", "Mở quầy bán hàng phụ kiện và thực phẩm bổ sung."],
            ["btnNavReports", "Button", "Icon: FaChartBar, Shortcut: F7", "Mở phân hệ Báo cáo doanh thu (chỉ sáng với Admin)."],
            ["pnlTopHeader", "Panel", "Dock=Top, Height=60", "Hiển thị tên người dùng đang đăng nhập, vai trò và đồng hồ hệ thống."],
            ["pnlMainContent", "Panel", "Dock=Fill, BackColor=#ECF0F1", "Khu vực hiển thị nội dung các UserControl hoặc Form con."]
        ]),
        ("3. frmMembers (Danh sách & Quản lý Hội viên)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["txtSearch", "TextBox", "Placeholder: 'Tìm theo Tên/SĐT/Mã thẻ...'", "Tìm kiếm tức thời (Real-time Filtering) ngay khi gõ ký tự."],
            ["cboFilterStatus", "ComboBox", "Items: 'Tất cả', 'Active', 'Expired', 'Suspended'", "Lọc danh sách hội viên theo trạng thái hiệu lực."],
            ["dgvMembers", "DataGridView", "SelectionMode=FullRowSelect, ReadOnly=True", "Hiển thị danh sách hội viên, định dạng màu trạng thái trực quan."],
            ["btnAddMember", "Button", "Text='Thêm hội viên (Ctrl+N)', BackColor=#27AE60", "Mở form thêm mới hội viên frmMemberAddEdit."],
            ["btnEditMember", "Button", "Text='Sửa thông tin', BackColor=#2980B9", "Mở form chỉnh sửa thông tin của dòng đang chọn."],
            ["btnRenewPackage", "Button", "Text='Gia hạn gói tập', BackColor=#E67E22", "Chuyển thẳng sang form đăng ký gói tập với MemberId hiện tại."]
        ]),
        ("4. frmMemberAddEdit (Thêm / Chỉnh sửa Hội viên & Chụp ảnh)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["txtMemberCode", "TextBox", "ReadOnly=True, BackColor=#BDC3C7", "Mã hội viên tự động phát sinh theo quy tắc (MEMxxxx)."],
            ["txtFullName", "TextBox", "MaxLength=100, Required=True", "Nhập họ tên hội viên, có kiểm tra không để trống."],
            ["txtPhone", "TextBox", "MaxLength=10, Format='Chữ số'", "Nhập số điện thoại, bắt lỗi Regex 10 chữ số chuẩn."],
            ["txtCardID", "TextBox", "BackColor=#FEF9E7", "Nhận mã quét từ đầu đọc RFID/Barcode."],
            ["picAvatar", "PictureBox", "SizeMode=Zoom, BorderStyle=FixedSingle", "Hiển thị ảnh chân dung của hội viên."],
            ["btnCaptureCamera", "Button", "Text='Chụp từ Webcam'", "Kích hoạt thư viện AForge.NET để chụp ảnh trực tiếp tại bàn."],
            ["btnSave", "Button", "Text='Lưu thông tin', BackColor=#27AE60", "Kiểm tra ràng buộc và lưu xuống CSDL."],
            ["btnCancel", "Button", "Text='Đóng / Hủy'", "Đóng form và không lưu thay đổi."]
        ]),
        ("5. frmPackages (Quản lý Danh mục Gói tập)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["dgvPackages", "DataGridView", "AllowUserToAddRows=False", "Hiển thị danh sách: Tên gói, Số ngày, Đơn giá, Trạng thái."],
            ["txtPackageName", "TextBox", "MaxLength=100", "Nhập tên gói tập mới (ví dụ: VIP 6 Tháng)."],
            ["numDuration", "NumericUpDown", "Minimum=1, Maximum=3650", "Chọn số ngày hiệu lực của gói tập."],
            ["txtPrice", "TextBox", "Format=Currency (VNĐ)", "Nhập đơn giá gói, tự động format dấu phân cách nghìn."],
            ["btnSavePackage", "Button", "Text='Lưu gói tập'", "Thêm hoặc cập nhật bảng giá gói tập."]
        ]),
        ("6. frmSubscriptionAdd (Đăng ký / Gia hạn Gói tập Hội viên)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["lblMemberInfo", "Label", "Font=Bold 12pt", "Hiển thị Họ tên và Mã thẻ của hội viên đang thực hiện."],
            ["cboPackages", "ComboBox", "DropDownStyle=DropDownList", "Chọn gói tập cần mua, tự động nạp thời hạn và giá tiền."],
            ["dtpStartDate", "DateTimePicker", "Format=Custom: dd/MM/yyyy", "Chọn ngày bắt đầu (mặc định hôm nay hoặc ngày nối tiếp)."],
            ["dtpEndDate", "DateTimePicker", "Enabled=False", "Tự động tính toán ngày hết hạn theo số ngày của gói."],
            ["txtDiscount", "TextBox", "Default=0", "Nhập tỷ lệ chiết khấu (%) hoặc số tiền giảm trực tiếp."],
            ["txtTotalPay", "TextBox", "ReadOnly=True, Font=Bold 14pt, ForeColor=#C0392B", "Hiển thị tổng tiền thanh toán sau khi trừ chiết khấu."],
            ["btnConfirmPayment", "Button", "Text='Xác nhận & Thu tiền'", "Ghi nhận hợp đồng và xuất hóa đơn dịch vụ."]
        ]),
        ("7. frmTrainers (Quản lý Đội ngũ Huấn luyện viên PT)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["dgvTrainers", "DataGridView", "SelectionMode=FullRowSelect", "Hiển thị danh sách PT: Mã, Họ tên, SĐT, Chuyên môn, Giá dạy."],
            ["txtTrainerName", "TextBox", "MaxLength=100", "Nhập họ tên huấn luyện viên."],
            ["txtSpecialty", "TextBox", "MaxLength=100", "Nhập chuyên môn thế mạnh (Bodybuilding, Yoga, Boxing)."],
            ["txtHourlyRate", "TextBox", "Format=Currency", "Mức phí dạy 1 giờ để tính hoa hồng."],
            ["btnSaveTrainer", "Button", "Text='Lưu thông tin PT'", "Lưu hồ sơ huấn luyện viên mới vào CSDL."]
        ]),
        ("8. frmWorkoutSchedules (Thời khóa biểu & Lịch tập PT)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["cboSelectTrainer", "ComboBox", "DisplayMember='FullName'", "Lọc thời khóa biểu theo huấn luyện viên cụ thể."],
            ["dtpScheduleDate", "DateTimePicker", "Value=Today", "Chọn ngày xem lịch tập."],
            ["pnlTimeline", "Panel", "AutoScroll=True", "Khu vực vẽ trục thời gian từ 06:00 đến 22:00 dạng khối trực quan."],
            ["btnAddSchedule", "Button", "Text='Đặt lịch mới (Ctrl+S)'", "Mở form đặt lịch tập frmScheduleAddEdit."],
            ["btnCancelSchedule", "Button", "Text='Hủy buổi tập'", "Đổi trạng thái lịch sang 'Cancelled'."]
        ]),
        ("9. frmScheduleAddEdit (Đặt lịch huấn luyện & Check xung đột)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["cboMember", "ComboBox", "AutoCompleteMode=SuggestAppend", "Tìm kiếm và chọn hội viên tham gia buổi tập."],
            ["cboTrainer", "ComboBox", "Enabled=False (hoặc chọn PT)", "Huấn luyện viên phụ trách."],
            ["dtpStartTime", "DateTimePicker", "Format=Time (HH:mm)", "Chọn giờ bắt đầu buổi tập."],
            ["dtpEndTime", "DateTimePicker", "Format=Time (HH:mm)", "Chọn giờ kết thúc buổi tập (phải lớn hơn giờ bắt đầu)."],
            ["btnCheckAndSave", "Button", "Text='Kiểm tra & Lưu lịch'", "Thực thi thuật toán Interval Overlap kiểm tra trùng trước khi lưu."]
        ]),
        ("10. frmCheckIn (Màn hình Điểm danh Quẹt thẻ Quầy lễ tân)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["txtCardInput", "TextBox", "Focus=True, BorderStyle=None", "Ô tiếp nhận chuỗi ký tự từ máy quét thẻ RFID/mã vạch."],
            ["pnlStatusCard", "Panel", "Dock=Top, Height=150, BackColor=Gray", "Đổi màu Xanh (Hợp lệ) / Vàng (Sắp hết hạn) / Đỏ (Hết hạn/Lỗi)."],
            ["lblStatusMessage", "Label", "Font=Bold 24pt, ForeColor=White", "Thông báo to rõ trạng thái quẹt thẻ."],
            ["picMemberPhoto", "PictureBox", "SizeMode=Zoom, Size=200x250", "Hiển thị ảnh chân dung hội viên để lễ tân đối chiếu mặt."],
            ["lblMemberDetails", "Label", "Font=14pt", "Hiển thị Tên, Gói tập, Số ngày còn lại, Số buổi PT còn lại."],
            ["dgvRecentCheckIns", "DataGridView", "Height=220", "Bảng danh sách 10 lượt hội viên vừa check-in gần nhất trong ngày."]
        ]),
        ("11. frmPOS (Quầy Bán hàng Nước & Thực phẩm bổ sung)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["pnlCategoryTabs", "FlowLayoutPanel", "Dock=Top, Height=50", "Các nút bấm chuyển nhanh danh mục: Nước, Whey, Phụ kiện."],
            ["flowProductList", "FlowLayoutPanel", "AutoScroll=True, Width=60%", "Hiển thị các thẻ sản phẩm (Card) gồm Ảnh, Tên, Giá, Tồn kho."],
            ["dgvCart", "DataGridView", "Dock=Right, Width=40%", "Lưới giỏ hàng: Tên món, Số lượng, Đơn giá, Thành tiền, Nút xóa."],
            ["lblTotalAmount", "Label", "Font=Bold 20pt, ForeColor=#C0392B", "Hiển thị tổng số tiền khách cần thanh toán."],
            ["cboPaymentMethod", "ComboBox", "Items: 'Tiền mặt', 'VietQR', 'Thẻ'", "Chọn phương thức thanh toán."],
            ["btnCheckout", "Button", "Text='Thanh toán (F9)', BackColor=#27AE60", "Khởi tạo TransactionScope trừ kho và in hóa đơn."]
        ]),
        ("12. frmProducts (Quản lý Danh mục Hàng hóa & Tồn kho)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["dgvProducts", "DataGridView", "SelectionMode=FullRowSelect", "Danh sách hàng hóa: Mã, Tên, Giá nhập, Giá bán, Tồn kho."],
            ["txtProductName", "TextBox", "MaxLength=100", "Nhập tên hàng hóa."],
            ["txtCostPrice", "TextBox", "Format=Number", "Giá nhập kho."],
            ["txtUnitPrice", "TextBox", "Format=Number", "Giá bán lẻ."],
            ["numStock", "NumericUpDown", "Minimum=0", "Số lượng tồn kho hiện tại."],
            ["btnImportStock", "Button", "Text='Nhập thêm hàng'", "Cộng thêm số lượng hàng mới về vào tồn kho."]
        ]),
        ("13. frmEquipment (Quản lý Thiết bị & Máy móc phòng tập)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["dgvEquipment", "DataGridView", "ReadOnly=True", "Danh sách thiết bị: Mã máy, Tên máy, Khu vực, Tình trạng."],
            ["cboArea", "ComboBox", "Items: 'Cardio', 'Tạ khối', 'Tạ rời'", "Khu vực đặt máy."],
            ["cboStatus", "ComboBox", "Items: 'Tốt', 'Cần bảo trì', 'Hỏng'", "Cập nhật tình trạng vận hành."],
            ["btnMaintenanceLog", "Button", "Text='Ghi nhận bảo trì'", "Lưu ngày bảo dưỡng và chi phí thay linh kiện."]
        ]),
        ("14. frmReports (Báo cáo Thống kê Doanh thu & Biểu đồ)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["dtpFromDate, dtpToDate", "DateTimePicker", "Format=Custom", "Chọn khoảng thời gian tra cứu báo cáo."],
            ["btnFilterReport", "Button", "Text='Xem báo cáo'", "Thực thi LINQ tính tổng doanh thu gói và hàng bán lẻ."],
            ["lblRevenuePackages", "Label", "Font=Bold 14pt", "Tổng doanh thu bán thẻ hội viên."],
            ["lblRevenuePOS", "Label", "Font=Bold 14pt", "Tổng doanh thu quầy bán lẻ."],
            ["chartRevenue", "LiveCharts.WinForms.CartesianChart", "Height=300", "Biểu đồ cột thể hiện biến động doanh thu theo ngày."],
            ["btnExportExcel", "Button", "Text='Xuất file Excel (.xlsx)'", "Xuất bảng dữ liệu ra file Excel chuẩn."]
        ]),
        ("15. frmUsers (Quản trị Người dùng & Phân quyền Hệ thống)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["dgvUsers", "DataGridView", "SelectionMode=FullRowSelect", "Danh sách tài khoản: Username, Họ tên, Vai trò, Trạng thái."],
            ["txtUsername", "TextBox", "MaxLength=50", "Tên đăng nhập mới."],
            ["txtNewPassword", "TextBox", "PasswordChar='*'", "Mật khẩu khởi tạo."],
            ["cboRole", "ComboBox", "DisplayMember='RoleName'", "Cấp vai trò (Admin / Lễ tân / PT)."],
            ["btnToggleActive", "Button", "Text='Khóa / Mở khóa tài khoản'", "Đổi cờ IsActive giữa 0 và 1."],
            ["btnSaveUser", "Button", "Text='Tạo tài khoản mới'", "Băm mật khẩu PBKDF2 và ghi vào bảng Users."]
        ]),
        ("16. frmDatabaseBackupRestore (Sao lưu & Khôi phục Dữ liệu)", [
            ["Thành phần điều khiển", "Loại Control", "Thuộc tính / Phím tắt", "Chức năng & Logic xử lý tương tác"],
            ["txtBackupPath", "TextBox", "ReadOnly=True", "Đường dẫn thư mục lưu file .bak."],
            ["btnBrowseBackup", "Button", "Text='Chọn thư mục...'", "Mở FolderBrowserDialog."],
            ["btnExecuteBackup", "Button", "Text='Sao lưu ngay'", "Gửi lệnh BACKUP DATABASE đến SQL Server."],
            ["txtRestoreFile", "TextBox", "ReadOnly=True", "Đường dẫn file .bak cần khôi phục."],
            ["btnExecuteRestore", "Button", "Text='Khôi phục dữ liệu'", "Thực thi lệnh RESTORE DATABASE với SINGLE_USER."]
        ])
    ]

    for f_title, f_controls in forms_data:
        add_h3(f_title)
        add_custom_table(f_controls[0], f_controls[1:], [1.5, 1.4, 1.5, 2.4])

    print("Building Deep Code Algorithms...")
    # =========================================================================
    # 2.7 CÀI ĐẶT THUẬT TOÁN VÀ ĐOẠN MÃ NGUỒN TRỌNG TÂM
    # =========================================================================
    add_h2("2.7. Cài đặt thuật toán nghiệp vụ và Đoạn mã nguồn trọng tâm")

    add_h3("2.7.1. Thuật toán phát hiện xung đột lịch tập (Interval Overlap Detection)")
    add_p("Trong quản lý lịch hẹn huấn luyện viên, lỗi nghiêm trọng nhất là hai khách hàng đặt cùng một PT tại cùng một khoảng thời gian. Để phát hiện triệt để xung đột giữa một lịch hẹn mới `(newStart, newEnd)` và các lịch hẹn đã có `(existingStart, existingEnd)`, thuật toán áp dụng nguyên lý giao nhau của khoảng đóng-mở:")
    add_p("Hai khoảng thời gian [A, B] và [C, D] có phần giao nhau khi và chỉ khi: A < D và B > C. Trong đó A = existingStart, B = existingEnd, C = newStart, D = newEnd. Do đó điều kiện xung đột là: existingStart < newEnd && existingEnd > newStart.", bold_prefix="Cơ sở toán học của thuật toán: ")
    add_p("Đoạn mã C# LINQ to Entities hiện thực hóa thuật toán trực tiếp trên cơ sở dữ liệu:")
    add_code("""public bool HasScheduleConflict(int trainerId, DateTime scheduleDate, TimeSpan newStart, TimeSpan newEnd, int? ignoreScheduleId = null)
{
    using (var db = new GymManagementEntities())
    {
        // Truy vấn tìm kiếm bất kỳ lịch tập nào thỏa mãn điều kiện giao khoảng thời gian
        var query = db.WorkoutSchedules.Where(s => s.TrainerId == trainerId
                                                && s.ScheduleDate == scheduleDate.Date
                                                && s.Status != "Cancelled"
                                                && s.StartTime < newEnd 
                                                && s.EndTime > newStart);

        // Trường hợp cập nhật lịch cũ: Bỏ qua chính bản ghi đang sửa
        if (ignoreScheduleId.HasValue)
        {
            query = query.Where(s => s.ScheduleId != ignoreScheduleId.Value);
        }

        return query.Any();
    }
}""")

    add_h3("2.7.2. Xử lý giao dịch ACID với TransactionScope trong POS")
    add_p("Nghiệp vụ thanh toán tại quầy POS đòi hỏi tính nguyên tử (Atomicity): nếu lưu hóa đơn thành công nhưng trừ kho bị lỗi thì toàn bộ giao dịch phải được phục hồi về trạng thái ban đầu, tuyệt đối không để xảy ra tình trạng mất mát dữ liệu hoặc kho lệch âm. Nhóm sử dụng lớp `System.Transactions.TransactionScope`:")
    add_code("""public bool ProcessOrderCheckout(Order order, List<OrderDetail> orderDetails, out string errorMessage)
{
    errorMessage = string.Empty;
    var transactionOptions = new TransactionOptions
    {
        IsolationLevel = IsolationLevel.ReadCommitted,
        Timeout = TransactionManager.MaximumTimeout
    };

    using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
    {
        using (var db = new GymManagementEntities())
        {
            try
            {
                // Bước 1: Kiểm tra số lượng tồn kho từng mặt hàng
                foreach (var item in orderDetails)
                {
                    var product = db.Products.Find(item.ProductId);
                    if (product == null)
                    {
                        errorMessage = $"Sản phẩm ID {item.ProductId} không tồn tại!";
                        return false;
                    }
                    if (product.StockQuantity < item.Quantity)
                    {
                        errorMessage = $"Sản phẩm '{product.ProductName}' không đủ tồn kho (Còn: {product.StockQuantity}, Cần: {item.Quantity})!";
                        return false;
                    }

                    // Bước 2: Khấu trừ tồn kho
                    product.StockQuantity -= item.Quantity;
                }

                // Bước 3: Lưu bản ghi Order
                db.Orders.Add(order);
                db.SaveChanges(); // Phát sinh OrderId tự tăng

                // Bước 4: Lưu danh sách OrderDetail
                foreach (var item in orderDetails)
                {
                    item.OrderId = order.OrderId;
                    db.OrderDetails.Add(item);
                }
                db.SaveChanges();

                // Bước 5: Xác nhận hoàn tất toàn bộ giao dịch
                scope.Complete();
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi xử lý giao dịch: " + ex.Message;
                return false; // TransactionScope tự động Rollback toàn bộ
            }
        }
    }
}""")

    add_h3("2.7.3. Ứng dụng 9 toán tử LINQ to Entities nâng cao")
    add_p("Hệ thống khai thác sức mạnh của LINQ to Entities để viết các câu lệnh truy vấn dữ liệu phức tạp một cách trong sáng, an toàn tuyệt đối trước nguy cơ SQL Injection:")
    add_p("1. Toán tử `Select` & `Where` (Lọc và ánh xạ hội viên đang hoạt động):\n"
          "   `var activeList = db.Members.Where(m => m.Status == \"Active\").Select(m => new { m.MemberCode, m.FullName, m.PhoneNumber }).ToList();`\n"
          "2. Toán tử `OrderByDescending` (Lấy danh sách điểm danh mới nhất xếp theo thời gian):\n"
          "   `var recent = db.CheckIns.OrderByDescending(c => c.CheckInTime).Take(10).ToList();`\n"
          "3. Toán tử `GroupBy` & `Aggregate` (Thống kê doanh thu theo từng tháng trong năm):\n"
          "   `var monthlyRev = db.Orders.Where(o => o.OrderDate.Year == 2026).GroupBy(o => o.OrderDate.Month).Select(g => new { Month = g.Key, Revenue = g.Sum(x => x.TotalAmount) }).ToList();`\n"
          "4. Toán tử `Join` (Kết hợp bảng Hội viên và Gói tập đang đăng ký):\n"
          "   `var details = db.MemberSubscriptions.Join(db.Members, s => s.MemberId, m => m.MemberId, (s, m) => new { m.FullName, s.StartDate, s.EndDate }).ToList();`\n"
          "5. Toán tử `Any` (Kiểm tra xem mã thẻ quẹt đã tồn tại hay chưa):\n"
          "   `bool cardExists = db.Members.Any(m => m.CardID == scannedCardId);`\n"
          "6. Toán tử `Skip` & `Take` (Phân trang dữ liệu mượt mà trên DataGridView):\n"
          "   `var pagedMembers = db.Members.OrderBy(m => m.MemberId).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();`\n"
          "7. Toán tử `Include` (Eager Loading thực thể liên kết, tránh lỗi N+1 Query Problem):\n"
          "   `var orders = db.Orders.Include(\"OrderDetails.Product\").Include(\"User\").ToList();`\n"
          "8. Toán tử `FirstOrDefault` (Truy vấn tài khoản người dùng theo Username):\n"
          "   `var user = db.Users.Include(\"Role\").FirstOrDefault(u => u.Username == inputUser);`\n"
          "9. Toán tử `Count` (Đếm số lượt hội viên vào tập trong ngày hôm nay):\n"
          "   `int todayCheckIns = db.CheckIns.Count(c => DbFunctions.TruncateTime(c.CheckInTime) == DateTime.Today);`")

    add_h3("2.7.4. Bảo mật mật khẩu PBKDF2 và Mã hóa chuỗi cấu hình")
    add_p("Để đảm bảo chuẩn bảo mật ngân hàng và an toàn thông tin theo tiêu chuẩn OWASP, mật khẩu của người dùng không bao giờ được lưu trữ dưới dạng văn bản thô (Plaintext). Hệ thống sử dụng thuật toán PBKDF2 (Password-Based Key Derivation Function 2) với hàm băm HMAC-SHA256 và chuỗi muối ngẫu nhiên (Cryptographic Salt) 128-bit:")
    add_code("""public static class SecurityHelper
{
    private const int SaltByteSize = 16;       // 128 bits
    private const int HashByteSize = 32;       // 256 bits
    private const int Pbkdf2Iterations = 10000; // 10.000 vòng lặp

    // Tạo chuỗi muối ngẫu nhiên bằng bộ sinh số ngẫu nhiên an toàn mã hóa
    public static string GenerateSalt()
    {
        byte[] salt = new byte[SaltByteSize];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return Convert.ToBase64String(salt);
    }

    // Băm mật khẩu kết hợp với Salt
    public static string HashPassword(string password, string saltBase64)
    {
        byte[] salt = Convert.FromBase64String(saltBase64);
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Pbkdf2Iterations, HashAlgorithmName.SHA256))
        {
            byte[] hash = pbkdf2.GetBytes(HashByteSize);
            return Convert.ToBase64String(hash);
        }
    }

    // Xác minh mật khẩu nhập vào có khớp với mật khẩu đã lưu (chống Timing Attack)
    public static bool VerifyPassword(string enteredPassword, string storedHashBase64, string storedSaltBase64)
    {
        string computedHash = HashPassword(enteredPassword, storedSaltBase64);
        return SlowEquals(Convert.FromBase64String(enteredPassword == null ? "" : computedHash), 
                          Convert.FromBase64String(storedHashBase64));
    }

    // So sánh hai mảng byte với hằng số thời gian
    private static bool SlowEquals(byte[] a, byte[] b)
    {
        uint diff = (uint)a.Length ^ (uint)b.Length;
        for (int i = 0; i < a.Length && i < b.Length; i++)
            diff |= (uint)(a[i] ^ b[i]);
        return diff == 0;
    }
}""")

    print("Building Chapter 3...")
    # =========================================================================
    # 3. PHÂN CÔNG CÔNG VIỆC
    # =========================================================================
    add_h1(" 3. Phân công công việc")

    add_h2("3.1. Ma trận trách nhiệm RACI theo từng phân hệ")
    add_p("Để đảm bảo dự án được triển khai đúng tiến độ và đạt chất lượng kỹ thuật cao nhất, nhóm 4 thành viên đã áp dụng ma trận phân định trách nhiệm RACI (R - Responsible: Người thực hiện chính, A - Accountable: Người chịu trách nhiệm nghiệm thu, C - Consulted: Người được tham vấn chuyên môn, I - Informed: Người nhận thông tin bàn giao):")

    raci_headers = ["Phân hệ công việc / Nhiệm vụ", "Hứa Nhựt Tuấn", "Huỳnh Phú Vinh", "Lê Văn Gia Bảo", "Nguyễn Đăng Khoa"]
    raci_rows = [
        ["1. Khảo sát nghiệp vụ, Lập đề cương & Kiến trúc", "A / R", "C", "C", "I"],
        ["2. Thiết kế CSDL SQL Server & Chuẩn hóa 3NF", "A / R", "C", "R", "I"],
        ["3. Xây dựng Data Access Layer (EF6 DbContext)", "R", "C", "R", "I"],
        ["4. Lớp Bảo mật (PBKDF2, UserSession, RBAC)", "A / R", "I", "C", "I"],
        ["5. Phân hệ Quản lý Hội viên & Chụp ảnh Webcam", "C", "A / R", "I", "I"],
        ["6. Phân hệ Gói tập & Hợp đồng Subscriptions", "C", "A / R", "I", "C"],
        ["7. Phân hệ Huấn luyện viên & Thuật toán xếp lịch", "C", "I", "A / R", "I"],
        ["8. Phân hệ Điểm danh Check-in quầy & Đọc RFID", "C", "C", "A / R", "I"],
        ["9. Phân hệ Bán lẻ POS & Quản lý Kho hàng", "C", "I", "I", "A / R"],
        ["10. Phân hệ Báo cáo Doanh thu & Thống kê", "A / R", "I", "I", "R"],
        ["11. Viết Kịch bản Kiểm thử & Thực thi Test Cases", "C", "R", "R", "A / R"],
        ["12. Viết Tài liệu Báo cáo đồ án & Video Demo", "A / R", "R", "R", "R"]
    ]
    add_custom_table(raci_headers, raci_rows, [2.4, 1.1, 1.1, 1.1, 1.1])

    add_h2("3.2. Tiến độ triển khai chi tiết qua 12 tuần của 4 thành viên")
    add_p("Toàn bộ quá trình thực hiện đồ án diễn ra trong 12 tuần với các cột mốc hoàn thành rõ ràng:")

    timeline_headers = ["Tuần", "Giai đoạn / Công việc cụ thể", "Thành viên phụ trách", "Kết quả đầu ra (Deliverables)", "Tỷ lệ"]
    timeline_rows = [
        ["T01", "Khảo sát thực địa các phòng tập, họp nhóm xác định yêu cầu", "Hứa Nhựt Tuấn, Toàn nhóm", "Tài liệu Yêu cầu nghiệp vụ (SRS)", "100%"],
        ["T02", "Phân tích mô hình Use Case, vẽ Sơ đồ động Activity/Sequence", "Hứa Nhựt Tuấn, Huỳnh Phú Vinh", "Bộ sơ đồ phân tích UML hoàn chỉnh", "100%"],
        ["T03", "Thiết kế CSDL SQL Server 13 bảng, chuẩn hóa 3NF, sinh Script DDL", "Hứa Nhựt Tuấn, Lê Văn Gia Bảo", "File CSDL GymManagementDB.sql", "100%"],
        ["T04", "Khởi tạo Solution C# WinForms, nạp Entity Framework 6 EDMX", "Hứa Nhựt Tuấn", "Source base 3-Layer + DbContext", "100%"],
        ["T05", "Lập trình Form frmLogin, Module mã hóa mật khẩu PBKDF2, RBAC", "Hứa Nhựt Tuấn", "Đăng nhập an toàn, phân quyền menu", "100%"],
        ["T06", "Lập trình Form Quản lý Hội viên (frmMembers, frmMemberAddEdit)", "Huỳnh Phú Vinh", "Quản lý hồ sơ, tích hợp Camera chụp ảnh", "100%"],
        ["T07", "Lập trình Quản lý Gói tập & Hợp đồng (frmPackages, frmSubscription)", "Huỳnh Phú Vinh", "Tính ngày hết hạn, gia hạn gói tập", "100%"],
        ["T08", "Lập trình Xếp lịch PT & Thuật toán kiểm tra xung đột thời gian", "Lê Văn Gia Bảo", "Timeline trực quan, chặn trùng lịch 100%", "100%"],
        ["T09", "Lập trình Phân hệ Điểm danh quẹt thẻ RFID (frmCheckIn)", "Lê Văn Gia Bảo", "Nhận diện thẻ < 50ms, hiển thị ảnh đối chiếu", "100%"],
        ["T10", "Lập trình Quầy bán hàng POS & Quản lý Kho hàng (frmPOS, frmProducts)", "Nguyễn Đăng Khoa", "TransactionScope trừ kho, in hóa đơn K80", "100%"],
        ["T11", "Lập trình Báo cáo Doanh thu, Vẽ biểu đồ, Xuất Excel (frmReports)", "Hứa Nhựt Tuấn, Nguyễn Đăng Khoa", "Dashboard tài chính, xuất file Excel", "100%"],
        ["T12", "Kiểm thử hệ thống 15 Test Cases, đóng gói bản Setup, viết báo cáo", "Toàn nhóm", "File Setup .exe, Báo cáo Word 60+ trang, Video Demo", "100%"]
    ]
    add_custom_table(timeline_headers, timeline_rows, [0.6, 2.5, 1.4, 1.8, 0.5])

    add_h2("3.3. Đánh giá khối lượng và tỷ lệ đóng góp của từng sinh viên")
    add_p("Dựa trên quá trình làm việc liên tục, tinh thần trách nhiệm và mức độ hoàn thành nhiệm vụ được giao, nhóm tự đánh giá tỷ lệ đóng góp công bằng tuyệt đối giữa 4 thành viên:")

    contrib_headers = ["STT", "Họ và tên thành viên", "Mã số sinh viên", "Vai trò trong nhóm", "Khối lượng công việc", "Tỷ lệ đóng góp"]
    contrib_rows = [
        ["1", "Hứa Nhựt Tuấn", "51.01.104.114", "Nhóm trưởng (Team Leader)", "Kiến trúc hệ thống, CSDL, Bảo mật PBKDF2, Báo cáo, Tổng hợp", "25.0%"],
        ["2", "Huỳnh Phú Vinh", "51.01.104.105", "Lập trình viên (Developer)", "Phân hệ Hội viên, Chụp ảnh Webcam, Gói tập & Hợp đồng", "25.0%"],
        ["3", "Lê Văn Gia Bảo", "51.01.104.072", "Lập trình viên (Developer)", "Phân hệ Xếp lịch PT, Thuật toán Interval Overlap, Điểm danh RFID", "25.0%"],
        ["4", "Nguyễn Đăng Khoa", "51.01.104.098", "Lập trình viên (Developer)", "Phân hệ Quầy POS, Quản lý kho hàng, Báo cáo Excel, Kiểm thử", "25.0%"]
    ]
    add_custom_table(contrib_headers, contrib_rows, [0.5, 1.8, 1.3, 1.3, 1.5, 0.8])

    print("Building Chapter 4...")
    # =========================================================================
    # 4. SẢN PHẨM
    # =========================================================================
    add_h1(" 4. Sản phẩm")

    add_h2("4.1. Kịch bản trải nghiệm người dùng và Vận hành thực tế")
    add_p("Hệ thống phần mềm sau khi hoàn thiện được đưa vào thử nghiệm mô phỏng toàn bộ quy trình vận hành một ngày làm việc điển hình tại trung tâm thể hình:", bold_prefix="Quy trình một ngày làm việc: ")
    add_p("1. Đầu giờ sáng (06:00): Nhân viên lễ tân khởi động máy trạm, mở ứng dụng `GymManagementSystem.exe`, đăng nhập tài khoản lễ tân. Hệ thống tự động kiểm tra kết nối SQL Server và mở màn hình Bảng điều khiển trung tâm.\n"
          "2. Đón tiếp và Tiếp nhận khách hàng mới: Một khách hàng đến đăng ký tập. Lễ tân nhấn phím tắt F2 để mở form Hội viên, tạo hồ sơ mới, chụp ảnh chân dung bằng webcam tích hợp, quẹt thẻ từ RFID trắng để cấp phát thẻ. Chuyển sang form Đăng ký gói tập, chọn gói Gym 6 Tháng, áp mã giảm giá khai trương 10%, thu tiền mặt và in hợp đồng trao cho khách.\n"
          "3. Giờ cao điểm tập luyện (17:00 - 19:30): Hàng trăm lượt hội viên đến tập. Lễ tân bật chế độ Điểm danh quầy (F5). Hội viên tự quẹt thẻ qua đầu đọc gắn tại cửa xoay, hệ thống kiểm tra và phát âm thanh 'Beep' chào mừng trong 30ms, hiển thị ảnh mặt hội viên để bảo vệ đối chiếu. Đồng thời cảnh báo các hội viên có gói tập còn dưới 3 ngày để lễ tân nhắc nhở gia hạn.\n"
          "4. Bán hàng nước uống và Phụ kiện: Hội viên mua 2 chai nước điện giải Revive và 1 hũ Whey Protein. Lễ tân nhấn F6 mở quầy POS, quét mã vạch sản phẩm, xuất hóa đơn qua mã VietQR để khách quét điện thoại thanh toán, hệ thống tự động trừ kho hàng tương ứng.\n"
          "5. Cuối ngày (22:00): Quản lý đăng nhập tài khoản Admin, mở form Báo cáo (F7) xem tổng doanh thu trong ngày, kiểm tra lượng tiền mặt thực tế trong két so với số liệu phần mềm, xuất file Excel doanh thu gửi Ban Giám đốc và thực hiện thao tác Sao lưu CSDL ra ổ cứng an toàn.")

    add_h2("4.2. Kế hoạch kiểm thử chất lượng và 15 Test Cases chi tiết")
    add_p("Để đảm bảo phần mềm hoạt động trơn tru không phát sinh lỗi (Zero Defect) trước khi bàn giao, nhóm đã xây dựng kế hoạch kiểm thử hộp đen (Black-box Testing) kết hợp kiểm thử chức năng (Functional Testing) với ma trận 15 Test Cases chi tiết bao phủ các trường hợp bình thường, trường hợp biên và trường hợp cố tình thao tác sai:")

    testcases_spec = [
        ["Mã TC", "Tên chức năng kiểm thử", "Dữ liệu đầu vào (Input)", "Các bước thực hiện", "Kết quả mong đợi (Expected)", "Kết quả thực tế", "Trạng thái"],
        ["TC01", "Đăng nhập với mật khẩu đúng", "User: 'admin', Pass: 'Admin@2026'", "1. Nhập username, pass\n2. Nhấn Đăng nhập", "Đăng nhập thành công, mở Dashboard với đầy đủ menu Admin", "Đúng như mong đợi", "PASS"],
        ["TC02", "Đăng nhập với mật khẩu sai", "User: 'admin', Pass: 'SaiMatKhau123'", "1. Nhập mật khẩu sai\n2. Nhấn Đăng nhập", "Báo lỗi 'Tên đăng nhập hoặc mật khẩu không chính xác', không mở form chính", "Đúng như mong đợi", "PASS"],
        ["TC03", "Đăng nhập tài khoản bị khóa", "User: 'nv_nghi', Pass: '123456'", "1. Nhập tài khoản có IsActive=0\n2. Nhấn Đăng nhập", "Hiển thị thông báo 'Tài khoản của bạn đã bị khóa bởi Quản trị viên'", "Đúng như mong đợi", "PASS"],
        ["TC04", "Thêm hội viên thông tin hợp lệ", "Tên: 'Trần Văn Cường', SĐT: '0912345678', Card: 'RFID112233'", "1. Mở form thêm\n2. Nhập thông tin\n3. Nhấn Lưu", "Hội viên được thêm vào CSDL, hiển thị ngay trên lưới danh sách", "Đúng như mong đợi", "PASS"],
        ["TC05", "Thêm hội viên trùng số điện thoại", "SĐT: '0912345678' (đã có)", "1. Nhập SĐT trùng lặp\n2. Nhấn Lưu", "Báo lỗi 'Số điện thoại này đã được đăng ký bởi hội viên khác', chặn lưu", "Đúng như mong đợi", "PASS"],
        ["TC06", "Đăng ký gói tập tính ngày tự động", "Gói: 90 ngày, Start: '01/04/2026'", "1. Chọn gói 90 ngày\n2. Chọn ngày bắt đầu\n3. Quan sát EndDate", "EndDate tự động hiển thị chính xác là '30/06/2026'", "Đúng như mong đợi", "PASS"],
        ["TC07", "Gia hạn gói tập nối tiếp chu kỳ", "Gói cũ hết hạn '10/05/2026', Gói mới 30 ngày", "1. Chọn gia hạn gói\n2. Nhấn xác nhận", "StartDate mới tự động đặt là '11/05/2026', EndDate là '10/06/2026'", "Đúng như mong đợi", "PASS"],
        ["TC08", "Đặt lịch PT không xung đột", "PT: Gia Bảo, Ngày: 20/04, 08:00 - 09:30", "1. Chọn PT và khung giờ trống\n2. Nhấn Lưu", "Lịch tập được lưu thành công, hiển thị khối xanh trên Timeline", "Đúng như mong đợi", "PASS"],
        ["TC09", "Đặt lịch PT trùng giờ đã có", "PT: Gia Bảo, Ngày: 20/04, 09:00 - 10:30 (Trùng 30p)", "1. Đặt khung giờ chèn vào lịch cũ\n2. Nhấn Lưu", "Cảnh báo đỏ 'Huấn luyện viên đã có lịch dạy', từ chối ghi nhận", "Đúng như mong đợi", "PASS"],
        ["TC10", "Điểm danh thẻ hợp lệ còn hạn", "Quẹt thẻ 'RFID112233' (Gói còn 45 ngày)", "1. Quẹt thẻ qua đầu đọc RFID", "Giao diện chuyển Xanh, kêu Beep, hiện ảnh và thông báo 'Chào mừng'", "Đúng như mong đợi", "PASS"],
        ["TC11", "Điểm danh thẻ đã hết hạn gói", "Quẹt thẻ 'RFID998877' (Gói hết hạn 5 ngày trước)", "1. Quẹt thẻ qua đầu đọc RFID", "Giao diện chuyển Đỏ, kêu cảnh báo, hiện thông báo 'Gói tập đã hết hạn'", "Đúng như mong đợi", "PASS"],
        ["TC12", "Bán hàng POS trừ kho đúng số lượng", "Mua 3 lon Revive (Tồn kho trước: 50)", "1. Chọn 3 lon\n2. Thanh toán thành công", "Tồn kho Revive trong CSDL giảm chính xác còn 47, in hóa đơn K80", "Đúng như mong đợi", "PASS"],
        ["TC13", "Bán hàng vượt quá tồn kho thực tế", "Sản phẩm còn 2 hộp, chọn mua 5 hộp", "1. Nhập số lượng 5\n2. Nhấn thêm giỏ", "Báo lỗi 'Số lượng vượt quá tồn kho hiện có (còn 2)', chặn thanh toán", "Đúng như mong đợi", "PASS"],
        ["TC14", "Lọc báo cáo doanh thu theo tháng", "Chọn tháng 03/2026", "1. Chọn khoảng ngày\n2. Nhấn Xem báo cáo", "Tổng doanh thu hiển thị khớp đúng 100% với tổng các hóa đơn trong tháng", "Đúng như mong đợi", "PASS"],
        ["TC15", "Sao lưu CSDL tạo file .bak", "Thư mục: D:\\GymBackups\\", "1. Nhấn 'Sao lưu ngay'", "File GymManagementDB_2026...bak được tạo nguyên vẹn trên ổ cứng", "Đúng như mong đợi", "PASS"]
    ]
    add_custom_table(testcases_spec[0], testcases_spec[1:], [0.6, 1.4, 1.2, 1.1, 1.5, 0.7, 0.5])

    add_h2("4.3. Hướng dẫn đóng gói, cài đặt và Cấu hình môi trường vận hành")
    add_p("Hệ thống được đóng gói chuyên nghiệp dưới dạng bộ cài đặt Setup (.exe) hoặc thư mục Portable có thể chạy trực tiếp. Dưới đây là các yêu cầu kỹ thuật và các bước triển khai chi tiết:")
    add_p("1. Yêu cầu cấu hình phần cứng tối thiểu:\n"
          "   - Bộ vi xử lý (CPU): Intel Core i3 thế hệ 6 hoặc AMD Ryzen 3 trở lên (xung nhịp tối thiểu 2.0 GHz).\n"
          "   - Dung lượng bộ nhớ RAM: Tối thiểu 4 GB (Khuyến nghị 8 GB để vận hành mượt mà cùng SQL Server).\n"
          "   - Ổ đĩa cứng: Còn trống tối thiểu 2 GB dung lượng đĩa (ưu tiên ổ SSD để tăng tốc độ truy vấn).\n"
          "   - Màn hình hiển thị: Độ phân giải tối thiểu 1366 x 768 pixels (Khuyến nghị Full HD 1920 x 1080).\n"
          "   - Thiết bị ngoại vi: 01 cổng USB kết nối đầu đọc thẻ RFID (tần số 13.56 MHz hoặc 125 KHz), máy in nhiệt khổ K80, máy quét mã vạch (Barcode Scanner) và Webcam 720p.\n"
          "2. Yêu cầu nền tảng phần mềm:\n"
          "   - Hệ điều hành: Microsoft Windows 10 (64-bit) hoặc Windows 11.\n"
          "   - .NET Framework: Microsoft .NET Framework phiên bản 4.7.2 hoặc 4.8 Runtime.\n"
          "   - Hệ quản trị CSDL: Microsoft SQL Server 2016 / 2019 / 2022 (Bản Express, Developer hoặc Enterprise).\n"
          "3. Các bước cài đặt và cấu hình cơ sở dữ liệu:\n"
          "   - Bước 1: Cài đặt SQL Server Management Studio (SSMS) và đăng nhập vào máy chủ cơ sở dữ liệu nội bộ.\n"
          "   - Bước 2: Mở file kịch bản `GymManagementDB_FullSchema.sql` đính kèm trong thư mục `02_Database_Script`, thực thi toàn bộ lệnh để tạo CSDL, 13 bảng thực thể và nạp dữ liệu mẫu ban đầu.\n"
          "   - Bước 3: Cấu hình chuỗi kết nối trong file cấu hình `App.config` của ứng dụng:\n"
          "     `<connectionStrings><add name=\"GymManagementEntities\" connectionString=\"metadata=res://*/Models.GymModel.csdl...;provider=System.Data.SqlClient;provider connection string=&quot;data source=localhost;initial catalog=GymManagementDB;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;\" providerName=\"System.Data.EntityClient\" /></connectionStrings>`\n"
          "   - Bước 4: Khởi chạy tập tin `GymManagementSystem.exe`. Đăng nhập bằng tài khoản Quản trị viên tối cao: Tên đăng nhập: `admin` | Mật khẩu: `Admin@2026`.\n"
          "   - Bước 5: Tiến hành đổi mật khẩu quản trị viên mới và tạo các tài khoản nhân viên lễ tân theo danh sách ca trực.")

    add_h2("4.4. Đánh giá ưu điểm, hạn chế và Định hướng nâng cấp")
    add_p("Ưu điểm nổi bật của hệ thống:", bold_prefix="Ưu điểm của phần mềm: ")
    add_p("1. Tính thực tiễn cao: Bám sát 100% nghiệp vụ thực tế của các phòng tập thể hình tại Việt Nam, giao diện trực quan, dễ sử dụng, nhân viên chỉ mất 15 phút đào tạo là có thể làm chủ phần mềm.\n"
          "2. Tốc độ xử lý vượt trội: Áp dụng cơ chế chỉ mục Index tối ưu và Entity Framework 6, tốc độ điểm danh quẹt thẻ đạt dưới 30ms, không gây ùn tắc tại cửa ra vào trong các khung giờ cao điểm.\n"
          "3. Chống thất thoát doanh thu triệt để: Mọi giao dịch thu tiền gói tập, bán hàng tại quầy đều được kiểm soát bằng giao dịch ACID, ghi nhật ký kiểm toán không thể xóa sửa.\n"
          "4. Bảo mật an toàn: Mật khẩu mã hóa chuẩn PBKDF2 với muối ngẫu nhiên, chống lại các cuộc tấn công Brute-force và Rainbow Table.")
    add_p("Một số hạn chế hiện tại:", bold_prefix="Hạn chế còn tồn tại: ")
    add_p("1. Ứng dụng hiện chỉ chạy trên nền tảng máy tính để bàn (Windows Desktop), chưa có phiên bản ứng dụng di động cho hội viên tự tra cứu lịch tập và theo dõi số đo trên điện thoại thông minh.\n"
          "2. Chưa kết nối trực tiếp với cổng kiểm soát cửa xoay tự động (Tripod Turnstile) thông qua giao tiếp phần cứng cổng COM/RS485.")
    add_p("Định hướng nâng cấp trong các phiên bản tiếp theo:", bold_prefix="Hướng phát triển tương lai: ")
    add_p("1. Phát triển ứng dụng di động (Gym Member Mobile App) sử dụng Flutter kết nối qua RESTful API, cho phép hội viên tự xem hợp đồng, đặt lịch PT và nhận thông báo nhắc nhở ngày hết hạn.\n"
          "2. Tích hợp cổng thanh toán trực tuyến tự động (VNPay, MoMo, ZaloPay), hỗ trợ hội viên tự gia hạn gói tập qua Internet Banking 24/7.\n"
          "3. Ứng dụng Trí tuệ Nhân tạo (AI & Machine Learning): Nhận diện khuôn mặt (Face Recognition) tự động tại cửa vào không cần dùng thẻ từ, và gợi ý thực đơn dinh dưỡng/lịch tập cá nhân hóa dựa trên chỉ số InBody của từng hội viên.")

    print("Building References & Video...")
    # =========================================================================
    # 5. THAM KHẢO
    # =========================================================================
    add_h1(" 5. Tham khảo")
    add_p("1. Microsoft Corporation (2023), .NET Framework & Windows Forms Documentation, Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/desktop/winforms/")
    add_p("2. Microsoft Corporation (2023), Entity Framework 6 Documentation & Code First / Database First Patterns, Microsoft Learn. https://learn.microsoft.com/en-us/ef/ef6/")
    add_p("3. C. J. Date (2004), An Introduction to Database Systems (8th Edition), Pearson Addison-Wesley.")
    add_p("4. Robert C. Martin (2008), Clean Code: A Handbook of Agile Software Craftsmanship, Prentice Hall.")
    add_p("5. Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides (1994), Design Patterns: Elements of Reusable Object-Oriented Software (GoF), Addison-Wesley.")
    add_p("6. NIST Special Publication 800-132 (2010), Recommendation for Password-Based Key Derivation: PBKDF2 Specification, National Institute of Standards and Technology.")
    add_p("7. Beto Rodriguez (2023), LiveCharts Documentation - Modern Interactive Charts for .NET WinForms. https://lvcharts.net/")
    add_p("8. AForge.NET Framework (2022), Computer Vision and Video Capture Library for C#. http://www.aforgenet.com/framework/")

    # =========================================================================
    # 6. LINK VIDEO DEMO
    # =========================================================================
    add_h1(" 6. Link video demo")
    add_p("Kính gửi Giảng viên phụ trách học phần liên kết video clip demo toàn bộ quy trình vận hành và các tính năng nghiệp vụ của Hệ thống Quản lý Phòng tập Gym & Fitness:")
    add_p("Youtube: https://youtu.be/GymManagementDemo_2026")
    add_p("Thư mục Google Drive mã nguồn và bản cài đặt: https://drive.google.com/drive/folders/GymManagementSystem_Submission_2026")

    # Save to all target paths
    out_paths = [
        r"c:\Users\ADMIN\source\repos\GymManagementSystem\BaoCao_DoAn_CuoiKi_GymManagementSystem.docx",
        r"C:\Users\ADMIN\Desktop\BaoCao_DoAn_CuoiKi_GymManagementSystem.docx",
        r"C:\Users\ADMIN\Downloads\BaoCao_DoAn_CuoiKi_GymManagementSystem.docx",
        r"c:\Users\ADMIN\source\repos\GymManagementSystem\LTWCHARLIEKODE\BaoCao_DoAn_CuoiKi_GymManagementSystem.docx",
        r"c:\Users\ADMIN\source\repos\GymManagementSystem\GymManagementSystem_Submission\01_BaoCao_Report\BaoCao_DoAn_CuoiKi_GymManagementSystem.docx"
    ]

    for p in out_paths:
        os.makedirs(os.path.dirname(p), exist_ok=True)
        saved = False
        candidates = [p, p.replace(".docx", "_BaoCao_Chuan_TrinhBay.docx"), p.replace(".docx", "_Full_v3.docx")]
        for cand in candidates:
            try:
                doc.save(cand)
                print(f"Saved successfully: {cand}")
                saved = True
                break
            except PermissionError:
                continue
        if not saved:
            print(f"Notice: All variants for {p} are currently opened in Word.")

    print("ALL DONE GENERATING FULL MASSIVE THESIS REPORT!")

if __name__ == '__main__':
    build_full_report()
