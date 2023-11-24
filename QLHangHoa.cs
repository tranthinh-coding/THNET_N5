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
            db.Connection.ConnectionString = @"Data Source=Admin-PC;Initial Catalog=QLBanHangTapHoa;Integrated Security=True";

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
            txtMahang.Enabled = false;
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
            txtMahang.Enabled = true;
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
            HangHoa hanghoa = new HangHoa();
            hanghoa.MaHang = txtMahang.Text;
            hanghoa.TenHang = txtTenHang.Text;
            hanghoa.LoaiHang = cbBLoaiHang.SelectedValue.ToString();
            hanghoa.DonViTinh = txtDVT.Text;
            hanghoa.DonGia = float.Parse(txtDonGia.Text);
            if (txtMahang.Text != "" && txtTenHang.Text != "" && txtDVT.Text != "" && txtDonGia.Text != "")
            {
                if (IsNumeric(txtDonGia.Text) == false)
                {
                    MessageBox.Show("Vui lòng ko nhập chữ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (db.HangHoas.Any(hh => hh.MaHang == hanghoa.MaHang))
                    {
                        MessageBox.Show("Thêm thất bại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        db.HangHoas.InsertOnSubmit(hanghoa);
                        db.SubmitChanges();
                        Render();

                        txtMahang.Clear();
                        txtTenHang.Clear();
                        txtDVT.Clear();
                        txtDonGia.Clear();
                        txtMahang.Focus();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng không để trống", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            int select = dtGridViewHangHoa.SelectedCells[0].RowIndex;

            HangHoa hanghoa = db.HangHoas.Single(hh => hh.MaHang == dtGridViewHangHoa.Rows[select].Cells["MaHang"].Value.ToString());

            if (hanghoa.MaHang != txtMahang.Text)
            {
                MessageBox.Show("Không thể thay đổi mã hàng.");
            }

            hanghoa.TenHang = txtTenHang.Text;
      
            hanghoa.LoaiHang = cbBLoaiHang.SelectedValue.ToString();
      
            hanghoa.DonViTinh = txtDVT.Text;
            hanghoa.DonGia = float.Parse(txtDonGia.Text);
                
            if (txtMahang.Text != "" && txtTenHang.Text != "" && txtDVT.Text != "" && txtDonGia.Text != "")
            {
                if (IsNumeric(txtDonGia.Text) == false)
                {
                    MessageBox.Show("Vui lòng ko nhập chữ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    db.SubmitChanges();
                    Render();
                    txtMahang.Clear();
                    txtTenHang.Clear();
                    txtDVT.Clear();
                    txtDonGia.Clear();
                }
            }
            else
            {
                MessageBox.Show("Vui không để trống", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int select = dtGridViewHangHoa.SelectedCells[0].RowIndex;
            string MaHang = dtGridViewHangHoa.Rows[select].Cells["MaHang"].Value.ToString();

            HangHoa hanghoa = db.HangHoas.Single(hh => hh.MaHang == MaHang);

            // Xoa bo toan bo hoa don lien quan den mat hang
            //var s = db.CT_HoaDons.Where(xxe => xxe.MaHang == MaHang);
            //db.CT_HoaDons.DeleteAllOnSubmit(s);

            db.HangHoas.DeleteOnSubmit(hanghoa);
            db.SubmitChanges();

            Render();

            txtMahang.Clear();
            txtTenHang.Clear();
            txtDVT.Clear();
            txtDonGia.Clear();
            txtMahang.Focus();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var search = db.HangHoas.Where(hh => hh.MaHang.Contains(txtSearch.Text) || hh.TenHang.Contains(txtSearch.Text)).Select(hh => new { hh.MaHang, hh.TenHang, hh.LoaiHang, hh.DonViTinh, hh.DonGia });
            dtGridViewHangHoa.DataSource = search;
            txtMahang.Clear();
            txtTenHang.Clear();
            txtDVT.Clear();
            txtDonGia.Clear();
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
                //exSheet.Range["A" + (dong + i).ToString()].Value = dtViewLoaiHang.Rows[i].Cells["MaLoai"].Value.ToString();
                //exSheet.Range["B"+ (dong + i).ToString() +":C" +(dong + i).ToString()].MergeCells = true;
                //exSheet.Range["B" + (dong + i).ToString()].Value = dtViewLoaiHang.Rows[i].Cells["TenLoai"].Value.ToString();
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

            txtMahang.Text = dtGridViewHangHoa.Rows[select].Cells["MaHang"].Value.ToString();
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
            txtMahang.Clear();
            txtTenHang.Clear();
            txtDVT.Clear();
            txtDonGia.Clear();
            txtMahang.Enabled = true;
            txtTenHang.Enabled = true;
            txtDVT.Enabled = true;
            txtDonGia.Enabled = true;
            txtMahang.Focus();
        }
    }
}
