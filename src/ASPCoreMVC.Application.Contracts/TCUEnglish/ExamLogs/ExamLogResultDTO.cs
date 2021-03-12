using System;
using System.Collections.Generic;
using System.Text;

namespace ASPCoreMVC.TCUEnglish.ExamLogs
{
    public class ExamLogResultDTO
    {
        public Guid LogId { get; set; }
        public List<QAPairDTO> Answers { get; set; }
    }
}
