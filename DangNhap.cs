using N5;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Nhom5_TVThinhNHQHuyPNTanDVDucTNQuynh_LTNet
{
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }

        private void DangNhap_Load(object sender, EventArgs e)
        {

        }


        private void bunifuButton2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {

        }

        // btn dang nhap
        private void bunifuButton1_Click_1(object sender, EventArgs e)
        {
            string taiKhoan = txtTaiKhoan.Text;
            string matKhau = txtMatKhau.Text;

            bool isHasErr = false;
            // set error
            if (taiKhoan == "")
            {
                taiKhoanErr.Text = "Vui lòng nhập tài khoản.";
                isHasErr = true;
            }
            if (matKhau == "")
            {
                matKhauErr.Text = "Vui lòng nhập mật khẩu.";
                isHasErr = true;
            }

            if (isHasErr) return;

            try
            {
                QLHHDBDataContext db = new QLHHDBDataContext();

                var result = db.NhanViens
                    .Where(p => p.MaNV == taiKhoan && p.MatKhau == matKhau)
                    .Select(p => new
                    {
                        p.MaNV,
                        p.TenNV,
                        p.Quyen,
                        p.DiaChi
                    })
                    .Single();

                if (result == null) throw new Exception();
            
                Store.User = new NhanVien
                {
                    MaNV = result.MaNV,
                    TenNV = result.TenNV,
                    Quyen = result.Quyen,
                    DiaChi = result.DiaChi
                };

                (new Dashboard()).Show();
                this.Hide();

            } catch
            {
                formError.Text = "Tài khoản hoặc mật khẩu không chính xác.";
            }

        }

        private void txtTaiKhoan_TextChange(object sender, EventArgs e)
        {
            taiKhoanErr.Text = "";
            formError.Text = "";
        }

        private void txtMatKhau_TextChange(object sender, EventArgs e)
        {
            matKhauErr.Text = "";
            formError.Text = "";
        }
    }
}
