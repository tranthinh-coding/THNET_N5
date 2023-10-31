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
            closeBtn.Cursor = Cursors.Hand;
            loginBtn.Cursor = Cursors.Hand;
            

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            
        }

        private void loginBtn_Paint(object sender, PaintEventArgs e)
        {
            string[] col = { "MaNV" };
            string username = usernameTb.Text;
            string password = passwordTb.Text;
            SqlDataAdapter da = DB.Table("NhanVien").Where("MaNV", username, "=").Where("MatKhau",password, "=").Select(col);
            if (da != null)
            {
                MessageBox.Show("Dang nhap thanh cong");
            }
        }

        private void bunifuGradientPanel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
