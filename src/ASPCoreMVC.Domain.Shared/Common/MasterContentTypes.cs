using System;
using System.Collections.Generic;
using System.Text;

namespace ASPCoreMVC.Common
{
    public enum MasterContentTypes
    {
        /// <summary>
        /// Bỏ qua nội dung  bậc cao
        /// </summary>
        Ignore,
        /// <summary>
        /// Nội dung là image
        /// </summary>
        Image,
        /// <summary>
        /// Nội dung là audio
        /// </summary>
        Audio,
        /// <summary>
        /// Nội dung là video
        /// </summary>
        Video,
        /// <summary>
        /// Nội dung là bài báo/đoạn văn
        /// </summary>
        Article,
        /// <summary>
        /// Nội dung là tham chiếu đến thư viện ngữ pháp
        /// </summary>
        Grammar
    }
}
