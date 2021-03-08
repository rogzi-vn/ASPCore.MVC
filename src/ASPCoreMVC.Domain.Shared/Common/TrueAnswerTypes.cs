using System;
using System.Collections.Generic;
using System.Text;

namespace ASPCoreMVC.Common
{
    public enum TrueAnswerTypes
    {
        /// <summary>
        /// Duy nhất 1 đáp án đúng
        /// </summary>
        OnlyOneCorrect,
        /// <summary>
        /// Nhiều sự lựa chọn đúng, đúng khi chọn 1 trong số đó
        /// </summary>
        //MultiplePickOneCorrect,
        /// <summary>
        /// Nhiều sự lựa chọn đúng, đúng khi chọn hết trong số đó
        /// </summary>
        //MultiplePickFullCorrect,
        /// <summary>
        /// Tất cả các câu trả lời là đúng, đúng khi là 1 trong số đó
        /// </summary>
        FullPickOneCorrect
    }
}
