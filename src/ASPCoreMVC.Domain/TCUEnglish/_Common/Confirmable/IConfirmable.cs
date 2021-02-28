using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreMVC.TCUEnglish._Common.Confirmable
{
    public interface IConfirmable
    {
        /// <summary>
        /// Trạng thái đã confirm hay chưa
        /// </summary>
        public bool IsConfirmed { get; set; }
        /// <summary>
        /// Thời điểm confirm
        /// </summary>
        public DateTime? ConfirmedTime { get; set; }
        /// <summary>
        /// Người confirm
        /// </summary>
        public Guid? ConfirmerId { get; set; }
    }
}
