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
    public partial class QLDoiMatKhau : Form
    {
        QLHHDBDataContext db = new QLHHDBDataContext();

        public QLDoiMatKhau()
        {
            InitializeComponent();
        }


        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            txtErrCfrmPwd.Text = "";
            txtErrCurrPwd.Text = "";
            txtErrNextPwd.Text = "";

            bool err = false;
            if (string.IsNullOrEmpty(currPwd.Text))
            {
                txtErrCurrPwd.Text = "Vui lòng nhập mật khẩu hiện tại";
                err = true;
            }

            if (string.IsNullOrEmpty(nextPwd.Text))
            {
                txtErrNextPwd.Text = "Vui lòng nhập mật khẩu mới";
                err = true;
            }

            if (string.IsNullOrEmpty(cfirmPwd.Text))
            {
                txtErrCfrmPwd.Text = "Vui lòng nhập mật khẩu xác nhận";
                err = true;
            }

            if (nextPwd.Text != cfirmPwd.Text)
            {
                txtErrCfrmPwd.Text = "Mật khẩu xác nhận không chính xác";
                err = true;
            }

            if (err)
            {
                return;
            }

            NhanVien User = db.NhanViens.SingleOrDefault(k => k.MaNV == Store.User.MaNV);

            if (User.MatKhau != currPwd.Text)
            {
                MessageBox.Show("Mật khẩu của bạn không chính xác.");
                return;
            }

            User.MatKhau = nextPwd.Text;

            db.SubmitChanges();

            MessageBox.Show("Thay đổi mật khẩu thành công");

            txtErrCfrmPwd.Text = "";
            txtErrCurrPwd.Text = "";
            txtErrNextPwd.Text = "";
        }

        private void QLDoiMatKhau_Load(object sender, EventArgs e)
        {
            txtErrCfrmPwd.Text = "";
            txtErrCurrPwd.Text = "";
            txtErrNextPwd.Text = "";
        }

        private void currPwd_TextChange(object sender, EventArgs e)
        {
            txtErrCurrPwd.Text = "";
        }

        private void nextPwd_TextChange(object sender, EventArgs e)
        {
            txtErrNextPwd.Text = "";
        }

        private void cfirmPwd_TextChange(object sender, EventArgs e)
        {
            txtErrCfrmPwd.Text = "";
        }
    }
}
