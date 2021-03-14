using System;
using System.Collections.Generic;
using System.Text;

namespace ASPCoreMVC.Helpers
{
    public static class EzColor
    {
        public static string RandomHex()
        {
            var random = new Random();
            return string.Format("#{0:X6}", random.Next(0x1000000));
        }
    }
}
