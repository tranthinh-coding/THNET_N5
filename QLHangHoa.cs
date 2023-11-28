using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Nhom5_TVThinhNHQHuyPNTanDVDucTNQuynh_LTNet
{
    public partial class QLHangHoa : Form
    {
        QLHHDBDataContext db;
        DataTableCollection tableCollection;
        List<LoaiHang> listLoaiHang;
        
        public QLHangHoa()
        {
            InitializeComponent();
            db = new QLHHDBDataContext();

            var query = from x in db.LoaiHangs
                        select new
                        {
                            MaLoai = x.MaLoai,
                            TenLoai = x.TenLoai
                        };

            listLoaiHang = query.AsEnumerable()
                                   .Select(x => new LoaiHang
                                   {
                                       MaLoai = x.MaLoai,
                                       TenLoai = x.TenLoai
                                   })
                                   .ToList();

            cbBLoaiHang.ValueMember = "MaLoai";
            cbBLoaiHang.DisplayMember = "TenLoai";
            cbBLoaiHang.DataSource = listLoaiHang;
        }
        private void QLHangHoa_Load(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            txtTenHang.Enabled = false;
            cbBLoaiHang.Enabled = false;
            txtDVT.Enabled = false;
            txtDonGia.Enabled = false;
            txtSearch.Enabled = false;
            dtGridViewHangHoa.Enabled = false;
        }

        public void Render()
        {
            var render = from p in db.HangHoas
                         join c in db.LoaiHangs
                         on p.LoaiHang equals c.MaLoai
                         select new
                         {
                             p.MaHang,
                             p.TenHang,
                             c.TenLoai,
                             p.DonViTinh,
                             p.DonGia
                         };

            dtGridViewHangHoa.DataSource = render;
        }

        private void btnRender_Click(object sender, EventArgs e)
        {
            Render();
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            txtTenHang.Enabled = true;
            cbBLoaiHang.Enabled = true;
            txtDVT.Enabled = true;
            txtDonGia.Enabled = true;
            txtSearch.Enabled = true;
            dtGridViewHangHoa.Enabled = true;
        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (
                IsNil(txtDonGia.Text) ||
                IsNil(txtDVT.Text) ||
                IsNil(txtTenHang.Text)
            )
            {
                MessageBox.Show("Vui lòng điền đầy đủ các trường");
                return;
            }

            if (!IsNumeric(txtDonGia.Text))
            {
                MessageBox.Show("Vui lòng ko nhập chữ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HangHoa hanghoa = new HangHoa();
            hanghoa.MaHang = Utils.RandomID("HH");
            hanghoa.TenHang = txtTenHang.Text;
            hanghoa.LoaiHang = cbBLoaiHang.SelectedValue.ToString();
            hanghoa.DonViTinh = txtDVT.Text;
            hanghoa.DonGia = float.Parse(txtDonGia.Text);
           
            if (db.HangHoas.Any(hh => hh.MaHang == hanghoa.MaHang))
            {
                MessageBox.Show("Mã hàng này đã tồn tại.");
            }
            else
            {
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                db.HangHoas.InsertOnSubmit(hanghoa);
                db.SubmitChanges();
                Render();

                txtTenHang.Clear();
                txtDVT.Clear();
                txtDonGia.Clear();
                txtTenHang.Focus();
            }
        }

        // null or empty
        private bool IsNil(string x)
        {
            return String.IsNullOrEmpty(x);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (
                IsNil(txtDonGia.Text) ||
                IsNil(txtDVT.Text) || 
                IsNil(txtTenHang.Text)
            ) {
                MessageBox.Show("Vui lòng điền đầy đủ các trường");
                return;
            }
            
            if (!IsNumeric(txtDonGia.Text))
            {
                MessageBox.Show("Vui lòng ko nhập chữ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int select = dtGridViewHangHoa.SelectedCells[0].RowIndex;
            string MaHang = dtGridViewHangHoa.Rows[select].Cells["MaHang"].Value.ToString();
            HangHoa hanghoa = db.HangHoas.Single(hh => hh.MaHang == MaHang);

            if (hanghoa.MaHang != MaHang)
            {
                MessageBox.Show("Không thể thay đổi mã hàng.");
                return;
            }

            hanghoa.TenHang = txtTenHang.Text;
      
            hanghoa.LoaiHang = cbBLoaiHang.SelectedValue.ToString();
      
            hanghoa.DonViTinh = txtDVT.Text;
            hanghoa.DonGia = float.Parse(txtDonGia.Text);

            db.SubmitChanges();
            Render();
            txtMaHang.Text = "";
            txtTenHang.Clear();
            txtDVT.Clear();
            txtDonGia.Clear();
            txtTenHang.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int select = dtGridViewHangHoa.SelectedCells[0].RowIndex;
            string MaHang = dtGridViewHangHoa.Rows[select].Cells["MaHang"].Value.ToString();

            HangHoa hanghoa = db.HangHoas.Single(hh => hh.MaHang == MaHang);

            db.HangHoas.DeleteOnSubmit(hanghoa);
            db.SubmitChanges();

            Render();
            txtMaHang.Text = "";
            txtTenHang.Clear();
            txtDVT.Clear();
            txtDonGia.Clear();
            txtTenHang.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var search = db.HangHoas.Where(hh => hh.MaHang.Contains(txtSearch.Text) || hh.TenHang.Contains(txtSearch.Text)).Select(hh => new { hh.MaHang, hh.TenHang, hh.LoaiHang, hh.DonViTinh, hh.DonGia });
            dtGridViewHangHoa.DataSource = search;
            txtMaHang.Text = "";
            txtTenHang.Clear();
            txtDVT.Clear();
            txtDonGia.Clear();
            txtTenHang.Focus();
        }

        private void exportExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Đối tượng Application chỉ ứng dụng excel
            Excel.Application exApp = new Excel.Application();
            // chỉ file excel
            Excel.Workbook exBook = exApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            // chi trang tính trong file excel
            Excel.Worksheet exSheet = (Excel.Worksheet)exBook.Worksheets[1];
            // Chỉ 1 ô trong file excel
            Excel.Range tenTruong = (Excel.Range)exSheet.Cells[1, 1];

            // Tiêu đề
            exSheet.Range["B2:G2"].Font.Size = 24;
            exSheet.Range["B2:G2"].MergeCells = true;
            exSheet.Range["B2:G2"].Value = "DANH SÁCH HÀNG HÓA";
            exSheet.Range["B2:G2"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            exSheet.Range["B2:G2"].Font.Bold = true;

            // Bảng danh sách hóa đơn
            exSheet.Range["A4:H4"].Font.Size = 14;
            exSheet.Range["A4:H4"].Font.Bold = true;
            exSheet.Range["A4"].Value = "Mã Hàng";
            exSheet.Range["B4:C4"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            exSheet.Range["B4:C4"].MergeCells = true;
            exSheet.Range["B4"].Value = "Tên Hàng";
            exSheet.Range["D4:E4"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            exSheet.Range["D4:E4"].MergeCells = true;
            exSheet.Range["D4"].Value = "Tên Loại";
            exSheet.Range["F4:G4"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            exSheet.Range["F4:G4"].MergeCells = true;
            exSheet.Range["F4"].Value = "Đơn Vị Tính";
            exSheet.Range["H4"].Value = "Đơn Giá";

            int dong = 5;
            for (int i = 0; i < dtGridViewHangHoa.Rows.Count; i++)
            {
                exSheet.Range["A" + (dong + i).ToString()].Value = dtGridViewHangHoa.Rows[i].Cells["MaHang"].Value.ToString();
                exSheet.Range["B" + (dong + i).ToString() + ":C" + (dong + i).ToString()].MergeCells = true;
                exSheet.Range["B" + (dong + i).ToString()].Value = dtGridViewHangHoa.Rows[i].Cells["TenHang"].Value.ToString();
                exSheet.Range["D" + (dong + i).ToString() + ":E" + (dong + i).ToString()].MergeCells = true;
                exSheet.Range["D" + (dong + i).ToString()].Value = dtGridViewHangHoa.Rows[i].Cells["TenLoai"].Value.ToString();
                exSheet.Range["F" + (dong + i).ToString() + ":G" + (dong + i).ToString()].MergeCells = true;
                exSheet.Range["F" + (dong + i).ToString()].Value = dtGridViewHangHoa.Rows[i].Cells["DonViTinh"].Value.ToString();
                exSheet.Range["H" + (dong + i).ToString()].Value = dtGridViewHangHoa.Rows[i].Cells["DonGia"].Value.ToString();
            }

            exSheet.Name = "QLHangHoa";
            // File excel hoạt động
            exSheet.Activate();
            // Đối tượng lưu
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel Files|*.xlsx;*.xls";
            if (save.ShowDialog() == DialogResult.OK)
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

        private void importExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(OpenFileDialog openFileDialog = new OpenFileDialog() {})
            {
                if(openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using(var str = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using(IExcelDataReader reader = ExcelReaderFactory.CreateReader(str))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true}
                            });
                            tableCollection = result.Tables;
                            foreach(DataTable table in tableCollection)
                            {
                                comboBox1.Items.Add(table.TableName);
                            }
                        }
                    }
                }
            }
        }

        private void dtGridViewHangHoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int select = dtGridViewHangHoa.SelectedCells[0].RowIndex;

            txtMaHang.Text = dtGridViewHangHoa.Rows[select].Cells["MaHang"].Value.ToString();
            txtTenHang.Text = dtGridViewHangHoa.Rows[select].Cells["TenHang"].Value.ToString();
            
            string TenLoai = dtGridViewHangHoa.Rows[select].Cells["TenLoai"].Value.ToString();
            int indexOfLoaiHang = listLoaiHang.FindIndex(x => x.TenLoai == TenLoai);

            cbBLoaiHang.SelectedValue = listLoaiHang.ElementAt(indexOfLoaiHang).MaLoai;
            txtDVT.Text = dtGridViewHangHoa.Rows[select].Cells["DonViTinh"].Value.ToString();
            txtDonGia.Text = dtGridViewHangHoa.Rows[select].Cells["DonGia"].Value.ToString();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = tableCollection[comboBox1.SelectedItem.ToString()];
            dtGridViewHangHoa.DataSource = dt;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtMaHang.Text = "";
            txtTenHang.Clear();
            txtDVT.Clear();
            txtDonGia.Clear();
            txtTenHang.Enabled = true;
            txtDVT.Enabled = true;
            txtDonGia.Enabled = true;
            txtTenHang.Focus();
        }
    }
}
