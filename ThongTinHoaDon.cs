﻿using System;
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
    public partial class ThongTinHoaDon : Form
    {
        public DataTable dataTable { get; set; }
        public String maHoaDon { get; set; }
        public String tongTienThanhToan { get; set; }
        public string tenKhach { get; set; }
        public string nhanVien { get; set; }
        public DateTime ngay { get; set; }
        public ThongTinHoaDon()
        {
            InitializeComponent();
            
        }

        private void ThongTinHoaDon_Load(object sender, EventArgs e)
        {
            data.DataSource = dataTable;
            thanhTien.Text = tongTienThanhToan;
            soHD.Text = maHoaDon;
            ngayBan.Text = ngay.ToString();
            tenKhachHang.Text = tenKhach;
            tenNV.Text = "Đỗ Văn Đức";
        }
    }
}