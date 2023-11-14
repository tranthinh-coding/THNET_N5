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
        Color buttonActiveColor = Color.FromArgb(244, 247, 248);

        QLKhachHang formKhachHang = null;
        QLHangHoa formHangHoa = null;
        QLLoaiHang formLoaiHang = null;
        QLHoaDon formHoaDon = null;

        Form currentFormOpen = null;

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
            qlkhBtn.IdleFillColor = buttonNormalColor;
            hangHoaBtn.IdleFillColor = buttonNormalColor;
            hoaDonBtn.IdleFillColor = buttonNormalColor;
            loaiHangBtn.IdleFillColor = buttonNormalColor;
        }
        private void qlhd_Click(object sender, EventArgs e)
        {
            resetButtonColor();
            currentFormOpen?.Hide();
            hoaDonBtn.IdleFillColor = buttonActiveColor;

            formHoaDon = new QLHoaDon();

            setForm(formHoaDon);

        }

        private void qlkh_Click(object sender, EventArgs e)
        {
            resetButtonColor();
            currentFormOpen?.Hide();

            qlkhBtn.IdleFillColor = buttonActiveColor;

            formKhachHang = new QLKhachHang();

            setForm(formKhachHang);
        }

        private void qlhh_Click(object sender, EventArgs e)
        {
            resetButtonColor();
            currentFormOpen?.Hide();

            hangHoaBtn.IdleFillColor = buttonActiveColor;

            formHangHoa = new QLHangHoa();

            setForm(formHangHoa);
        }

        private void qllh_Click(object sender, EventArgs e)
        {
            currentFormOpen?.Close();
            loaiHangBtn.IdleFillColor = buttonActiveColor;

            formLoaiHang = new QLLoaiHang();

            setForm(formLoaiHang);
        }

        void setForm(Form form)
        {
            resetButtonColor();
            currentFormOpen = form;
            currentFormOpen.TopLevel = false;
            currentFormOpen.MdiParent = this;
            currentFormOpen.Parent = mainPanel;
            currentFormOpen.Dock = DockStyle.Fill;

            currentFormOpen.Show();
        }
    }
}
