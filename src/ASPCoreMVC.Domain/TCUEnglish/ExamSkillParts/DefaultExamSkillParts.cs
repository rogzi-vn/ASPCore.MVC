using ASPCoreMVC.TCUEnglish.ExamSkillCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreMVC.TCUEnglish.ExamSkillParts
{
    public static class DefaultExamSkillParts
    {
        public static ExamSkillPart B1_Listening_Part1 = new ExamSkillPart
        {
            Order = 0,
            ExamSkillCategoryId = DefaultExamSkillCategories.B1_Listening.Id,
            MasterContentType = Common.MasterContentTypes.Audio,
            AnswerType = Common.AnswerTypes.ImageAnswer,
            Name = "Part 1",
            NumDisplay = 7,
            Instructions = "",
            Tips = "",
            NumSubQues = 1,
            NumAns = 3,
            LimitTimeInMinutes = 15,
            MaxScores = 14,
            ArticleEditor = Common.EditorDisplayOptions.FullOption,
            IsHaveQuestionText = true,
            TrueAnswerType = Common.TrueAnswerTypes.OnlyOneCorrect
        }.SetId("d2fbb4de-d263-4501-87d2-7229285bac0f");
        public static ExamSkillPart B1_Listening_Part2 = new ExamSkillPart
        {
            Order = 1,
            ExamSkillCategoryId = DefaultExamSkillCategories.B1_Listening.Id,
            MasterContentType = Common.MasterContentTypes.Audio,
            AnswerType = Common.AnswerTypes.TextAnswer,
            Name = "Part 2",
            NumDisplay = 1,
            Instructions = "",
            Tips = "",
            NumSubQues = 6,
            NumAns = 3,
            LimitTimeInMinutes = 10,
            MaxScores = 6,
            ArticleEditor = Common.EditorDisplayOptions.FullOption,
            IsHaveQuestionText = true,
            TrueAnswerType = Common.TrueAnswerTypes.OnlyOneCorrect
        }.SetId("fad54862-373a-41c0-99ea-494ec90424e1");
        public static ExamSkillPart B1_Reading_Part1 = new ExamSkillPart
        {
            Order = 0,
            ExamSkillCategoryId = DefaultExamSkillCategories.B1_Reading.Id,
            MasterContentType = Common.MasterContentTypes.Grammar,
            AnswerType = Common.AnswerTypes.TextAnswer,
            Name = "Part 1",
            NumDisplay = 10,
            Instructions = "",
            Tips = "",
            NumSubQues = 1,
            NumAns = 4,
            LimitTimeInMinutes = 10,
            MaxScores = 10,
            ArticleEditor = Common.EditorDisplayOptions.FullOption,
            IsHaveQuestionText = true,
            TrueAnswerType = Common.TrueAnswerTypes.OnlyOneCorrect
        }.SetId("216af4b5-45cc-4315-ad79-be7f3430f293");
        public static ExamSkillPart B1_Reading_Part2 = new ExamSkillPart
        {
            Order = 1,
            ExamSkillCategoryId = DefaultExamSkillCategories.B1_Reading.Id,
            MasterContentType = Common.MasterContentTypes.Image,
            AnswerType = Common.AnswerTypes.TextAnswer,
            Name = "Part 2",
            NumDisplay = 5,
            Instructions = "",
            Tips = "",
            NumSubQues = 1,
            NumAns = 3,
            LimitTimeInMinutes = 5,
            MaxScores = 5,
            ArticleEditor = Common.EditorDisplayOptions.FullOption,
            IsHaveQuestionText = true,
            TrueAnswerType = Common.TrueAnswerTypes.OnlyOneCorrect
        }.SetId("8a80db21-e2c5-4ec2-9b29-0f8651db952e");
        public static ExamSkillPart B1_Reading_Part3 = new ExamSkillPart
        {
            Order = 2,
            ExamSkillCategoryId = DefaultExamSkillCategories.B1_Reading.Id,
            MasterContentType = Common.MasterContentTypes.Article,
            AnswerType = Common.AnswerTypes.TextAnswer,
            Name = "Part 3",
            NumDisplay = 1,
            Instructions = "",
            Tips = "",
            NumSubQues = 5,
            NumAns = 4,
            LimitTimeInMinutes = 10,
            MaxScores = 5,
            ArticleEditor = Common.EditorDisplayOptions.FullOption,
            IsHaveQuestionText = true,
            TrueAnswerType = Common.TrueAnswerTypes.OnlyOneCorrect
        }.SetId("eec3dbe1-3128-4630-977c-ca838f35ee1e");
        public static ExamSkillPart B1_Reading_Part4 = new ExamSkillPart
        {
            Order = 3,
            ExamSkillCategoryId = DefaultExamSkillCategories.B1_Reading.Id,
            MasterContentType = Common.MasterContentTypes.Article,
            AnswerType = Common.AnswerTypes.TextAnswer,
            Name = "Part 4",
            NumDisplay = 1,
            Instructions = "",
            Tips = "",
            NumSubQues = 10,
            NumAns = 4,
            LimitTimeInMinutes = 10,
            MaxScores = 10,
            ArticleEditor = Common.EditorDisplayOptions.FullOption,
            IsHaveQuestionText = false,
            TrueAnswerType = Common.TrueAnswerTypes.OnlyOneCorrect
        }.SetId("875fb789-c26f-44d5-921a-2e9fa8c67fde");

        public static ExamSkillPart B1_Writing_Part1 = new ExamSkillPart
        {
            Order = 0,
            ExamSkillCategoryId = DefaultExamSkillCategories.B1_Writing.Id,
            MasterContentType = Common.MasterContentTypes.Rewrite,
            AnswerType = Common.AnswerTypes.FillAnswer,
            Name = "Part 1",
            NumDisplay = 1,
            Instructions = "",
            Tips = "",
            NumSubQues = 5,
            NumAns = 4,
            LimitTimeInMinutes = 15,
            MaxScores = 10,
            ArticleEditor = Common.EditorDisplayOptions.Ignore,
            IsHaveQuestionText = true,
            TrueAnswerType = Common.TrueAnswerTypes.FullPickOneCorrect
        }.SetId("8b0b10f3-9383-4c47-90ec-4536975e47f1");

        public static ExamSkillPart B1_Writing_Part2 = new ExamSkillPart
        {
            Order = 1,
            ExamSkillCategoryId = DefaultExamSkillCategories.B1_Writing.Id,
            MasterContentType = Common.MasterContentTypes.Article,
            AnswerType = Common.AnswerTypes.WriteAnswer,
            Name = "Part 2",
            NumDisplay = 1,
            Instructions = "",
            Tips = "",
            NumSubQues = 1,
            NumAns = 1,
            LimitTimeInMinutes = 30,
            MaxScores = 20,
            ArticleEditor = Common.EditorDisplayOptions.FullOption,
            IsHaveQuestionText = true,
            TrueAnswerType = Common.TrueAnswerTypes.OnlyOneCorrect
        }.SetId("9286e252-750e-4bff-8fd1-9e29f9f0e5bc");

        public static ExamSkillPart B1_Speaking_Part1 = new ExamSkillPart
        {
            Order = 0,
            ExamSkillCategoryId = DefaultExamSkillCategories.B1_Speaking.Id,
            MasterContentType = Common.MasterContentTypes.Video,
            AnswerType = Common.AnswerTypes.RecorderAnswer,
            Name = "Part 1",
            NumDisplay = 1,
            Instructions = "",
            Tips = "",
            NumSubQues = 1,
            NumAns = 1,
            LimitTimeInMinutes = 10,
            MaxScores = 20,
            ArticleEditor = Common.EditorDisplayOptions.FullOption,
            IsHaveQuestionText = false
        }.SetId("e52a321e-0677-4b5b-8e2a-477e3031f963");
    }
}
