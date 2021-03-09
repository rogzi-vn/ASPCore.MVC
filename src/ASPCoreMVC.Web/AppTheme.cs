using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.DependencyInjection;

namespace ASPCoreMVC.Web
{
    // Cấu hình để tùy chỉnh cho themeZ
    [ThemeName("AppTheme")]
    public class AppTheme : ITheme, ITransientDependency
    {
        public const string Name = "AppTheme";

        public const int Limit = 10;

        public const string DefaultLayout = "~/Layouts/_Layout.cshtml";
        public const string AccountLayout = "~/Layouts/_AuthLayout.cshtml";
        public const string EmptyLayout = "~/Layouts/_Empty.cshtml";

        public const string HeaderImports = "~/Partials/Header.Imports.cshtml";
        public const string FooterImports = "~/Partials/Footer.Imports.cshtml";

        public const string LayoutTopbar = "~/Partials/_Layout.Topbar.cshtml";
        public const string LayoutTopbarAdminMenu = "~/Partials/_Layout.Topbar.AdminMenu.cshtml";

        public const string LayoutSidebar = "~/Partials/_Layout.Sidebar.cshtml";
        public const string LayoutFooter = "~/Partials/_Layout.Footer.cshtml";
        public const string LayoutBreadcrumb = "~/Partials/_Layout.Breadcrumb.cshtml";
        public const string LayoutLogoutModal = "~/Partials/_Layout.LogoutModal.cshtml";
        public const string LayoutSelectFileModal = "~/Partials/_Layout.SelectFileModal.cshtml";
        public const string ContentNothing = "~/Partials/_Content.Nothing.cshtml";

        public const string UserProfileViewable = "~/Pages/_Common/Partials/UserProfiles/UserProfileViewable.cshtml";

        public const string PartialQARender = "~/Pages/Exams/Partials/PartialQARender.cshtml";

        public virtual string GetLayout(string name, bool fallbackToDefault = true)
        {
            switch (name)
            {
                case StandardLayouts.Application:
                    return DefaultLayout;
                case StandardLayouts.Account:
                    return AccountLayout;
                case StandardLayouts.Empty:
                    return EmptyLayout;
                default:
                    return fallbackToDefault
                        ? DefaultLayout
                        : null;
            }
        }
    }
}