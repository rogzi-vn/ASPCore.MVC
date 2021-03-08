using ASPCoreMVC.TCUEnglish.ExamCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreMVC.TCUEnglish.ExamSkillCategories
{
    public static class DefaultExamSkillCategories
    {
        public static ExamSkillCategory B1_Listening = new ExamSkillCategory
        {
            Order = 0,
            ExamCategoryId = DefaultExamCategories.B1.Id,
            Name = "Listening",
            Tips = "",
            LimitTimeInMinutes = 20,
            MaxScores = 20
        }.SetId("9e5536e2-1fc6-425a-93dd-e8e0c2019bbc");

        public static ExamSkillCategory B1_Reading = new ExamSkillCategory
        {
            Order = 1,
            ExamCategoryId = DefaultExamCategories.B1.Id,
            Name = "Reading",
            Tips = "",
            LimitTimeInMinutes = 45,
            MaxScores = 30
        }.SetId("5318ae1e-2cd6-47e3-a3df-f7d40af482aa");

        public static ExamSkillCategory B1_Writing = new ExamSkillCategory
        {
            Order = 2,
            ExamCategoryId = DefaultExamCategories.B1.Id,
            Name = "Writing",
            Tips = "",
            LimitTimeInMinutes = 45,
            MaxScores = 30
        }.SetId("93057d24-c2d6-4152-8569-4021f46f4d24");

        public static ExamSkillCategory B1_Speaking = new ExamSkillCategory
        {
            Order = 3,
            ExamCategoryId = DefaultExamCategories.B1.Id,
            Name = "Speaking",
            Tips = "",
            LimitTimeInMinutes = 10,
            MaxScores = 20
        }.SetId("a3fa21d9-60ec-4b93-8e12-e5dec01e4c24");
    }
}
