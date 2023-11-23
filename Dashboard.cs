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
        ThongKe formThongKe = null;

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
            resetButtonColor();
            thongKeBtn.Visible = Store.IsAdmin();
        }

        private void resetButtonColor()
        {
            qlkhBtn.IdleFillColor = buttonNormalColor;
            hangHoaBtn.IdleFillColor = buttonNormalColor;
            hoaDonBtn.IdleFillColor = buttonNormalColor;
            loaiHangBtn.IdleFillColor = buttonNormalColor;
            thongKeBtn.IdleFillColor = buttonNormalColor;
        }
        private void qlhd_Click(object sender, EventArgs e)
        {
            resetButtonColor();
            currentFormOpen?.Hide();
            hoaDonBtn.IdleFillColor = buttonActiveColor;

            formHoaDon = new QLHoaDon();

            setForm(formHoaDon);

        }

        private void qltk_Click(object sender, EventArgs e)
        {
            resetButtonColor();
            currentFormOpen?.Hide();
            thongKeBtn.IdleFillColor = buttonActiveColor;

            formThongKe = new ThongKe();

            setForm(formThongKe);
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
            //currentFormOpen?.Close();
            resetButtonColor();
            currentFormOpen?.Hide();

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
