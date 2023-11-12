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
    public partial class QLLoaiHang : Form
    {
        QLHHDBDataContext db;
        int CurrRowSelect = -1;
      

        public QLLoaiHang()
        {
            InitializeComponent();
            db = new QLHHDBDataContext();
            db.Connection.ConnectionString = @"Data Source=Admin-PC;Initial Catalog=QLBanHangTapHoa;Integrated Security=True";
        }

        public void Render()
        {
            var render = db.LoaiHangs.Select(lh => new { lh.MaLoai, lh.TenLoai });
            dtViewLoaiHang.DataSource = render;

            CurrRowSelect = -1;
            txtMaLoai.Text = "";
            txtLoaiHang.Text = "";
        }

        private void QLLoaiHang_Load(object sender, EventArgs e)
        {
            txtMaLoai.Text = "";
            txtLoaiHang.Text = "";
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            txtMaLoai.Enabled = false;
            txtLoaiHang.Enabled = false;
            txtSearch.Enabled = false;
            dtViewLoaiHang.Enabled = false;
        }

        private void btnRender_Click(object sender, EventArgs e)
        {
            Render();
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            txtMaLoai.Enabled = true;
            txtLoaiHang.Enabled = true;
            txtSearch.Enabled = true;
            txtMaLoai.Text = "";
            txtLoaiHang.Text = "";
            btnRender.Enabled = false;
            dtViewLoaiHang.Enabled = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtMaLoai.Text = "";
            txtLoaiHang.Text = "";
            txtMaLoai.Focus();
            txtMaLoai.Enabled = true;
            txtLoaiHang.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            LoaiHang loaihang = new LoaiHang();
            loaihang.MaLoai = txtMaLoai.Text;
            loaihang.TenLoai = txtLoaiHang.Text;

            if (txtMaLoai.Text != "" && txtLoaiHang.Text != "")
            {
                if (db.LoaiHangs.Any(lhs => lhs.MaLoai == loaihang.MaLoai))
                {
                    MessageBox.Show("Thêm thất bại", "Error");
                }
                else
                {
                    MessageBox.Show("Thêm thành công", "Thông báo");
                    // Thêm vào bảng
                    db.LoaiHangs.InsertOnSubmit(loaihang);
                    // Thêm vào database
                    db.SubmitChanges();
                    Render();

                    txtMaLoai.Text = "";
                    txtLoaiHang.Text = "";
                    txtMaLoai.Focus();
                }
            }
            else
            {
                MessageBox.Show("Vui không để trống", "Error");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            txtMaLoai.Text = "";
            txtLoaiHang.Text = "";
            
            // Kiem tra loai hang co ton tai hay khong
            try
            {
                if (CurrRowSelect >= 0)
                {
                    LoaiHang loaihang = db.LoaiHangs.Single(lh => lh.MaLoai == dtViewLoaiHang.Rows[CurrRowSelect].Cells["MaLoai"].Value.ToString());
                    db.LoaiHangs.DeleteOnSubmit(loaihang);
                    db.SubmitChanges();
                    Render();
                }
                else
                {
                    MessageBox.Show("Vui lòng select để xóa", "Error");
                }
            } catch (Exception err)
            {
                // Thong bao co san pham dang lien ket voi loaihang nay. Xac nhan xoa hay khong
                MessageBox.Show("Không thể xoá loại hàng này.");
            }
           
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            txtMaLoai.Enabled = false;
            // Kiem tra loai hang co ton tai hay khong truoc khi sua
            // Neu ton tai roi thi sua
            // Neu khong ton tai thi them moi luon
            if (CurrRowSelect >= 0)
            {
                LoaiHang loaihang = db.LoaiHangs.Single(lh => lh.MaLoai == dtViewLoaiHang.Rows[CurrRowSelect].Cells["MaLoai"].Value.ToString());
                loaihang.MaLoai = txtMaLoai.Text;
                loaihang.TenLoai = txtLoaiHang.Text;
                if (txtMaLoai.Text != "" && txtLoaiHang.Text!= "") {
                    db.SubmitChanges();
                    Render();
                    txtMaLoai.Text = "";
                    txtLoaiHang.Text = "";
                } else
                {
                    MessageBox.Show("Vui không để trống", "Error");
                }
            }
        }

        private void dtViewLoaiHang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaLoai.Text = "";
            txtLoaiHang.Text = "";
            if (CurrRowSelect >= 0)
            {
                txtMaLoai.Text = dtViewLoaiHang.Rows[CurrRowSelect].Cells["MaLoai"].Value.ToString();
                txtLoaiHang.Text = dtViewLoaiHang.Rows[CurrRowSelect].Cells["TenLoai"].Value.ToString();
            }
        }

        private void dtViewLoaiHang_SelectionChanged(object sender, EventArgs e)
        {
            CurrRowSelect = dtViewLoaiHang.CurrentRow.Index;
            if (CurrRowSelect >= 0)
            {
                txtMaLoai.Text = dtViewLoaiHang.Rows[CurrRowSelect].Cells["MaLoai"].Value.ToString();
                txtLoaiHang.Text = dtViewLoaiHang.Rows[CurrRowSelect].Cells["TenLoai"].Value.ToString();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var search = db.LoaiHangs.Where(lh => lh.MaLoai.Contains(txtSearch.Text) || lh.TenLoai.Contains(txtSearch.Text)).Select(lh => new { lh.MaLoai, lh.TenLoai });
            dtViewLoaiHang.DataSource = search;
            txtMaLoai.Text = "";
            txtLoaiHang.Text = "";
        }

    }
}
