using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.WordClasses
{
    public class WordClassService : WrapperCrudAppService<
        WordClass,
        WordClassDTO,
        Guid,
        GetWordClassDTO>, IWordClassService
    {
        public WordClassService(IRepository<WordClass, Guid> repo) : base(repo) { }

        public async Task<ResponseWrapper<List<WordClassDTO>>> GetAll()
        {
            return new ResponseWrapper<List<WordClassDTO>>()
                .SuccessReponseWrapper(ObjectMapper.Map<
                List<WordClass>, List<WordClassDTO>>(await Repository.GetListAsync()), "Successful");
        }

        protected override async Task<IQueryable<WordClass>> CreateFilteredQueryAsync(GetWordClassDTO input)
        {
            var query = await Repository.GetQueryableAsync();
            return query.Where(x => input.Filter.IsNullOrEmpty() || x.Name.Contains(input.Filter));
        }
    }
}
