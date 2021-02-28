using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Helpers
{
    public static class PaginateHelper
    {
        private static string isActive(int i, int pageindex)
        {
            return i == pageindex ? "active" : "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkRouteFormat">"/my-data/{0}" hoặc "my-data?page-param={0}"</param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string Generate(string linkRouteFormat, int p, long totalItemCount, int limit)
        {
            var TotalPages = (int)Math.Ceiling(totalItemCount / (double)limit);
            var HasPreviousPage = p > 1;
            var HasNextPage = p < TotalPages;
            var item = "";
            if (TotalPages <= 5)
                for (int i = 1; i <= TotalPages; i++)
                {
                    string disabled = p == i ? "disabled" : "";
                    item += "<li class=\"page-item " + disabled + " " + isActive(i, p) + "\">" +
                        GenerateTag(linkRouteFormat, i) +
                        "</li>";
                }
            else
            {
                int prev = p - TotalPages + 2 > 0 ? p - TotalPages + 2 : 0;
                int next = 0;
                if ((p - 2) > 1)
                {
                    item += "<li class=\"page-item\"><a class=\"page-link\">. . .</a></li>";
                }
                for (int i = p - 2 - prev; i <= p + 2 + next; i++)
                {
                    if (i < 1) { next++; continue; }
                    if (i > TotalPages) { continue; }
                    string disabled = p == i ? "disabled" : "";
                    item += "<li class=\"page-item " + disabled + " " + isActive(i, p) + "\">" + GenerateTag(linkRouteFormat, i) + "</li>";
                }
                if ((p + 2) < TotalPages) item += "<li class=\"page-item\"><a class=\"page-link\">. . .</a></li>";
            }
            var prevDisabled = !HasPreviousPage ? "disabled" : "";
            var nextDisabled = !HasNextPage ? "disabled" : "";

            return "<li data-popup=\"tooltip\" title=\"Trang đầu\" class=\"page-item " + prevDisabled + "\">" + GenerateTag(linkRouteFormat, 1, "<i class=\"fas fa-angle-double-left\"></i>") + "</li>&nbsp;" +
                "<li data-popup=\"tooltip\" title=\"Trước\" class=\"page-item " + prevDisabled + "\">" + GenerateTag(linkRouteFormat, p - 1, "<i class=\"fas fa-angle-left\"></i>") + "</li>" + item +
                "<li data-popup=\"tooltip\" title=\"Sau\" class=\"page-item " + nextDisabled + "\">" + GenerateTag(linkRouteFormat, p + 1, "<i class=\"fas fa-angle-right\"></i>") + "</li>&nbsp;" +
                "<li data-popup=\"tooltip\" title=\"Trang cuối\" class=\"page-item " + nextDisabled + "\">" + GenerateTag(linkRouteFormat, TotalPages, "<i class=\"fas fa-angle-double-right\"></i>") + "</li>";
        }

        private static string GenerateTag(string linkRouteFormat, int page, string display = "")
        {
            return "<a class=\"page-link\" href=\"" + string.Format(linkRouteFormat, page) + "\">" +
                (display.IsNullOrEmpty() ? page : display)
                + "</a>";
        }
    }
}
