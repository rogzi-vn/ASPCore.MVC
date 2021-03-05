using ASPCoreMVC._Commons;
using ASPCoreMVC.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ASPCoreMVC.TCUEnglish.UserExams
{
    public interface IRenderExamService : IApplicationService
    {
        public Task<ResponseWrapper<ExamForRenderDTO>> GetRenderExam(RenderExamTypes type, Guid? destID);
    }
}
