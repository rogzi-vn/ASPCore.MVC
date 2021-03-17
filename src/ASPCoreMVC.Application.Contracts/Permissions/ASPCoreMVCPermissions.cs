namespace ASPCoreMVC.Permissions
{
    public static class ASPCoreMVCPermissions
    {
        public const string GroupName = "ASPCoreMVC";

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";

        public static class UserManager
        {
            public const string Default = GroupName + ".UserManager";
        }

        public static class WordClassesManager
        {
            public const string Default = GroupName + ".WordClassesManager";
        }

        public static class VocabularyTopicManager
        {
            public const string Default = GroupName + ".VocabularyTopicManager";
        }

        public static class VocabularyManager
        {
            public const string Default = GroupName + ".VocabularyManager";
        }

        public static class FileManager
        {
            public const string Default = GroupName + ".FileManager";
        }

        public static class GrammarManager
        {
            public const string Default = GroupName + ".GrammarManager";
        }

        public static class ExamManager
        {
            public const string Default = GroupName + ".ExamManager";
        }
    }
}