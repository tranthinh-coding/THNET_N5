using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Linq;

namespace Nhom5_TVThinhNHQHuyPNTanDVDucTNQuynh_LTNet
{
    public partial class QLHoaDon : Form
    {
        QLHHDBDataContext db = new QLHHDBDataContext();
        List<HangHoa> list = new List<HangHoa>();
        public QLHoaDon()
        {
            InitializeComponent();
            
            
            var result = db.HangHoas.Select(p => new
            {
                p.MaHang,
                p.LoaiHang,
                p.TenHang,

            });
            matHang.DataSource = result.ToList();
            matHang.DisplayMember = "TenHang";
            matHang.ValueMember = "MaHang";
            db.SubmitChanges();
        }

        private void btnThemHang_Click(object sender, EventArgs e)
        {
            DataTable table = new DataTable();
            table.Columns.Add("TenHang", typeof(string));
            table.Columns.Add("DonViTinh", typeof(string));
            table.Columns.Add("DonGia", typeof(long));
            table.Columns.Add("SoLuong", typeof(int));
            table.Columns.Add("TongTien", typeof(long));
            var result = db.HangHoas.Select(p => new
            {
                p.TenHang,
                p.DonViTinh,
                p.DonGia
            });
            foreach (var item in result)
            {
                DataRow row = table.NewRow();

                row["TenHang"] = item.TenHang;
                row["DonViTinh"] = item.DonViTinh;
                row["DonGia"] = item.DonGia;

                table.Rows.Add(row);
            }
            data.DataSource = table;
            
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            tab.Height = 400;
            groupBox2.Height = 488;
        }

        private void radioButton1_AppearanceChanged(object sender, EventArgs e)
        {
            tab.Height = 153;
            groupBox2.Height = 370;
        }

        private void tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tab.SelectedIndex == 0)
            {
                tab.Height = 120;
                groupBox2.Height = 250;
            }
            else
            {
                tab.Height = 370;
                groupBox2.Height = 460;
            }
        }
    }
}
