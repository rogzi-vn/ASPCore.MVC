namespace ASPCoreMVC.TCUEnglish.AppFiles
{
    public static class AppFileDefaults
    {
        public static AppFile RootDirectory = new AppFile
        {
            ParentId = null,
            Name = "Root",
            IsDirectory = true,
            Length = 0
        }.SetId("baffc1bb-19e5-4116-808e-d2d725cd494f")
            .SetPath("/");
        public static AppFile UserAvatarsDirectory = new AppFile
        {
            ParentId = RootDirectory.Id,
            Name = "User Avatars",
            IsDirectory = true,
            Length = 0
        }.SetId("ad11fdb9-b242-497d-a3bd-177fc7eb49bd")
            .SetPath("/user-avatars");
        public static AppFile AppPhotosDirectory = new AppFile
        {
            ParentId = RootDirectory.Id,
            Name = "Photos",
            IsDirectory = true,
            Length = 0
        }.SetId("10637afc-8f4e-4eaf-98f8-63d400df1e47")
            .SetPath("/photos");
        public static AppFile AppVideosDirectory = new AppFile
        {
            ParentId = RootDirectory.Id,
            Name = "Videos",
            IsDirectory = true,
            Length = 0
        }.SetId("2be8b695-a3bf-4f83-bf57-7aeda65bd5cc")
            .SetPath("/videos");
        public static AppFile AppAudiosDirectory = new AppFile
        {
            ParentId = RootDirectory.Id,
            Name = "Audios",
            IsDirectory = true,
            Length = 0
        }.SetId("d400bafd-9b0e-4137-b5b7-a4d3595cc659")
            .SetPath("/audios");
        public static AppFile AppCommonsDirectory = new AppFile
        {
            ParentId = RootDirectory.Id,
            Name = "Commons",
            IsDirectory = true,
            Length = 0
        }.SetId("0b7cad7c-d1e0-476e-9724-6c7c21f8c185")
            .SetPath("/commons");
        public static AppFile AppDocumentsDirectory = new AppFile
        {
            ParentId = RootDirectory.Id,
            Name = "Documents",
            IsDirectory = true,
            Length = 0
        }.SetId("822e2557-a824-46b7-9a85-df171dbe73b7")
            .SetPath("/document");
    }
}
