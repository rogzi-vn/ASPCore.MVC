using ASPCoreMVC.TCUEnglish.AppFiles;
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
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using ASPCoreMVC.TCUEnglish.ExamQuestionContainers;

namespace ASPCoreMVC.EntityFrameworkCore
{
    public static class ASPCoreMVCDbContextModelCreatingExtensions
    {
        public static void ConfigureASPCoreMVC(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(ASPCoreMVCConsts.DbTablePrefix + "YourEntities", ASPCoreMVCConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});


            builder.Entity<AppFile>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(AppFile), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API)
                b.Property(x => x.Name).IsRequired().HasMaxLength(255);
                b.Property(x => x.Path).IsRequired();
                b.Property(x => x.Length).IsRequired();

                b.HasOne<AppFile>()
                .WithMany()
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ExamAnswer>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(ExamAnswer), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API)
                b.Property(x => x.ExamQuestionId).IsRequired();
                b.Property(x => x.AnswerContent).IsRequired().HasMaxLength(255);
                b.Property(x => x.IsCorrect).IsRequired();

                b.HasOne<ExamQuestion>().WithMany().HasForeignKey(x => x.ExamQuestionId).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<ExamCategory>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(ExamCategory), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API)
                b.Property(x => x.Name).HasMaxLength(128);
            });
            builder.Entity<ExamQuestionContainer>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(ExamQuestionContainer), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API)
                b.Property(x => x.SkillPartId).IsRequired();
                b.Property(x => x.Name).HasMaxLength(128);
                b.HasOne<Grammar>().WithMany().HasForeignKey(x => x.GrammarId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne<ExamQuestionGroup>().WithMany().HasForeignKey(x => x.ExamQuestionGroupId).OnDelete(DeleteBehavior.SetNull);
            });
            builder.Entity<ExamQuestionGroup>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(ExamQuestionGroup), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API)
                b.Property(x => x.Name).HasMaxLength(128);
            });
            builder.Entity<ExamQuestion>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(ExamQuestion), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API)
                b.Property(x => x.Text).IsRequired().HasMaxLength(255);

                b.HasOne<ExamQuestionContainer>().WithMany().HasForeignKey(x => x.ExamQuestionContainerId).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<ExamSkillCategory>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(ExamSkillCategory), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API)
                b.Property(x => x.ExamCategoryId).IsRequired();
                b.Property(x => x.Name).IsRequired().HasMaxLength(255);
                b.Property(x => x.LimitTimeInMinutes).IsRequired();
                b.Property(x => x.MaxScores).IsRequired();

                b.HasOne<ExamCategory>().WithMany().HasForeignKey(x => x.ExamCategoryId).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<ExamSkillPart>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(ExamSkillPart), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API)
                b.Property(x => x.ExamSkillCategoryId).IsRequired();
                b.Property(x => x.MasterContentType).IsRequired();
                b.Property(x => x.AnswerType).IsRequired();
                b.Property(x => x.Name).IsRequired().HasMaxLength(255);
                b.Property(x => x.NumSubQues).IsRequired();
                b.Property(x => x.NumAns).IsRequired();
                b.Property(x => x.LimitTimeInMinutes).IsRequired();
                b.Property(x => x.MaxScores).IsRequired();

                b.HasOne<ExamSkillCategory>().WithMany().HasForeignKey(x => x.ExamSkillCategoryId).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Grammar>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(Grammar), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API)
                b.Property(x => x.GrammarCategoryId).IsRequired();
                b.Property(x => x.Name).IsRequired().HasMaxLength(255);
                b.Property(x => x.Structure).IsRequired().HasMaxLength(255);

                b.HasOne<GrammarCategory>().WithMany().HasForeignKey(x => x.GrammarCategoryId).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<GrammarCategory>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(GrammarCategory), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API
                b.Property(x => x.About).HasMaxLength(512);
            });
            builder.Entity<Vocabulary>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(Vocabulary), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API)
                b.Property(x => x.VocabularyTopicId).IsRequired();
                b.Property(x => x.WordClassId).IsRequired();
                b.Property(x => x.Word).IsRequired().HasMaxLength(128);
                b.Property(x => x.Pronounce).HasMaxLength(128);
                b.Property(x => x.PronounceAudio).HasMaxLength(255);

                b.HasOne<VocabularyTopic>().WithMany().HasForeignKey(x => x.VocabularyTopicId).OnDelete(DeleteBehavior.Cascade);
                b.HasOne<WordClass>().WithMany().HasForeignKey(x => x.WordClassId).OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<VocabularyTopic>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(VocabularyTopic), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API)
                b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            });
            builder.Entity<WordClass>(b =>
            {
                b.ToTable(ASPCoreMVCConsts.DbTablePrefix + nameof(WordClass), ASPCoreMVCConsts.DbSchema);
                //Configure the base properties
                b.ConfigureByConvention();

                //Configure other properties (if you are using the fluent API)
                b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            });
        }
    }
}