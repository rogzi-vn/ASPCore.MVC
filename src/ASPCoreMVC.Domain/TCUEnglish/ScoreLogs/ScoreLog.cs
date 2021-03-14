using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.ScoreLogs
{
    public class ScoreLog : CreationAuditedEntity<Guid>
    {
        public Guid ExamLogId { get; set; }
        public Guid DestId { get; set; }
        public float Scores { get; set; }
        public float MaxScores { get; set; }
        public float RateInParent { get; set; }
    }
}
