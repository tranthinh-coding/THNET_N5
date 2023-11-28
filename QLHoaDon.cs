using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Linq;
using DocumentFormat.OpenXml.Office2010.PowerPoint;

namespace Nhom5_TVThinhNHQHuyPNTanDVDucTNQuynh_LTNet
{
    public partial class QLHoaDon : Form
    {
        QLHHDBDataContext db = new QLHHDBDataContext();
        DataTable table = new DataTable();
        public QLHoaDon()
        {
            InitializeComponent();
            date.Value = DateTime.Now;
            date.Enabled = false;
            table.Columns.Add("TenHang", typeof(string));
            table.Columns.Add("DonViTinh", typeof(string));
            table.Columns.Add("DonGia", typeof(long));
            table.Columns.Add("SoLuong", typeof(int));
            table.Columns.Add("TongTien", typeof(long));

            LamMoiDanhSachHangHoa();
            matHang.DisplayMember = "TenHang";
            matHang.ValueMember = "MaHang";

            LamMoiDanhSachKhachHang();
            comboboxTen.ValueMember = "MaKH";
            comboboxTen.DisplayMember = "TenKH";
        }

        long tongTienNumber = 0;
        private void btnThemHang_Click(object sender, EventArgs e)
        {
            table.PrimaryKey = new DataColumn[] { table.Columns["TenHang"] };
            if (btnSoLuong.Value > 0)
            {
                var result = db.HangHoas.Where(p => p.MaHang == matHang.SelectedValue.ToString()).Select(p => new
                {
                    p.TenHang,
                    p.DonViTinh,
                    p.DonGia
                });
                foreach (var item in result)
                {
                    DataRow row = table.NewRow();

                    var existingRow = table.Rows.Find(item.TenHang);
                    if(existingRow != null)
                    {
                        existingRow["SoLuong"] = int.Parse(existingRow["SoLuong"].ToString()) + int.Parse(btnSoLuong.Value.ToString());

                        existingRow["TongTien"] = int.Parse(existingRow["SoLuong"].ToString()) * int.Parse(existingRow["DonGia"].ToString());
                        tongTienNumber += int.Parse(btnSoLuong.Value.ToString()) * int.Parse(item.DonGia.ToString());
                    }
                    else
                    {
                        row["TenHang"] = item.TenHang;
                        row["DonViTinh"] = item.DonViTinh;
                        row["DonGia"] = item.DonGia;
                        row["SoLuong"] = btnSoLuong.Value;
                        row["TongTien"] = int.Parse(btnSoLuong.Value.ToString()) * int.Parse(item.DonGia.ToString());
                        tongTienNumber += int.Parse(btnSoLuong.Value.ToString()) * int.Parse(item.DonGia.ToString());
                        table.Rows.Add(row);
                    }
                }
                data.DataSource = table;
                btnSoLuong.Value = 0;
                thanhTien.Text = tongTienNumber.ToString("#,##0₫");
            }
            else
            {
                MessageBox.Show("Yêu cầu số lượng tối thiểu 1");
            }
            
        }

        private void tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tab.SelectedIndex == 0)
            {
                tab.Height = 120;
                groupBox2.Height = 250;
            }
            else
            {
                tab.Height = 370;
                groupBox2.Height = 490;
            }
        }

        private string RandomID(string prefix = "")
        {
            Random random = new Random();

            int number = random.Next(10000);

            if (number < 10)
            {
                return prefix + "0000" + number;
            }
            if (number < 100)
            {
                return prefix + "000" + number;
            }
            if (number < 1000)
            {
                return prefix + "00" + number;
            }
            return prefix + number;
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (table.Rows.Count < 1)
                {
                    MessageBox.Show("Chưa chọn sản phẩm");
                    return;
                }

                DataTable tb = new DataTable();
                tb = (DataTable)data.DataSource;

                HoaDon hoaDon = new HoaDon();
                KhachHang kh = new KhachHang();

                if (tab.SelectedTab == tabPage1)
                {
                    kh.MaKH = comboboxTen.SelectedValue.ToString();
                    kh.TenKH = comboboxTen.Text;
                } else
                {
                    // Tao khach hang moi khi chon tabPage2
                    string MaKH = RandomID("KH");
                    kh.MaKH = MaKH;
                    kh.TenKH = tenKhach.Text;
                    kh.DiaChi = diaChi.Text;
                    kh.Quan = quan.Text;
                    kh.ThanhPho = thanhPho.Text;
                    db.KhachHangs.InsertOnSubmit(kh);
                    db.SubmitChanges();
                    LamMoiDanhSachKhachHang();
                }

                hoaDon.SoHD = RandomID("HD");
                hoaDon.NgayBan = date.Value;
                hoaDon.MaKH = kh.MaKH;
                hoaDon.MaNV = Store.User.MaNV;

                db.HoaDons.InsertOnSubmit(hoaDon);
                db.SubmitChanges();

                foreach (DataRow row in table.Rows)
                {
                    CT_HoaDon cT_HoaDon = new CT_HoaDon();
                    int soLuong = int.Parse(row[3].ToString());
                    int donGia = int.Parse(row[4].ToString());
                    String tenHang = row[0].ToString();
                    var rs = db.HangHoas.Where(h => h.TenHang == tenHang).Select(p => p.MaHang).ToList();
                    if (rs.Any())
                    {
                        cT_HoaDon.MaHang = rs[0];
                    }
                    cT_HoaDon.SoHD = hoaDon.SoHD;
                    cT_HoaDon.SoLuong = soLuong;
                    cT_HoaDon.DonGia = donGia;

                    db.CT_HoaDons.InsertOnSubmit(cT_HoaDon);
                }
                db.SubmitChanges();
                MessageBox.Show("Thanh toán thành công!");

                ThongTinHoaDon formHoaDon = new ThongTinHoaDon();
                formHoaDon.tenKhach = kh.TenKH;
                formHoaDon.maHoaDon = hoaDon.SoHD;
                formHoaDon.ngay = date.Value;
                formHoaDon.dataTable = tb;
                formHoaDon.tongTienThanhToan = thanhTien.Text;
                formHoaDon.ShowDialog();

                table.Rows.Clear();
                btnSoLuong.Value = 0;
                thanhTien.Text = "0";
            }
            catch (Exception err)
            {
                MessageBox.Show("Thanh toán thất bại vui lòng kiểm tra lại!" + err.Message);
            }
        }

        private void QLHoaDon_Load(object sender, EventArgs e)
        {

        }

        private void LamMoiDanhSachKhachHang()
        {
            var khachHang = db.KhachHangs.Select(p => new
            {
                p.MaKH,
                p.TenKH
            });

            comboboxTen.DataSource = khachHang;
        }

        // làm mới dữ liệu hàng hoá
        private void LamMoiDanhSachHangHoa()
        {
            var result = db.HangHoas.Select(p => new
            {
                p.MaHang,
                p.LoaiHang,
                p.TenHang,
            });
            matHang.DataSource = result.ToList();
        }
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            LamMoiDanhSachHangHoa();
        }
    }
}
