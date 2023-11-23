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
    public partial class ChiTietHoaDon : Form
    {
        public DataTable dataTable { get; set; }
        public String maHoaDon { get; set; }
        public String tongTienThanhToan { get; set; }
        public string tenKhach { get; set; }
        public string nhanVien { get; set; }
        public DateTime ngay { get; set; }
        public ChiTietHoaDon()
        {
            InitializeComponent();

        }

        private void ChiTietHoaDon_Load(object sender, EventArgs e)
        {
            data.DataSource = dataTable;
            thanhTien.Text = tongTienThanhToan;
            soHD.Text = maHoaDon;
            ngayBan.Text = ngay.ToString();
            tenKhachHang.Text = tenKhach;
            tenNV.Text = "Đỗ Văn Đức";
            
        }

        private void btnThemHang_Click(object sender, EventArgs e)
        {
            QLHHDBDataContext db = new QLHHDBDataContext();
            try
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    CT_HoaDon cT_HoaDon = new CT_HoaDon();
                    int soLuong = int.Parse(row[3].ToString());
                    int donGia = int.Parse(row[4].ToString());
                    String tenHang = row[0].ToString();
                    var rs = db.HangHoas.Where(h => h.TenHang == tenHang).Select(p => p.MaHang).ToList();
                    if (rs.Any())
                    {
                        cT_HoaDon.MaHang = rs[0];
                    }
                    cT_HoaDon.SoHD = maHoaDon;
                    cT_HoaDon.SoLuong = soLuong;
                    cT_HoaDon.DonGia = donGia;

                    
                    db.CT_HoaDons.InsertOnSubmit(cT_HoaDon);
                    db.SubmitChanges();
                    
                }

                MessageBox.Show("In hóa đơn thành công");

                //StoreBill.maHoaDon = this.maHoaDon;
                //StoreBill.ngay = this.ngay;
                //StoreBill.tongTienThanhToan = this.tongTienThanhToan;
                //StoreBill.tenKhach = this.tenKhach;
                //StoreBill.nhanVien = this.nhanVien;


            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message);
                
            }
        }
    }
}
