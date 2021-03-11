using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamCatInstructors;
using ASPCoreMVC.TCUEnglish.ExamSkillCategories;
using ASPCoreMVC.TCUEnglish.ExamSkillParts;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using ASPCoreMVC.TCUEnglish.SkillParts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.Web.Pages.Exams
{
    public class ExamsIndexModel : AppPageModel
    {
        private readonly IExamCategoryService _ExamCategoryServices;
        private readonly ISkillCategoryService _SkillCategoryServices;
        private readonly ISkillPartService _SkillPartServices;

        private readonly IExamCatInstructService _ExamCatInstructService;

        public List<Ex> Exams { get; set; } = new List<Ex>();
        public List<ExamCatInstructDTO> Instructors { get; set; } = new List<ExamCatInstructDTO>();

        public ExamsIndexModel(
            IExamCategoryService _ExamCategoryServices,
            ISkillCategoryService _SkillCategoryServices,
            ISkillPartService _SkillPartServices,
            IExamCatInstructService _ExamCatInstructService)
        {
            this._ExamCategoryServices = _ExamCategoryServices;
            this._SkillCategoryServices = _SkillCategoryServices;
            this._SkillPartServices = _SkillPartServices;
            this._ExamCatInstructService = _ExamCatInstructService;
        }
        public async Task OnGetAsync()
        {
            await GetDataCollection();
        }

        public async Task<List<Ex>> GetDataCollection()
        {
            // Lấy danh sách kỳ thi
            (await _ExamCategoryServices.GetBase()).Data
                .ForEach(x =>
                {
                    var ex = new Ex
                    {
                        Exam = x,
                        Scs = new List<Sc>()
                    };

                    _SkillCategoryServices.GetBase(x.Id).Data
                    .ForEach(y =>
                    {
                        var sc = new Sc
                        {
                            Cat = y,
                            Parts = _SkillPartServices.GetBase(y.Id).Data
                        };
                        ex.Scs.Add(sc);
                    });

                    Exams.Add(ex);
                });

            return Exams;
        }

    }

    public class Ex
    {
        public ExamCategoryBaseDTO Exam { get; set; }
        public List<Sc> Scs { get; set; }
    }

    public class Sc
    {
        public SkillCategoryBaseDTO Cat { get; set; }
        public List<SkillPartBaseDTO> Parts { get; set; }
    }


}
