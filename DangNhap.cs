﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    }
}
