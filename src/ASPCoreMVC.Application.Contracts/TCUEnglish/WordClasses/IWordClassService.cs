using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ASPCoreMVC.TCUEnglish.WordClasses
{
    public interface IWordClassService : IWrapperCrudAppService<
        WordClassDTO, Guid, GetWordClassDTO>
    {
        public Task<ResponseWrapper<List<WordClassDTO>>> GetAllWordClasses();
    }
}
