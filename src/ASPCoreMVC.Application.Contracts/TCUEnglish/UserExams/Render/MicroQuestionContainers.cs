using ASPCoreMVC.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.TCUEnglish.UserExams
{
    public class MicroQuestionContainers : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string MediaPath { get; set; }
        public string Article { get; set; }
        public List<MicroQuestionDTO> Questions { get; set; }
    }
}
