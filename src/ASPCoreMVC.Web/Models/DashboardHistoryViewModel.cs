using ASPCoreMVC.TCUEnglish.ExamLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.Web.Models
{
    public class DashboardHistoryViewModel
    {
        public ExamHistoryStatDTO Stats { get; set; }
        public PagedResultDto<ExamLogBaseDTO> Histories { get; set; }
    }
}
