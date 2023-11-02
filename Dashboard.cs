using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nhom5_TVThinhNHQHuyPNTanDVDucTNQuynh_LTNet
{
    public partial class Dashboard : Form
    {
        Color buttonNormalColor = Color.Transparent;
        Color buttonActiveColor = Color.FromArgb(249, 252, 253);

        QLKhachHang formKhachHang = null;
        QLHangHoa formHangHoa = null;
        QLLoaiHang formLoaiHang = null;
        QLHoaDon formHoaDon = null;

        public Dashboard()
        {
            InitializeComponent();
        }

        // Đăng xuất
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            Store.User = null;

            this.Hide();
            (new DangNhap()).Show();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            //usrName.Text = Store.User.TenNV;
            /*
            QLHHDBDataContext db = new QLHHDBDataContext();

            var res = db.HangHoas.Select(p =>
                new {
                    p.MaHang,
                    p.TenHang,
                    p.LoaiHang,
                    p.DonGia,
                    p.DonViTinh
                });

            DataTable table = new DataTable();
            table.Columns.Add("Mã");
            table.Columns.Add("Tên hàng");
            table.Columns.Add("Loại hàng");
            table.Columns.Add("Đơn giá");
            table.Columns.Add("Đơn vị tính");

            foreach (var item in res)
            {
                table.Rows.Add(item.MaHang, item.TenHang, item.LoaiHang, item.DonGia, item.DonViTinh);
            }
            */
            resetButtonColor();
        }

        private void resetButtonColor()
        {
            qlkhBtn.BackColor = buttonNormalColor;
            hangHoaBtn.BackColor = buttonNormalColor;
            hoaDonBtn.BackColor = buttonNormalColor;
            loaiHangBtn.BackColor = buttonNormalColor;
        }
        private void qlhd_Click(object sender, EventArgs e)
        {
            resetButtonColor();
            qlkhBtn.BackColor = buttonActiveColor;

            if (formHoaDon == null)
            {
                formHoaDon = new QLHoaDon();
                formHoaDon.FormClosed += FormHoaDon_Closed;
                formHoaDon.MdiParent = this;
                formHangHoa.Dock = DockStyle.Fill;
                formHoaDon.Show();
            }
        }
        private void FormKH_Closed(object sender, FormClosedEventArgs e)
        {
            formKhachHang = null;
        }
        private void FormHoaDon_Closed(object sender, FormClosedEventArgs e)
        {
            formHoaDon = null;
        }
        private void FormHangHoa_Closed(object sender, FormClosedEventArgs e)
        {
            formHangHoa = null;
        }
        private void FormLoaiHang_Closed(object sender, FormClosedEventArgs e)
        {
            formLoaiHang = null;
        }

        private void qlkh_Click(object sender, EventArgs e)
        {
            qlkhBtn.BackColor = buttonActiveColor;

            if (formKhachHang == null)
            {
                formKhachHang = new QLKhachHang();
                formKhachHang.FormClosed += FormKH_Closed;
                formKhachHang.MdiParent = this;
                formHangHoa.Dock = DockStyle.Fill;
                formKhachHang.Show();
            }
        }

        private void qlhh_Click(object sender, EventArgs e)
        {
            hangHoaBtn.BackColor = buttonActiveColor;

            if (formHangHoa == null)
            {
                formHangHoa = new QLHangHoa();
                formHangHoa.FormClosed += FormHangHoa_Closed;
                formHangHoa.MdiParent = this;
                formHangHoa.Dock = DockStyle.Fill;
                formHangHoa.Show();
            }
        }

        private void qllh_Click(object sender, EventArgs e)
        {
            loaiHangBtn.BackColor = buttonActiveColor;

            if (formLoaiHang == null)
            {
                formLoaiHang = new QLLoaiHang();
                formLoaiHang.FormClosed += FormLoaiHang_Closed;
                formLoaiHang.MdiParent = this;
                formHangHoa.Dock = DockStyle.Fill;
                formLoaiHang.Show();
            }
        }
    }
}
