using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Helpers
{
    // https://ned.im/noty
    public static class ToastHelper
    {
        private static string Generate(string message, string type)
        {
            return $"showToast('{type}','{message}');";
        }

        private static string Success(string message)
        {
            return Generate("<i class=\"far fa-check-circle\"></i>&nbsp;&nbsp;" + message, nameof(Success).ToLower());
        }
        private static string Error(string message)
        {
            return Generate("<i class=\"fas fa-exclamation-circle\"></i>&nbsp;&nbsp;" + message, nameof(Error).ToLower());
        }
        private static string Warning(string message)
        {
            return Generate("<i class=\"fas fa-exclamation-triangle\"></i>&nbsp;&nbsp;" + message, nameof(Warning).ToLower());
        }
        private static string Info(string message)
        {
            return Generate("<i class=\"fas fa-info-circle\"></i>&nbsp;&nbsp;" + message, nameof(Info).ToLower());
        }

        /// <summary>
        /// Render các thông báo nếu có ra trang HTML
        /// </summary>
        public static HtmlString ShowToasts(this IHtmlHelper<dynamic> helper)
        {

            // Khởi tạo biến chứa danh sách thông báo
            List<string> toasts = new List<string>();

            // Lấy danh sách các thông báo đang có
            if (helper.TempData[nameof(ToastHelper)] != null)
                toasts.AddRange((string[])helper.TempData[nameof(ToastHelper)]);

            // Nếu trống thì bỏ qua
            if (toasts == null || toasts.Count <= 0)
                return HtmlString.Empty;

            // Không thì cho hiển thị
            return new HtmlString("<script>\r\n" +
                string.Join("\r\n", toasts) +
                "\r\n</script>");
        }

        private static List<string> CollectToasts(this Controller controller)
        {
            // Khởi tạo biến chứa danh sách thông báo
            List<string> toasts = new List<string>();

            // Kiểm tra mục TempData xem có thông báo nào còn được lưu trữ không
            if (controller.TempData[nameof(ToastHelper)] != null)
                toasts.AddRange((string[])controller.TempData[nameof(ToastHelper)]);

            // Xóa bỏ các thông báo đã có
            controller.TempData[nameof(ToastHelper)] = null;

            // Trả về kết quả
            return toasts;
        }

        private static List<string> CollectToasts(this PageModel controller)
        {
            // Khởi tạo biến chứa danh sách thông báo
            List<string> toasts = new List<string>();

            // Kiểm tra mục TempData xem có thông báo nào còn được lưu trữ không
            if (controller.TempData[nameof(ToastHelper)] != null)
                toasts.AddRange((string[])controller.TempData[nameof(ToastHelper)]);

            // Xóa bỏ các thông báo đã có
            controller.TempData[nameof(ToastHelper)] = null;

            // Trả về kết quả
            return toasts;
        }

        /// <summary>
        /// Thêm một thông báo ở mức độ tùy chỉnh cao
        /// </summary>
        private static void AddToasts(this Controller controller, params string[] toasts)
        {
            if (toasts == null || toasts.Length <= 0)
                throw new Exception("Không thể để nội dung thông báo rỗng được");

            // Lấy danh sách thông báo đã có
            List<string> toastCollention = CollectToasts(controller) ?? new List<string>();

            // Thêm thông báo mới vào
            toastCollention.AddRange(toasts);

            // Lưu lại dữ liệu
            controller.TempData[nameof(ToastHelper)] = toastCollention.ToArray();
        }
        private static void AddToasts(this PageModel controller, params string[] toasts)
        {
            if (toasts == null || toasts.Length <= 0)
                throw new Exception("Không thể để nội dung thông báo rỗng được");

            // Lấy danh sách thông báo đã có
            List<string> toastCollention = CollectToasts(controller) ?? new List<string>();

            // Thêm thông báo mới vào
            toastCollention.AddRange(toasts);

            // Lưu lại dữ liệu
            controller.TempData[nameof(ToastHelper)] = toastCollention.ToArray();
        }

        /// <summary>
        /// Tạo một thông báo kiểu SUCCESS
        /// </summary>
        public static Controller ToastSuccess(this Controller controller, string message)
        {
            AddToasts(controller, Success(message));
            return controller;
        }
        public static T ToastSuccess<T>(this T controller, string message) where T : PageModel
        {
            AddToasts(controller, Success(message));
            return controller;
        }
        public static HtmlString ToastSuccess(this IHtmlHelper<dynamic> helper, string message)
        {
            return new HtmlString("<script>\r\n" +
                string.Join("\r\n", Success(message)) +
                "\r\n</script>");
        }

        /// <summary>
        /// Tạo một thông báo kiểu WARNING
        /// </summary>
        public static Controller ToastWarning(this Controller controller, string message)
        {
            AddToasts(controller, Warning(message));
            return controller;
        }

        public static T ToastWarning<T>(this T controller, string message) where T : PageModel
        {
            AddToasts(controller, Warning(message));
            return controller;
        }
        public static HtmlString ToastWarning(this IHtmlHelper<dynamic> helper, string message)
        {
            return new HtmlString("<script>\r\n" +
                string.Join("\r\n", Warning(message)) +
                "\r\n</script>");
        }

        /// <summary>
        /// Tạo một thông báo kiểu ERROR
        /// </summary>
        public static Controller ToastError(this Controller controller, string message)
        {
            AddToasts(controller, Error(message));
            return controller;
        }
        public static T ToastError<T>(this T controller, string message) where T : PageModel
        {
            AddToasts(controller, Error(message));
            return controller;
        }
        public static HtmlString ToastError(this IHtmlHelper<dynamic> helper, string message)
        {
            return new HtmlString("<script>\r\n" +
                string.Join("\r\n", Error(message)) +
                "\r\n</script>");
        }

        /// <summary>
        /// Tạo một thông báo kiểu INFO
        /// </summary>
        public static Controller ToastInfo(this Controller controller, string message)
        {
            AddToasts(controller, Info(message));
            return controller;
        }
        public static T ToastInfo<T>(this T controller, string message) where T : PageModel
        {
            AddToasts(controller, Info(message));
            return controller;
        }
        public static HtmlString ToastInfo(this IHtmlHelper<dynamic> helper, string message)
        {
            return new HtmlString("<script>\r\n" +
                string.Join("\r\n", Info(message)) +
                "\r\n</script>");
        }

    }
}
