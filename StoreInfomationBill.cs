using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhom5_TVThinhNHQHuyPNTanDVDucTNQuynh_LTNet
{
    class StoreInfomationBill
    {
        public DataTable dataTable { get; set; }
        public String maHoaDon { get; set; }
        public String tongTienThanhToan { get; set; }
        public string tenKhach { get; set; }
        public string nhanVien { get; set; }
        public DateTime ngay { get; set; }
    }
}
