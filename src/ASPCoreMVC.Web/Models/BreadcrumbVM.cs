using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Models
{
    public class BreadcrumbVM
    {
        public string Title { get; set; }
        public string Href { get; set; }
        public static List<BreadcrumbVM> BuildBreadcrumb(params string[][] inp)
        {
            var breadcrumbs = new List<BreadcrumbVM>();
            for (int i = 0; i < inp.Length; i++)
            {
                if (inp[i].Length == 1)
                {
                    breadcrumbs.Add(new BreadcrumbVM
                    {
                        Title = inp[i][0],
                        Href = ""
                    });
                }
                else if (inp[i].Length > 1)
                {
                    breadcrumbs.Add(new BreadcrumbVM
                    {
                        Title = inp[i][0],
                        Href = inp[i][1] ?? ""
                    });
                }
            }
            return breadcrumbs;
        }
    }
}
