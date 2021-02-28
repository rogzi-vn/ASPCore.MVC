using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASPCoreMVC.TCUEnglish.ExamQuestionGroups
{
    public interface IExamQuestionGroupService : IWrapperCrudAppService<QuestionGroupDTO, Guid, GetQuestionGroupDTO>
    {
    }
}
