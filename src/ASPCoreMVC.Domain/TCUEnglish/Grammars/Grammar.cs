using ASPCoreMVC.Localization;
using ASPCoreMVC.TCUEnglish._Common.LocalizeContent;
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace ASPCoreMVC.TCUEnglish.Grammars
{
    public class Grammar : AuditedAggregateRootAndLocalizeContent<Guid>
    {
        public Guid GrammarCategoryId { get; set; }
        /// <summary>
        /// Tên ngữ pháp
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Cấu trúc của ngữ pháp
        /// </summary>
        public string Structure { get; set; }
        /// <summary>
        /// Đoạn văn viết về cách sử dụng
        /// </summary>
        public string About { get; set; }
    }
}
