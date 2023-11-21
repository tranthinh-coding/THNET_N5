using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using N5;
using Org.BouncyCastle.Asn1.Crmf;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;
using OfficeOpenXml;
using System.IO;
using ClosedXML.Excel;

namespace Nhom5_TVThinhNHQHuyPNTanDVDucTNQuynh_LTNet
{
    public partial class QLKhachHang : Form
    {
        QLHHDBDataContext db;
        public QLKhachHang()
        {
            InitializeComponent();
            db = new QLHHDBDataContext();
        }


        private void QLKhachHang_Load(object sender, EventArgs e)
        {
            try
            {
                if (qlkhGv.Columns.Count > 0 )
                {
                    qlkhGv.Columns.Clear();
                }
                var result = db.KhachHangs.Select(p => new
                {
                    p.MaKH,
                    p.TenKH,
                    p.DiaChi,
                    p.Quan,
                    p.ThanhPho
                });
                if (result == null) throw new Exception();
                qlkhGv.DataSource = result;
                qlkhGv.Columns[0].HeaderText = "Mã";
                qlkhGv.Columns[1].HeaderText = "Tên";
                qlkhGv.Columns[2].HeaderText = "Phường";
                qlkhGv.Columns[3].HeaderText = "Quận";
                qlkhGv.Columns[4].HeaderText = "Thành phố";
               
                // Tạo cột nút "Xóa"
                DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
                deleteColumn.HeaderText = "Lựa chọn";
                deleteColumn.Name = "DeleteButton";
                deleteColumn.Text = "Xóa";
                deleteColumn.UseColumnTextForButtonValue = true;

                qlkhGv.Columns.Add(deleteColumn);
            }
            catch
            {
                return;
            }
        }

        void resetError()
        {
            mkhErr.Text = "";
            tkhErr.Text = "";
            dcErr.Text = "";
            quanErr.Text = "";
            thanhphoErr.Text = "";
        }

        private void resetTextBox()
        {
            mkhTb.Text = "";
            mkhTb.Enabled = true;
            tkhTb.Text = "";
            diachiTb.Text = "";
            quanTb.Text = "";
            thanhphoTb.Text = "";
        }

        private bool kiemTra(string field)
        {
            return string.IsNullOrEmpty(field);
            
        }
        private string xuLyKiemTra(string field)
        {

            if (kiemTra(field)) {
                return "Không được để trống!";
            }
            return null;
        }
        private void addBtn_Click(object sender, EventArgs e)
        {
            resetError();
            db = new QLHHDBDataContext();
            try
            {
                if (kiemTra(mkhTb.Text) || kiemTra(tkhTb.Text) || kiemTra(diachiTb.Text) || kiemTra(quanTb.Text) || kiemTra(thanhphoTb.Text))
                {
                    mkhErr.Text = xuLyKiemTra(mkhTb.Text);
                    tkhErr.Text = xuLyKiemTra(tkhTb.Text);
                    dcErr.Text = xuLyKiemTra(diachiTb.Text);
                    quanErr.Text = xuLyKiemTra(quanTb.Text);
                    thanhphoErr.Text = xuLyKiemTra(thanhphoTb.Text);
                    return;
                }

                KhachHang kh = db.KhachHangs.SingleOrDefault(k => k.MaKH == mkhTb.Text);
                bool ktraTonTaiKh = true;
                if (kh == null)
                {
                    ktraTonTaiKh = false;
                    kh = new KhachHang();
                }
                kh.MaKH = mkhTb.Text;
                kh.TenKH = tkhTb.Text;
                kh.DiaChi = diachiTb.Text;
                kh.Quan = quanTb.Text;
                kh.ThanhPho = thanhphoTb.Text;
                if (ktraTonTaiKh)
                {
                    DialogResult result = MessageBox.Show("Mã khách hàng này đã tồn tại, bạn thực sự muốn ghi đè?", "Lưu ý!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        db.SubmitChanges();
                        resetTextBox();
                    }
                }
                else
                {
                    db.KhachHangs.InsertOnSubmit(kh);
                    db.SubmitChanges();
                    resetTextBox();
                }
                QLKhachHang_Load(sender, e);
            }
            catch 
            {
                MessageBox.Show("Thao tác quá nhanh, vui lòng thử lại!");
            }
            
        }
        private void qlkhGv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            resetError();
            try
            {
                // Delete btn
                var kh = db.KhachHangs.First(p => p.MaKH == qlkhGv.Rows[e.RowIndex].Cells["MaKH"].Value.ToString());
                if (qlkhGv.Columns[e.ColumnIndex].Name == "DeleteButton" && e.RowIndex >= 0 && e.RowIndex < qlkhGv.Rows.Count)
                {
                            DialogResult result = MessageBox.Show("Bạn thực sự muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            // Xóa dữ liệu từ Khách hàng từ database
                            if (result == DialogResult.No)
                            {
                                return;
                            }
                            db.KhachHangs.DeleteOnSubmit(kh);
                            db.SubmitChanges();
                    // Xóa dòng tương ứng từ DataGridView
                    //qlkhGv.Rows.RemoveAt(e.RowIndex);
                    QLKhachHang_Load(sender, e);
                    resetTextBox();
                    return;
                }

            }
            catch 
            {
                MessageBox.Show("Thao tác quá nhanh, vui lòng thử lại!", "Chậm lại!");
            }
        }

