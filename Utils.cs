using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nhom5_TVThinhNHQHuyPNTanDVDucTNQuynh_LTNet
{
    internal class Utils
    {
        public static string RandomID(string prefix = "")
        {
            Random random = new Random();

            int number = random.Next(10000);

            if (number < 10)
            {
                return prefix + "0000" + number;
            }
            if (number < 100)
            {
                return prefix + "000" + number;
            }
            if (number < 1000)
            {
                return prefix + "00" + number;
            }
            return prefix + number;
        }

    }
}
