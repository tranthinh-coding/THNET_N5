using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhom5_TVThinhNHQHuyPNTanDVDucTNQuynh_LTNet
{
    public class Constants
    {
        public static int Admin = 1;
        public static int NhanVien = 0;

        public static bool IsAdmin(int id) { return Admin == id; }

        public static bool IsNhanVien(int id) { return NhanVien == id; }
    }
}
