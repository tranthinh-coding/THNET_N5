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


        private void LoadData()
        {
            try
            {
                if (qlkhGv.Columns.Count > 0)
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

        private void QLKhachHang_Load(object sender, EventArgs e)
        {
            LoadData();
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
                LoadData();
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
                    LoadData();
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

        private void hoverLabel(Label label)
        {
            label.ForeColor = ColorTranslator.FromHtml("25, 91, 255");
            label.Font = new Font(export.Font, export.Font.Style | FontStyle.Underline);
            label.Cursor = Cursors.Hand;
        }
        private void leaveLabel(Label label)
        {
            label.ForeColor = Color.DimGray;
            label.Font = new Font(export.Font, export.Font.Style & ~FontStyle.Underline);
        }
        private void export_MouseHover_1(object sender, EventArgs e)
        {
            hoverLabel(export);
        }

        private void export_MouseLeave(object sender, EventArgs e)
        {
            leaveLabel(export);
        }

        private void import_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xlsx;*.xls";
            // Hiển thị hộp thoại mở tệp
            openFileDialog.Title = "Chọn file nhập dữ liệu";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    using (XLWorkbook workbook = new XLWorkbook(filePath))
                    {
                        IXLWorksheet worksheet = workbook.Worksheet(1);
                        DataTable dt = new DataTable();
                        bool firstRow = true;
                        foreach (IXLRow row in worksheet.Rows())
                        {
                            //Kiểm tra nếu đã có hàng đầu tiên thì thêm luôn cột
                            if (firstRow)
                            {
                                //Thêm cột vào datatable
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Columns.Add(cell.Value.ToString());
                                }
                                firstRow = false;
                            }
                            else
                            {
                                //Nếu chưa có hàng nào thì tạo 
                                DataRow newRow = dt.NewRow();
                                foreach (IXLCell cell in row.Cells())
                                {
                                    newRow[cell.Address.ColumnNumber - 1] = cell.Value.ToString();
                                }
                                dt.Rows.Add(newRow);
                            }
                        }
                       // qlkhGv.DataSource = dt;
                        using (QLHHDBDataContext db = new QLHHDBDataContext())
                        {
                            try
                            {
                                // for -> insert -> lỗi -> bỏ qua, insert tiếp
                                var dataRows = dt.AsEnumerable();
                                List<string> mkhtontai = new List<string>();
                                foreach (var item in dataRows)
                                {
                                    var khtontai = db.KhachHangs.Select(k => k.MaKH).ToList();
                                    if (khtontai.Contains(item[0].ToString()))
                                    {
                                        mkhtontai.Add(item[0].ToString());
                                    }
                                    else
                                    {
                                        KhachHang x = new KhachHang();
                                        x.MaKH = item[0].ToString();
                                        x.TenKH = item[1].ToString();
                                        x.DiaChi = item[2].ToString();
                                        x.Quan = item[3].ToString();
                                        x.ThanhPho = item[4].ToString();
                                        db.KhachHangs.InsertOnSubmit(x);
                                    } 
                                }
                                db.SubmitChanges();
                                LoadData();
                                MessageBox.Show("Nhập dữ liệu thành công!", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (mkhtontai.Count > 0)
                                    MessageBox.Show("Những mã khách hàng sau đã tồn tại nên không thể nhập từ file: " + string.Join(",", mkhtontai), "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            catch (Exception err)
                            {
                                // insert thất bại, gridView bị lỗi hiển thị
                                // load lai table trước khi hiện thông báo
                                LoadData(); 
                                if (err.Message.Contains("The duplicate key value is"))
                                {
                                    string[] parts = err.Message.Split('.');

                                    MessageBox.Show(parts[3]);
                                }
                            }

                        }
                    }
                }catch(SqlException sqlex)
                {
                    MessageBox.Show(sqlex.ToString());
                }
            }
        }

        private void import_MouseHover(object sender, EventArgs e)
        {
            hoverLabel(import);
        }

        private void import_MouseLeave(object sender, EventArgs e)
        {
            leaveLabel(import);
        }
    }
    
}
