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
        QLDoiMatKhau formMatKhau = null;
        ThongTinNhom formThongTinNhom = null;

        Form currentFormOpen = null;

        public Dashboard()
        {
            InitializeComponent();
        }

        // Đăng xuất
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            Store.User = null;

            this.Close();
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
            qlmk.IdleFillColor = buttonNormalColor;
        }
        private void qlhd_Click(object sender, EventArgs e)
        {
            if (formHoaDon == null)
                formHoaDon = new QLHoaDon();
            if (currentFormOpen == formHoaDon) return;

            resetButtonColor();
            currentFormOpen?.Hide();
            hoaDonBtn.IdleFillColor = buttonActiveColor;

            setForm(formHoaDon);

        }

        private void qltk_Click(object sender, EventArgs e)
        {
            if (formThongKe == null)
                formThongKe = new ThongKe();
            if (currentFormOpen == formThongKe) return;
            resetButtonColor();
            currentFormOpen?.Hide();
            thongKeBtn.IdleFillColor = buttonActiveColor;

            setForm(formThongKe);
        }

        private void qlkh_Click(object sender, EventArgs e)
        {
            if (formKhachHang == null)
                formKhachHang = new QLKhachHang();
            if (currentFormOpen == formKhachHang) return;
            resetButtonColor();
            currentFormOpen?.Hide();
            qlkhBtn.IdleFillColor = buttonActiveColor;

            setForm(formKhachHang);
        }

        private void qlhh_Click(object sender, EventArgs e)
        {
            if (formHangHoa == null)
                formHangHoa = new QLHangHoa();
            if (currentFormOpen == formHangHoa) return;
            resetButtonColor();
            currentFormOpen?.Hide();
            hangHoaBtn.IdleFillColor = buttonActiveColor;

            setForm(formHangHoa);
        }

        private void qllh_Click(object sender, EventArgs e)
        {
            if (formLoaiHang == null)
                formLoaiHang = new QLLoaiHang();

            if (currentFormOpen == formLoaiHang) return;

            resetButtonColor();
            currentFormOpen?.Hide();
            loaiHangBtn.IdleFillColor = buttonActiveColor;

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

        private void thongTinNhom_Click(object sender, EventArgs e)
        {
            if (formThongTinNhom == null)
                formThongTinNhom = new ThongTinNhom();
            if (currentFormOpen == formThongTinNhom) return;
            resetButtonColor();
            currentFormOpen?.Hide();
            qlkhBtn.IdleFillColor = buttonActiveColor;

            setForm(formThongTinNhom);
        }

        private void qlmk_Click(object sender, EventArgs e)
        {
            if (formMatKhau == null)
                formMatKhau = new QLDoiMatKhau();

            if (currentFormOpen == formMatKhau) return;

            resetButtonColor();
            currentFormOpen?.Hide();
            qlmk.IdleFillColor = buttonActiveColor;

            setForm(formMatKhau);
        }
    }
}
