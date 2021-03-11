using ASPCoreMVC.AppUsers;
using ASPCoreMVC.TCUEnglish.UserExams;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPCoreMVC.Web.Models
{
    public class ExamRenderViewModel
    {
        [BindProperty]
        public ExamForRenderDTO ExamContent { get; set; }
        [BindProperty]
        public Guid? ExamLogId { get; set; }

        public AppUserDTO ExamUser { get; set; }
        public string ExamName { get; set; } = "";
        public float LimitTime { get; set; } = 0F;
    }
}
