using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.ScoreLogs
{
    public class ScoreLogDTO : EntityDto<Guid>
    {
        public Guid ExamLogId { get; set; }
        public Guid DestId { get; set; }
        public float Scores { get; set; }
        public float MaxScores { get; set; }
        public float RateInParent { get; set; }


    }
}
