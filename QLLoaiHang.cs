using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Nhom5_TVThinhNHQHuyPNTanDVDucTNQuynh_LTNet
{
    public partial class QLLoaiHang : Form
    {
        QLHHDBDataContext db;
        int CurrRowSelect = -1;
      

        public QLLoaiHang()
        {
            InitializeComponent();
            db = new QLHHDBDataContext();
            db.Connection.ConnectionString = @"Data Source=Admin-PC;Initial Catalog=QLBanHangTapHoa;Integrated Security=True";
        }

        public void Render()
        {
            var render = db.LoaiHangs.Select(lh => new { lh.MaLoai, lh.TenLoai });
            dtViewLoaiHang.DataSource = render;

            txtMaLoai.Clear();
            txtLoaiHang.Clear();
            txtMaLoai.Enabled = true;
        }

        private void QLLoaiHang_Load(object sender, EventArgs e)
        {
            CurrRowSelect = -1;
            txtMaLoai.Clear();
            txtLoaiHang.Clear();
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            txtMaLoai.Enabled = false;
            txtLoaiHang.Enabled = false;
            txtSearch.Enabled = false;
            dtViewLoaiHang.Enabled = false;
        }

        private void btnRender_Click(object sender, EventArgs e)
        {
            Render();
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            txtMaLoai.Enabled = true;
            txtLoaiHang.Enabled = true;
            txtSearch.Enabled = true;
            txtMaLoai.Clear();
            txtLoaiHang.Clear();
            btnRender.Enabled = false;
            dtViewLoaiHang.Enabled = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtMaLoai.Clear();
            txtLoaiHang.Clear();
            txtMaLoai.Enabled = true;
            txtLoaiHang.Enabled = true;
            txtMaLoai.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtMaLoai.Enabled = true;
            LoaiHang loaihang = new LoaiHang();
            loaihang.MaLoai = txtMaLoai.Text;
            loaihang.TenLoai = txtLoaiHang.Text;

            if (txtMaLoai.Text != "" && txtLoaiHang.Text != "")
            {
                if (db.LoaiHangs.Any(lhs => lhs.MaLoai == loaihang.MaLoai))
                {
                    MessageBox.Show("Thêm thất bại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Thêm vào bảng
                    db.LoaiHangs.InsertOnSubmit(loaihang);
                    // Thêm vào database
                    db.SubmitChanges();
                    Render();

                    txtMaLoai.Clear();
                    txtLoaiHang.Clear();
                    txtMaLoai.Focus();
                }
            }
            else
            {
                MessageBox.Show("Vui không để trống", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            txtMaLoai.Clear();
            txtLoaiHang.Clear();

            // Kiem tra loai hang co ton tai hay khong
            try
            {
                if (CurrRowSelect >= 0)
                {
                    LoaiHang loaihang = db.LoaiHangs.Single(lh => lh.MaLoai == dtViewLoaiHang.Rows[CurrRowSelect].Cells["MaLoai"].Value.ToString());

                    if (loaihang.HangHoas.Count > 0)
                    {
                        DialogResult x = MessageBox.Show("Loại hàng này có chứa liên kết đến một vài hàng hóa.", "Xác nhận xóa", MessageBoxButtons.OK);


                        if (x == DialogResult.OK)
                        {
                            loaihang.HangHoas.Clear();
                        } else
                        {
                            return;
                        }
                    }

                    db.LoaiHangs.DeleteOnSubmit(loaihang);
                    db.SubmitChanges();
                    Render();
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("Vui lòng select để xóa", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } catch (Exception err)
            {
                // Thong bao co san pham dang lien ket voi loaihang nay. Xac nhan xoa hay khong
                MessageBox.Show("Không thể xoá loại hàng này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (CurrRowSelect >= 0)
            {
                LoaiHang loaihang = db.LoaiHangs.Single(lh => lh.MaLoai == dtViewLoaiHang.Rows[CurrRowSelect].Cells["MaLoai"].Value.ToString());
                loaihang.MaLoai = txtMaLoai.Text;
                loaihang.TenLoai = txtLoaiHang.Text;
                if (txtMaLoai.Text != "" && txtLoaiHang.Text!= "") {
                    MessageBox.Show("Edit thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    db.SubmitChanges();
                    Render();
                    txtMaLoai.Clear();
                    txtLoaiHang.Clear();
                } else
                {
                    MessageBox.Show("Vui không để trống", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var search = db.LoaiHangs.Where(lh => lh.MaLoai.Contains(txtSearch.Text) || lh.TenLoai.Contains(txtSearch.Text)).Select(lh => new { lh.MaLoai, lh.TenLoai });
            dtViewLoaiHang.DataSource = search;
            txtMaLoai.Clear();
            txtLoaiHang.Clear();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Đối tượng Application chỉ ứng dụng excel
            Excel.Application exApp = new Excel.Application();
            // chỉ file excel
            Excel.Workbook exBook = exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            // chi trang tính trong file excel
            Excel.Worksheet exSheet = (Excel.Worksheet)exBook.Worksheets[1];
            // Chỉ 1 ô trong file excel
            Excel.Range tenTruong = (Excel.Range)exSheet.Cells[1,1];

            // Tiêu đề
            exSheet.Range["A2:D2"].Font.Size = 18;
            exSheet.Range["A2:D2"].MergeCells = true;
            exSheet.Range["A2:D2"].Value = "DANH SÁCH LOẠI HÀNG";
            exSheet.Range["A2:D2"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            exSheet.Range["A2:D2"].Font.Bold = true;

            // Bảng danh sách hóa đơn
            exSheet.Range["A4:D4"].Font.Size = 14;
            exSheet.Range["A4:D4"].Font.Bold = true;
            exSheet.Range["A2:D2"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            exSheet.Range["A4"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            exSheet.Range["A4"].Value = "Mã Loại";
            exSheet.Range["B4:C4"].MergeCells = true;
            exSheet.Range["B4:C4"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            exSheet.Range["B4"].Value = "Tên Loại";

            // Các sản phẩm
            int dong = 5;
            for(int i = 0; i < dtViewLoaiHang.Rows.Count; i++)
            {
                exSheet.Range["A" + (dong + i).ToString()].Value = dtViewLoaiHang.Rows[i].Cells["MaLoai"].Value.ToString();
                exSheet.Range["B"+ (dong + i).ToString() +":C" +(dong + i).ToString()].MergeCells = true;
                exSheet.Range["B" + (dong + i).ToString()].Value = dtViewLoaiHang.Rows[i].Cells["TenLoai"].Value.ToString();
            }
            // Đặt tên cho file excel
            exSheet.Name = "QLLoaiHang";
            // File excel hoạt động
            exSheet.Activate();
            // Đối tượng lưu
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel Files|*.xlsx;*.xls";
            if (save.ShowDialog() == DialogResult.OK )
            {
                exBook.SaveAs(save.FileName);
                exBook.Close();
                exApp.Quit();

                MessageBox.Show("Tập tin Excel đã được lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                exBook.Close();
                exApp.Quit();

                MessageBox.Show("Lưu tập tin Excel đã bị hủy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void dtViewLoaiHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaLoai.Enabled = false;
            CurrRowSelect = dtViewLoaiHang.SelectedCells[0].RowIndex;
            txtMaLoai.Clear();
            txtLoaiHang.Clear();
            if (CurrRowSelect >= 0)
            {
                txtMaLoai.Text = dtViewLoaiHang.Rows[CurrRowSelect].Cells["MaLoai"].Value.ToString();
                txtLoaiHang.Text = dtViewLoaiHang.Rows[CurrRowSelect].Cells["TenLoai"].Value.ToString();
            }
        }
    }
}
