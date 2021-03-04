using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace ASPCoreMVC.Web.Settings
{
    public class PageSettingProvider : SettingDefinitionProvider
    {
        public const string SITE_NAME = nameof(SITE_NAME);
        public const string SITE_LOGO = nameof(SITE_LOGO);
        public const string SITE_FAVICON = nameof(SITE_FAVICON);
        public const string SITE_DESCRIPTION = nameof(SITE_DESCRIPTION);
        public const string SITE_KEYWORDS = nameof(SITE_KEYWORDS);
        public const string SITE_AUTHOR = nameof(SITE_AUTHOR);

        public const string SITE_HOLDER_IMAGE = nameof(SITE_HOLDER_IMAGE);

        public const string LOGIN_ENABLE = nameof(LOGIN_ENABLE);
        public const string REGISTRATION_ENABLE = nameof(REGISTRATION_ENABLE);

        public const string LOGIN_SIBAR_IMAGE = nameof(LOGIN_SIBAR_IMAGE);
        public const string LOGIN_SIBAR_INTRO = nameof(LOGIN_SIBAR_INTRO);
        public const string LOGIN_SIBAR_DESCRIPTION = nameof(LOGIN_SIBAR_DESCRIPTION);

        public const string USER_CREATOR_DEFAULT_PASSWORD_FOR_NEW_USER = nameof(USER_CREATOR_DEFAULT_PASSWORD_FOR_NEW_USER);

        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                        new SettingDefinition(SITE_NAME, "ASPCoreMVC"),
                        new SettingDefinition(SITE_LOGO, "/dist/img/logo.png"),
                        new SettingDefinition(SITE_FAVICON, "/dist/img/favicon.ico"),
                        new SettingDefinition(SITE_DESCRIPTION, "ASPCore Description"),
                        new SettingDefinition(SITE_KEYWORDS, "ASPCore, Vistark, Nguyen Trong Nghia"),
                        new SettingDefinition(SITE_AUTHOR, "Nguyen Trong Nghia"),

                        new SettingDefinition(SITE_HOLDER_IMAGE, "/dist/img/holder.png"),

                        new SettingDefinition(LOGIN_ENABLE, "true"),
                        new SettingDefinition(REGISTRATION_ENABLE, "true"),
                        new SettingDefinition(LOGIN_SIBAR_IMAGE, "/dist/img/login-banner.png"),

                        new SettingDefinition(USER_CREATOR_DEFAULT_PASSWORD_FOR_NEW_USER, "12345678")
                    );
        }
    }
}
