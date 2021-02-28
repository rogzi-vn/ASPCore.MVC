using System;
using System.Collections.Generic;
using System.Text;

namespace ASPCoreMVC.Helpers
{
    public static class PathHelper
    {
        public static string TrueCombine(params string[] path)
        {
            return path.JoinAsString("/")
                .Replace(@"\", "/")
                .Replace("//", "/")
                .Replace("//", "/");
        }
    }
}
