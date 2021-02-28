using ASPCoreMVC.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ASPCoreMVC.Permissions
{
    public class ASPCoreMVCPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var aspCoreMVC = context.AddGroup(ASPCoreMVCPermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(ASPCoreMVCPermissions.MyPermission1, L("Permission:MyPermission1"));

            var userProfilesPermission = aspCoreMVC.AddPermission(ASPCoreMVCPermissions.UserProfiles.Default, L("Trans:Permission:UserProfiles"));
            userProfilesPermission.AddChild(ASPCoreMVCPermissions.UserProfiles.Create, L("Trans:Permission:UserProfiles:Create"));
            userProfilesPermission.AddChild(ASPCoreMVCPermissions.UserProfiles.Edit, L("Trans:Permission:UserProfiles:Edit"));
            userProfilesPermission.AddChild(ASPCoreMVCPermissions.UserProfiles.Delete, L("Trans:Permission:UserProfiles:Delete"));

            var appFilesPermission = aspCoreMVC.AddPermission(ASPCoreMVCPermissions.AppFiles.Default, L("Trans:Permission:AppFiles"));
            appFilesPermission.AddChild(ASPCoreMVCPermissions.AppFiles.Create, L("Trans:Permission:AppFiles:Create"));
            appFilesPermission.AddChild(ASPCoreMVCPermissions.AppFiles.Edit, L("Trans:Permission:AppFiles:Edit"));
            appFilesPermission.AddChild(ASPCoreMVCPermissions.AppFiles.Delete, L("Trans:Permission:AppFiles:Delete"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ASPCoreMVCResource>(name);
        }
    }
}
