using Microsoft.Reporting.WinForms;
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
    public partial class ThongKe : Form
    {
        QLHHDBDataContext db = new QLHHDBDataContext();
        public ThongKe()
        {
            InitializeComponent();
           
        }

        private void ThongKe_Load(object sender, EventArgs e)
        {


            var query = from p in db.HangHoas
                        join c in db.LoaiHangs
                        on p.LoaiHang equals c.MaLoai select new
                        {
                            p.MaHang,
                            p.TenHang,
                            c.TenLoai,
                            p.DonViTinh,
                            p.DonGia
                        };

            DataTable table = new DataTable();
            table.Columns.Add("MaHang");
            table.Columns.Add("TenHang");
            table.Columns.Add("LoaiHang");
            table.Columns.Add("DonViTinh");
            table.Columns.Add("DonGia");
            foreach(var item in query)
            {
                DataRow row = table.NewRow();
                row["MaHang"] = item.MaHang;
                row["TenHang"] = item.TenHang;
                row["LoaiHang"] = item.TenLoai;
                row["DonViTinh"] = item.DonViTinh;
                row["DonGia"] = item.DonGia;
                table.Rows.Add(row);
            }
           
            reportViewer1.LocalReport.ReportPath = "ThongKeMatHang.rdlc";
            var soure = new ReportDataSource("DataSet1", table);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(soure);
            this.reportViewer1.RefreshReport();

            var danhSachHoaDon = from a in db.HoaDons
                                 join b in db.CT_HoaDons
                                 on a.SoHD equals b.SoHD
                                 join c in db.HangHoas
                                 on b.MaHang equals c.MaHang
                                 select new
                                 {
                                     a.SoHD,
                                     a.NgayBan,
                                     c.TenHang,
                                     c.DonViTinh,
                                     c.DonGia,
                                     b.SoLuong,
                                     TongTien = b.DonGia
                                 };

            DataTable tableBill = new DataTable();
            DataTable tbCtHoaDon = new DataTable();
            DataTable tbMatHang = new DataTable();
            
            tableBill.Columns.Add("SoHD");
            tableBill.Columns.Add("NgayBan");
            tbMatHang.Columns.Add("TenHang");
            tbMatHang.Columns.Add("DonViTinh");
            tbMatHang.Columns.Add("DonGia");
            tbCtHoaDon.Columns.Add("SoLuong");
            tbCtHoaDon.Columns.Add("DonGia");
            tbCtHoaDon.Columns.Add("TongTien");
            long tongTien = 0;
            foreach(var item in danhSachHoaDon)
            {
                DataRow billRow = tableBill.NewRow();
                DataRow matHangRow = tbMatHang.NewRow();
                DataRow ctHoaDonRow = tbCtHoaDon.NewRow();
                billRow["SoHD"] = item.SoHD;
                billRow["NgayBan"] = item.NgayBan;
                matHangRow["TenHang"] = item.TenHang;
                matHangRow["DonViTinh"] = item.DonViTinh;
                matHangRow["DonGia"] = item.DonGia;
                ctHoaDonRow["SoLuong"] = item.SoLuong;
                ctHoaDonRow["DonGia"] = item.TongTien;
                tongTien += long.Parse(item.TongTien.ToString());

                tableBill.Rows.Add(billRow);
                tbMatHang.Rows.Add(matHangRow);
                tbCtHoaDon.Rows.Add(ctHoaDonRow);
            }


            StoreSumMoney storeSum = new StoreSumMoney();

            storeSum.TongTienHoaDon = tongTien.ToString("#,##0₫");

            List<StoreSumMoney> listSum = new List<StoreSumMoney>();
            listSum.Add(storeSum);
            TKHD.LocalReport.ReportPath = "ThongKeHoaDon.rdlc";
            var soure2 = new ReportDataSource("DataSet1", tableBill);
            var soure3 = new ReportDataSource("DataSet2", tbCtHoaDon);
            var soure4 = new ReportDataSource("DataSet3", tbMatHang);
            var soure6 = new ReportDataSource("DataSet5", listSum);
            TKHD.LocalReport.DataSources.Clear();
            TKHD.LocalReport.DataSources.Add(soure2);
            TKHD.LocalReport.DataSources.Add(soure3);
            TKHD.LocalReport.DataSources.Add(soure4);
            TKHD.LocalReport.DataSources.Add(soure6);
            this.TKHD.RefreshReport();

            

            
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            DataTable dtHangHoa = new DataTable();
            dtHangHoa.Columns.Add("MaHang");
            dtHangHoa.Columns.Add("SoLuong");
            dtHangHoa.Columns.Add("DonGia");
            
            var dsHangHoa = from a in db.CT_HoaDons
                            join b in db.HangHoas on a.MaHang equals b.MaHang
                            group a by b.TenHang into g
                            select new
                            {
                                TenHang = g.Key,
                                SoLuong = g.Sum(x => x.SoLuong),
                                ThanhTien = g.Sum(x => x.DonGia)
                            };
            long tongTien = 0;
            int year = 0;
            if (comboBoxYear.SelectedItem.ToString().Contains("2023"))
            {
                dsHangHoa = from a in db.CT_HoaDons
                            join b in db.HangHoas on a.MaHang equals b.MaHang
                            join c in db.HoaDons on a.SoHD equals c.SoHD
                            where c.NgayBan.Value.Year == 2023
                            group a by b.TenHang into g
                            select new
                            {
                                TenHang = g.Key,
                                SoLuong = g.Sum(x => x.SoLuong),
                                ThanhTien = g.Sum(x => x.DonGia)
                            };
                year = 2023;
            }

            else if (comboBoxYear.SelectedItem.ToString().Contains("2022"))
            {
                dsHangHoa = from a in db.CT_HoaDons
                            join b in db.HangHoas on a.MaHang equals b.MaHang
                            join c in db.HoaDons on a.SoHD equals c.SoHD
                            where c.NgayBan.Value.Year == 2022
                            group a by b.TenHang into g
                            select new
                            {
                                TenHang = g.Key,
                                SoLuong = g.Sum(x => x.SoLuong),
                                ThanhTien = g.Sum(x => x.DonGia)
                            };
                year = 2022;
            }

            else if (comboBoxYear.SelectedItem.ToString().Contains("2021"))
            {
                dsHangHoa = from a in db.CT_HoaDons
                            join b in db.HangHoas on a.MaHang equals b.MaHang
                            join c in db.HoaDons on a.SoHD equals c.SoHD
                            where c.NgayBan.Value.Year == 2021
                            group a by b.TenHang into g
                            select new
                            {
                                TenHang = g.Key,
                                SoLuong = g.Sum(x => x.SoLuong),
                                ThanhTien = g.Sum(x => x.DonGia)
                            };
                year = 2021;
            }
            else if (comboBoxYear.SelectedItem.ToString().Contains("2020"))
            {
                dsHangHoa = from a in db.CT_HoaDons
                            join b in db.HangHoas on a.MaHang equals b.MaHang
                            join c in db.HoaDons on a.SoHD equals c.SoHD
                            where c.NgayBan.Value.Year == 2020
                            group a by b.TenHang into g
                            select new
                            {
                                TenHang = g.Key,
                                SoLuong = g.Sum(x => x.SoLuong),
                                ThanhTien = g.Sum(x => x.DonGia)
                            };
                year = 2020;
            }



            foreach (var item in dsHangHoa)
            {
                DataRow row = dtHangHoa.NewRow();
                row["MaHang"] = item.TenHang;
                row["SoLuong"] = item.SoLuong;
                row["DonGia"] = item.ThanhTien;
                tongTien += long.Parse(row["DonGia"].ToString());
                dtHangHoa.Rows.Add(row);
            }

            StoreSumMoney store = new StoreSumMoney();
            List <StoreSumMoney> listSum = new List<StoreSumMoney>();
            store.TongTienHoaDon = tongTien.ToString("#,##0₫");
            listSum.Add(store);

            if (year > 0)
            {
                StoreYear.Year = "Năm: " + year;
            }
            else
            {
                StoreYear.Year = "";
            }
           
            BaoCaoMatHang.LocalReport.ReportPath = "BaoCaoBanHang.rdlc";
            var nguonHangHoa = new ReportDataSource("DataSet1", dtHangHoa);
            var nguonHangHoa2 = new ReportDataSource("DataSet2", listSum);
            var listYearData = new ReportDataSource("DataSet3", StoreYear.Year);
            BaoCaoMatHang.LocalReport.DataSources.Clear();
            BaoCaoMatHang.LocalReport.DataSources.Add(nguonHangHoa);
            BaoCaoMatHang.LocalReport.DataSources.Add(nguonHangHoa2);
            BaoCaoMatHang.LocalReport.DataSources.Add(listYearData);
            this.BaoCaoMatHang.RefreshReport();
        }
    }
}
