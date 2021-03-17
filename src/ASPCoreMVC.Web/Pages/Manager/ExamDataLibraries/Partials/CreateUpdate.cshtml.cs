using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCoreMVC._Commons;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamDataLibraries;
using ASPCoreMVC.TCUEnglish.Grammars;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using ASPCoreMVC.TCUEnglish.SkillParts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPCoreMVC.Web.Pages.Manager.ExamDataLibraries.Partials
{
    [Authorize]
    public class ExamDataCreateUpdateModel : AppPageModel
    {
        private readonly IExamCategoryService _ExamCategoryService;
        private readonly ISkillCategoryService _SkillCategoryService;
        private readonly ISkillPartService _SkillPartService;

        private readonly IExamDataLibraryService _ExamDataLibraryService;

        private readonly IGrammarService _GrammarService;

        [BindProperty]
        public ExamQuestionContainerDTO Container { get; set; }
        [BindProperty]
        public bool IsCreate { get; set; } = true;
        public ExamCategoryBaseDTO CurrentExamCategory { get; set; }
        public SkillCategoryBaseDTO CurrentSkillCategory { get; set; }
        public SkillPartBaseDTO CurrentSkillPart { get; set; }
        public List<GrammarSimpify> Grammars { get; set; }

        public ExamDataCreateUpdateModel(
            IExamCategoryService examCategoryService,
            ISkillCategoryService skillCategoryService,
            ISkillPartService skillPartService,
            IExamDataLibraryService examDataLibraryService,
            IGrammarService grammarService)
        {
            _ExamCategoryService = examCategoryService;
            _SkillCategoryService = skillCategoryService;
            _SkillPartService = skillPartService;
            _ExamDataLibraryService = examDataLibraryService;
            _GrammarService = grammarService;
        }

        public async Task<IActionResult> OnGetAsync(Guid? exId, Guid? skillCatId, Guid? skillPartId, Guid? id)
        {
            var redirectPath = await PreLaunch(exId, skillCatId, skillPartId, id);

            if (redirectPath != null)
            {
                if (redirectPath == "")
                    return Page();
                else
                    return Redirect(redirectPath);
            }

            /* Main code */

            ResponseWrapper<ExamQuestionContainerDTO> res;
            IsCreate = id == null || id == Guid.Empty;
            if (IsCreate)
                res = await _ExamDataLibraryService.GetForCreateAsync(CurrentSkillPart.Id);
            else
                res = await _ExamDataLibraryService.GetForUpdateAsync(id.Value);

            if (!res.Success)
            {
                ToastError(res.Message);
                return Redirect($"/manager/exam-categories/{exId}/skill-categories/{skillCatId}/skill-parts/{skillPartId}/exam-data-libraries");
            }
            Container = res.Data;

            return Page();
        }

        // Run and validate, get nesscessary data
        private async Task<string> PreLaunch(Guid? exId, Guid? skillCatId, Guid? skillPartId, Guid? id)
        {
            if (exId == null || exId == Guid.Empty)
            {
                ToastError(L["Please select correct exam category"]);
                return $"/manager/exam-categories";
            }

            if (skillCatId == null || skillCatId == Guid.Empty)
            {
                ToastError(L["Please select correct skill category"]);
                return $"/manager/exam-categories/{exId}/skill-categories";
            }

            if (skillPartId == null || skillPartId == Guid.Empty)
            {
                ToastError(L["Please select correct skill part"]);
                return $"/manager/exam-categories/{exId}/skill-categories/{skillCatId}/skill-parts";
            }

            // Lấy loại kỳ thi
            var examCat = await _ExamCategoryService.GetSimpify(exId.Value);
            if (!examCat.Success)
            {
                ToastError(examCat.Message);
                return $"/manager/exam-categories";
            }
            CurrentExamCategory = examCat.Data;

            // Lấy mục kỹ năng
            var skCat = _SkillCategoryService.GetSimpify(skillCatId.Value);
            if (!skCat.Success)
            {
                ToastError(skCat.Message);
                return $"/manager/exam-categories/{exId}/skill-categories";
            }
            CurrentSkillCategory = skCat.Data;

            // Lấy phần của mục kỹ năng
            var skPart = _SkillPartService.GetSimpify(skillPartId.Value);
            if (!skPart.Success)
            {
                ToastError(skPart.Message);
                return $"/manager/exam-categories/{exId}/skill-categories/{skillCatId}/skill-parts";
            }
            CurrentSkillPart = skPart.Data;

            if (CurrentSkillPart.MasterContentType == Common.MasterContentTypes.Grammar)
            {
                var res = await _GrammarService.GetAllSimpifyAsync();
                if (!res.Success || res.Data == null)
                {
                    ToastError(res.Message);
                    return "";
                }
                Grammars = res.Data;
            }

            return null;
        }

        public async Task<IActionResult> OnPostAsync(Guid? exId, Guid? skillCatId, Guid? skillPartId, Guid? id)
        {
            var redirectPath = await PreLaunch(exId, skillCatId, skillPartId, id);

            if (redirectPath != null)
            {
                if (redirectPath == "")
                    return Page();
                else
                    return Redirect(redirectPath);
            }

            /* Main code */

            if ((Container.MasterContentType == Common.MasterContentTypes.Audio ||
                Container.MasterContentType == Common.MasterContentTypes.Image ||
                Container.MasterContentType == Common.MasterContentTypes.Video) &&
                Container.MediaPath.IsNullOrEmpty())
            {
                ModelState.AddModelError("Container.MediaPath", L["You need to choose the appropriate file"]);
            }

            for (int i = 0; i < Container.Questions.Count; i++)
            {
                // Nếu thuộc kiểu nội dung lớn là kiểu dành cho câu hỏi kiểu viết lại thì tiến hành kiểm tra tính hợp lệ
                if (Container.MasterContentType == Common.MasterContentTypes.Rewrite &&
                    Container.Questions[i].TextClone.IsNullOrEmpty())
                {
                    ModelState.AddModelError($"Container.Questions[{i}].TextClone",
                        string.Format(L["You must input second sentence for rewrite questions"]));
                }

                // Nếu không phải kiểu trả lời là viết lại và ghi âm thì tiến hành yêu cầu chọn ít nhất 1 đáp án đúng
                if (!Container.Questions[i].Answers.Any(x => x.IsCorrect) &&
                    !Container.Questions[i].Answers.Any(x => x.AnswerType == Common.AnswerTypes.WriteAnswer) &&
                    !Container.Questions[i].Answers.Any(x => x.AnswerType == Common.AnswerTypes.RecorderAnswer))
                {
                    ModelState.AddModelError($"Container.Questions[{i}].Answers",
                        string.Format(L["You must select correct answer"]));
                }

                // Kiểm tra tính hợp lệ của từng câu trả lời
                for (int j = 0; j < Container.Questions[i].Answers.Count; j++)
                {
                    // Nếu nội dung câu trả lời là null hoặc bỏ trống
                    if (Container.Questions[i].Answers[j].AnswerContent.IsNullOrEmpty())
                    {
                        // Nếu câu trả lời thuộc: Full đáp án đúng, viết lại hoặc ghi âm thì tiến hành chỉnh sửa nội dung cứng
                        if (Container.Questions[i].Answers[j].TrueAnswerType == Common.TrueAnswerTypes.FullPickOneCorrect ||
                            Container.Questions[i].Answers[j].AnswerType == Common.AnswerTypes.WriteAnswer ||
                            Container.Questions[i].Answers[j].AnswerType == Common.AnswerTypes.RecorderAnswer)
                        {
                            Container.Questions[i].Answers[j].AnswerContent = "";
                            Container.Questions[i].Answers[j].IsCorrect = true;
                        }
                        else
                        {
                            // Ngược lại thì báo lỗi để người dùng tiến hành nhập nội dung
                            ModelState.AddModelError($"Container.Questions[{i}].Answers[{j}].AnswerContent", L["This field is required"]);
                        }
                    }
                }

                // Nếu không có bất cứ câu trả lời nào được nhập, và câu trả lời không thuộc kiểu viết hay ghi âm, tiến hành thông báo lỗi
                if (Container.Questions[i].Answers.All(x => x.AnswerContent.IsNullOrEmpty())
                    && !Container.Questions[i].Answers.Any(x => x.AnswerType == Common.AnswerTypes.WriteAnswer)
                    && !Container.Questions[i].Answers.Any(x => x.AnswerType == Common.AnswerTypes.RecorderAnswer))
                {
                    ModelState.AddModelError($"Container.Questions[{i}].Answers",
                        string.Format(L["You must input atleast one answers"]));
                }
            }

            if (Container.MasterContentType == Common.MasterContentTypes.Audio &&
                Container.Article.IsNullOrEmpty())
            {
                ModelState.AddModelError("Container.Article", L["You need input Article"]);
            }

            if (!ModelState.IsValid)
            {
                ToastError(L["Your input is invalid"]);
                return Page();
            }

            var res = await _ExamDataLibraryService.PostSyncAsync(Container);
            if (!res.Success)
            {
                ToastError(res.Message);
                return Page();
            }

            // Thành công hoàn toàn
            ToastSuccess(res.Message);
            return Redirect($"/manager/exam-categories/{exId}/skill-categories/{skillCatId}/skill-parts/{skillPartId}/exam-data-libraries");
        }
    }
}
