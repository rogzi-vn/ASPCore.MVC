using ASPCoreMVC.Common;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Helpers
{
    public static class ActionHelper
    {
        private static List<BaseMap> GetBreadcrumbs(this ViewDataDictionary vd)
        {
            List<BaseMap> breadcrumbs = new List<BaseMap>();

            if (vd["DictionaryBreadcrumbs"] != null)
            {
                var _bredcrumbs = (List<BaseMap>)vd["Breadcrumbs"];
                breadcrumbs.Add(new BaseMap(vd["Title"].ToString(), "javascript:;"));
                breadcrumbs.AddRange(_bredcrumbs);
            }
            else if (vd["Breadcrumbs"] != null)
            {
                var _bredcrumbs = (List<string>)vd["Breadcrumbs"];
                foreach (var brc in _bredcrumbs)
                {
                    breadcrumbs.Add(new BaseMap(brc, "javascript:;"));
                }
            }
            return breadcrumbs;
        }

        public static List<BaseMap> GetBreadcrumbs(this PageModel controller)
        {
            return controller.ViewData.GetBreadcrumbs();
        }
        public static List<BaseMap> GetBreadcrumbs(this IHtmlHelper<dynamic> controller)
        {
            return controller.ViewData.GetBreadcrumbs();
        }
        private static void UpdateLinear(this ViewDataDictionary vd, params string[] titleOrBreadcrumbr)
        {
            var brs = new List<string>();
            brs.AddRange(titleOrBreadcrumbr);
            vd["Title"] = titleOrBreadcrumbr.FirstOrDefault();
            vd["Breadcrumbs"] = brs;
        }

        public static void AddTitle(this PageModel controller, params string[] titleOrBreadcrumbr)
        {
            if (titleOrBreadcrumbr == null || titleOrBreadcrumbr.Length <= 0)
                throw new Exception("Tiêu đề không được rỗng");

            controller.ViewData.UpdateLinear(titleOrBreadcrumbr);
        }


        private static void Update(this ViewDataDictionary vd, string title, params BaseMap[] baseMap)
        {
            var brs = new List<BaseMap>();
            brs.Add(new BaseMap(title, "javascript:;"));
            brs.AddRange(baseMap);
            vd["Title"] = title;
            vd["DictionaryBreadcrumbs"] = brs;
        }

        public static void AddTitle(this PageModel controller, string title, params BaseMap[] baseMap)
        {
            if (title == null || title.Length <= 0)
                throw new Exception("Tiêu đề không được rỗng");
            if (baseMap == null || baseMap.Length <= 0)
            {
                controller.ViewData.UpdateLinear(title);
                return;
            }
            controller.ViewData.Update(title, baseMap);
        }
    }
}
