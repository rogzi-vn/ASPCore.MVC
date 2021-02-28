using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreMVC.TCUEnglish.ExamCategories
{
    public static class DefaultExamCategories
    {
        public static ExamCategory B1 = new ExamCategory
        {
            Name = "B1",
            Description = "Kỳ thi tiếng anh chuẩn B1",
            Tips = ""
        }.SetId("db7762be-a763-4431-bf4a-43ca7fb7fc57");
    }
}