        private void mkhTb_MouseClick(object sender, MouseEventArgs e)
        {
            mkhErr.Text = "";
        }

        private void tkhTb_MouseClick(object sender, MouseEventArgs e)
        {
            tkhErr.Text = "";
        }

        private void diachiTb_MouseClick(object sender, MouseEventArgs e)
        {
            dcErr.Text = "";
        }

        private void quanTb_MouseClick(object sender, MouseEventArgs e)
        {
            quanErr.Text = "";

        }

        private void thanhphoTb_MouseClick(object sender, MouseEventArgs e)
        {
            thanhphoErr.Text = "";
        }

        private void qlkhGv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = qlkhGv.CurrentRow;
            if (selectedRow != null)
            {
                if (selectedRow.Index >= 0)
                {
                    mkhTb.Text = qlkhGv.Rows[selectedRow.Index].Cells["MaKH"].Value.ToString();
                    mkhTb.Enabled = false;
                    tkhTb.Text = qlkhGv.Rows[selectedRow.Index].Cells["TenKH"].Value.ToString();
                    diachiTb.Text = qlkhGv.Rows[selectedRow.Index].Cells["DiaChi"].Value.ToString();
                    quanTb.Text = qlkhGv.Rows[selectedRow.Index].Cells["Quan"].Value.ToString();
                    thanhphoTb.Text = qlkhGv.Rows[selectedRow.Index].Cells["ThanhPho"].Value.ToString();
                }
            }
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            resetTextBox();
            resetError();
        }

        private void searchTb_TextChanged(object sender, EventArgs e)
        {
            var query = db.KhachHangs.Where(p => p.MaKH.Contains(searchTb.Text) || p.TenKH.Contains(searchTb.Text)).Select(p => new {p.MaKH, p.TenKH, p.DiaChi, p.Quan, p.ThanhPho});
            qlkhGv.DataSource = query;
        }
        //hàm xuất excel
        private void xuatExcel(DataGridView dgv)
        {
            try
            {
               using (XLWorkbook workbook = new XLWorkbook())
                {
                    // Tạo một worksheet mới
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Sheet1");
                    // Lấy số lượng cột và hàng trong GridView
                    int columnsCount = qlkhGv.Columns.Count - 1;
                    int rowsCount = qlkhGv.Rows.Count;
                    // Ghi tiêu đề cột vào worksheet
                    for (int col = 0; col < columnsCount; col++)
                    {
                        worksheet.Cell(1, col + 1).Value = qlkhGv.Columns[col].HeaderText;
                    }
                    // Ghi dữ liệu từ GridView vào worksheet
                    for (int row = 0; row < rowsCount; row++)
                    {
                        for (int col = 0; col < columnsCount; col++)
                        {
                            worksheet.Cell(row + 2, col + 1).Value = qlkhGv.Rows[row].Cells[col].Value.ToString();
                        }
                    }
                    // luu file
                    using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Files|*.xlsx" })
                    {

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            string filePath = sfd.FileName;
                            workbook.SaveAs(filePath);
                            MessageBox.Show("Xuất dữ liệu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }catch (Exception ex)
            {
                MessageBox.Show("Không thể xuất dữ liệu tới excel: " + ex.Message, "Không Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void export_Click(object sender, EventArgs e)
        {
            xuatExcel(qlkhGv);

        }

        private void export_MouseHover_1(object sender, EventArgs e)
        {
            export.ForeColor = ColorTranslator.FromHtml("25, 91, 255");
            export.Font = new Font(export.Font, export.Font.Style | FontStyle.Underline);
        }

        private void export_MouseLeave(object sender, EventArgs e)
        {
            export.ForeColor = Color.DimGray;
            export.Font = new Font(export.Font, export.Font.Style & ~FontStyle.Underline);
        }
    }
    
}
