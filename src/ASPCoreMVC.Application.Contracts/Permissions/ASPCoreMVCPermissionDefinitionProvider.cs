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

            var UserManagerPermission = aspCoreMVC.AddPermission(ASPCoreMVCPermissions.UserManager.Default, L("User Manager"));

            var WordClassesManagerPermission = aspCoreMVC.AddPermission(ASPCoreMVCPermissions.WordClassesManager.Default, L("Word Classes Manager"));

            var VocabularyTopicManagerPermission = aspCoreMVC.AddPermission(ASPCoreMVCPermissions.VocabularyTopicManager.Default, L("Vocabulary Topic Manager"));

            var VocabularyManagerPermission = aspCoreMVC.AddPermission(ASPCoreMVCPermissions.VocabularyManager.Default, L("Vocabulary Manager"));

            var FileManagerPermission = aspCoreMVC.AddPermission(ASPCoreMVCPermissions.FileManager.Default, L("File Manager"));

            var GrammarManagerPermission = aspCoreMVC.AddPermission(ASPCoreMVCPermissions.GrammarManager.Default, L("Grammar Manager"));

            var ExamManagerPermission = aspCoreMVC.AddPermission(ASPCoreMVCPermissions.ExamManager.Default, L("Exam Manager"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ASPCoreMVCResource>(name);
        }
    }
}
