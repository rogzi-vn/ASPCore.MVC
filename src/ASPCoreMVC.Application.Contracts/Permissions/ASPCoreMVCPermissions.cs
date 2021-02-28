namespace ASPCoreMVC.Permissions
{
    public static class ASPCoreMVCPermissions
    {
        public const string GroupName = "ASPCoreMVC";

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";

        public static class UserProfiles
        {
            public const string Default = GroupName + ".UserProfiles";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }

        public static class AppFiles
        {
            public const string Default = GroupName + ".AppFiles";
            public const string Create = Default + ".Create";
            public const string Edit = Default + ".Edit";
            public const string Delete = Default + ".Delete";
        }
    }
}