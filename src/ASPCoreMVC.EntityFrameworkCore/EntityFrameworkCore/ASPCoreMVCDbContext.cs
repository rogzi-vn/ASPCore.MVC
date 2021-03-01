using Microsoft.EntityFrameworkCore;
using ASPCoreMVC.Users;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Users.EntityFrameworkCore;
using ASPCoreMVC.TCUEnglish.ExamAnswers;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamQuestionGroups;
using ASPCoreMVC.TCUEnglish.ExamQuestions;
using ASPCoreMVC.TCUEnglish.ExamSkillCategories;
using ASPCoreMVC.TCUEnglish.ExamSkillParts;
using ASPCoreMVC.TCUEnglish.Grammars;
using ASPCoreMVC.TCUEnglish.GrammarCategories;
using ASPCoreMVC.TCUEnglish.Vocabularies;
using ASPCoreMVC.TCUEnglish.VocabularyTopics;
using ASPCoreMVC.TCUEnglish.WordClasses;
using ASPCoreMVC.TCUEnglish.AppFiles;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ASPCoreMVC.TCUEnglish.ExamQuestionContainers;
using ASPCoreMVC.TCUEnglish.UserNotes;
using ASPCoreMVC.TCUEnglish.ExamCatInstructors;
using ASPCoreMVC.TCUEnglish.MesGroups;
using ASPCoreMVC.TCUEnglish.UserMessages;
using ASPCoreMVC.TCUEnglish.MemberInstructors;

namespace ASPCoreMVC.EntityFrameworkCore
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See ASPCoreMVCMigrationsDbContext for migrations.
     */
    [ConnectionStringName("Default")]
    public class ASPCoreMVCDbContext : AbpDbContext<ASPCoreMVCDbContext>
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<ExamAnswer> ExamAnswers { get; set; }
        public DbSet<ExamCategory> ExamCategories { get; set; }
        public DbSet<ExamQuestionGroup> ExamQuestionGroups { get; set; }
        public DbSet<ExamQuestionContainer> ExamQuestionContainers { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<ExamSkillCategory> ExamSkillCategories { get; set; }
        public DbSet<ExamSkillPart> ExamSkillParts { get; set; }
        public DbSet<Grammar> Grammar { get; set; }
        public DbSet<GrammarCategory> GrammarCategories { get; set; }
        public DbSet<Vocabulary> Vocabularies { get; set; }
        public DbSet<VocabularyTopic> VocabularyTopics { get; set; }
        public DbSet<WordClass> WordClasses { get; set; }
        public DbSet<AppFile> AppFiles { get; set; }

        public DbSet<UserNote> UserNotes { get; set; }
        public DbSet<ExamCatInstructor> ExamCatInstructors { get; set; }
        public DbSet<MessGroup> MessGroups { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<MemberInstructor> MemberInstructors { get; set; }

        /* Add DbSet properties for your Aggregate Roots / Entities here.
         * Also map them inside ASPCoreMVCDbContextModelCreatingExtensions.ConfigureASPCoreMVC
         */

        public ASPCoreMVCDbContext(DbContextOptions<ASPCoreMVCDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure the shared tables (with included modules) here */

            builder.Entity<AppUser>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users"); //Sharing the same table "AbpUsers" with the IdentityUser

                b.ConfigureByConvention();
                b.ConfigureAbpUser();

                /* Configure mappings for your additional properties
                 * Also see the ASPCoreMVCEfCoreEntityExtensionMappings class
                 */
            });

            /* Configure your own tables/entities inside the ConfigureASPCoreMVC method */

            builder.ConfigureASPCoreMVC();
        }

        //protected override bool TryCancelDeletionForSoftDelete(EntityEntry entry)
        //{
        //    // Bỏ qua xóa mềm, xóa là xóa luôn
        //    //return base.TryCancelDeletionForSoftDelete(entry);
        //    return true;
        //}
    }
}
