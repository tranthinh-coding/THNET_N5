using N5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhom5_TVThinhNHQHuyPNTanDVDucTNQuynh_LTNet
{
    public class Store
    {
        public static NhanVien User = null;

        public static bool IsNhanVien()
        {
            return User?.Quyen == 0;
        }

        public static bool IsAdmin()
        {
            return User?.Quyen == 1;
        }

        public static List<HangHoa> Products = new List<HangHoa>();
        public long TongTien { get; set; }
    }
}
