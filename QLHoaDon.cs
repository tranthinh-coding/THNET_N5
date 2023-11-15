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

namespace Nhom5_TVThinhNHQHuyPNTanDVDucTNQuynh_LTNet
{
    public partial class QLHoaDon : Form
    {
        QLHHDBDataContext db = new QLHHDBDataContext();
        List<HangHoa> list = new List<HangHoa>();
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

            var result = db.HangHoas.Select(p => new
            {
                p.MaHang,
                p.LoaiHang,
                p.TenHang,

            });
            matHang.DataSource = result.ToList();
            matHang.DisplayMember = "TenHang";
            matHang.ValueMember = "MaHang";
            db.SubmitChanges();

            var khachHang = db.KhachHangs.Select(p => new
            {
                p.MaKH,
                p.TenKH
            });

            comboboxTen.DataSource = khachHang;
            comboboxTen.ValueMember = "MaKH";
            comboboxTen.DisplayMember = "TenKH";
        }

        long tongTienNumber = 0;
        private void btnThemHang_Click(object sender, EventArgs e)
        {
            
            if(btnSoLuong.Value > 0)
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

                    row["TenHang"] = item.TenHang;
                    row["DonViTinh"] = item.DonViTinh;
                    row["DonGia"] = item.DonGia;
                    row["SoLuong"] = btnSoLuong.Value;
                    row["TongTien"] = int.Parse(btnSoLuong.Value.ToString()) * int.Parse(item.DonGia.ToString());
                    tongTienNumber += int.Parse(btnSoLuong.Value.ToString()) * int.Parse(item.DonGia.ToString());
                    table.Rows.Add(row);
                }
                data.DataSource = table;
                btnSoLuong.Value = 0;
                thanhTien.Text = tongTienNumber.ToString("#,##0.00₫");
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

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable tb = new DataTable();
                tb = (DataTable)data.DataSource;

                ThongTinHoaDon hoaDon = new ThongTinHoaDon();
                hoaDon.dataTable = tb;
                hoaDon.tongTienThanhToan = thanhTien.Text;

                HoaDon tbHoaDon = new HoaDon();

                KhachHang kh = new KhachHang();

                CT_HoaDon cT_HoaDon = new CT_HoaDon();

                Random random = new Random();

                int number = random.Next(10000);

                if (number < 10)
                {
                    kh.MaKH = "KH0000" + number;
                    tbHoaDon.SoHD = "HD0000" + number;
                }
                else if (number < 100 && number > 10)
                {
                    kh.MaKH = "KH000" + number;
                    tbHoaDon.SoHD = "HD000" + number;
                }
                else if (number < 1000 && number > 100)
                {
                    kh.MaKH = "KH00" + number;
                    tbHoaDon.SoHD = "HD000" + number;
                }
                else
                {
                    kh.MaKH = "KH" + number;
                    tbHoaDon.SoHD = "HD" + number;
                }

                if (tab.SelectedIndex == 0)
                {
                    hoaDon.tenKhach = comboboxTen.Text.ToString();
                    
                }
                else
                {
                    hoaDon.tenKhach = tenKhach.Text;

                    kh.TenKH = tenKhach.Text;
                    kh.DiaChi = diaChi.Text;
                    kh.Quan = quan.Text;
                    kh.ThanhPho = thanhPho.Text;
                    db.KhachHangs.InsertOnSubmit(kh);
                    db.SubmitChanges();
                }

                hoaDon.maHoaDon = tbHoaDon.SoHD;
                hoaDon.ngay = date.Value;

                MessageBox.Show("Thanh toán thành công!");

                tbHoaDon.NgayBan = date.Value;
                tbHoaDon.MaKH = comboboxTen.SelectedValue.ToString();
                tbHoaDon.MaNV = "NV04";
                db.HoaDons.InsertOnSubmit(tbHoaDon);
                db.SubmitChanges();

                var result = db.HangHoas.Where(p => p.MaHang == matHang.SelectedValue.ToString()).Where(p => p.MaHang == cT_HoaDon.MaHang).Select(p => new
                {
                    p.DonGia
                });

                cT_HoaDon.SoHD = tbHoaDon.SoHD;
                cT_HoaDon.MaHang = matHang.SelectedValue.ToString();
                cT_HoaDon.SoLuong = int.Parse(btnSoLuong.Value.ToString());
                cT_HoaDon.DonGia = cT_HoaDon.SoLuong * int.Parse(result.ToString());

                hoaDon.ShowDialog();
                this.Close();
                
            }
            catch
            {
                MessageBox.Show("Thanh toán thất bại vui lòng kiểm tra lại!");
            }
        }

        private void QLHoaDon_Load(object sender, EventArgs e)
        {

        }
    }
}
