using System;
using System.Collections.Generic;
using System.Text;

namespace ASPCoreMVC.Common
{
    public enum AnswerTypes
    {
        /// <summary>
        /// Danh sách các câu trả lời dưới dạng văn bản
        /// </summary>
        TextAnswer,
        /// <summary>
        /// Danh sách các câu trả lời dưới dạng hình ảnh
        /// </summary>
        ImageAnswer,
        /// <summary>
        /// Danh sách các câu trả lời dưới dạng âm thanh
        /// </summary>
        AudioAnswer,
        /// <summary>
        /// Câu trả lời dưới hình thức ghi âm và gửi tệp âm thanh
        /// </summary>
        RecorderAnswer,
        /// <summary>
        /// Câu trả lời dưới hình thức viết và gửi đáp án
        /// </summary>
        WriteAnswer,
        /// <summary>
        /// Câu trả lời dưới hình thức điền từ vào nơi còn thiếu
        /// </summary>
        FillAnswer
    }
}
