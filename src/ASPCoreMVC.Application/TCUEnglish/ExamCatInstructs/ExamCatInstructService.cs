using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamCatInstructors;
using ASPCoreMVC.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.ExamCatInstructs
{
    public class ExamCatInstructService : WrapperCrudAppService<
        ExamCatInstructor,
        ExamCatInstructDTO,
        Guid,
        GetExamCatInstructDTO,
        CreateUpdateExamCatInstructDTO>, IExamCatInstructService
    {
        private readonly IRepository<AppUser, Guid> _UserRepository;
        private readonly IRepository<ExamCategory, Guid> _ExamCategoryRepository;
        public ExamCatInstructService(
            IRepository<ExamCatInstructor, Guid> repo,
            IRepository<AppUser, Guid> _UserRepository,
            IRepository<ExamCategory, Guid> _ExamCategoryRepository) : base(repo)
        {
            this._UserRepository = _UserRepository;
            this._ExamCategoryRepository = _ExamCategoryRepository;
        }
        public async Task<ResponseWrapper<PagedResultDto<ExamCatInstructDTO>>> GetAllExamCatInstruct(GetExamCatInstructDTO input)
        {
            var query = await Repository.GetQueryableAsync();
            var tempQuery = query
                 // Joint with exam cat
                 .Join(
                 _ExamCategoryRepository,
                 eci => eci.ExamCategoryId,
                 ec => ec.Id,
                 (eci, ec) => new { eci, ec })
                 .Where(x => input.FilterExamName.IsNullOrEmpty() || x.ec.Name.Contains(input.FilterExamName))
                 .Where(x => input.ExamCategoryId == null || input.ExamCategoryId == Guid.Empty || x.eci.ExamCategoryId == input.ExamCategoryId)
                 // Join with user 
                 .Join(
                 _UserRepository,
                 x => x.eci.UserId,
                 u => u.Id,
                 (x, u) => new { x, u })
                 .Where(y => input.FilterDisplayName.IsNullOrEmpty() || y.u.DisplayName.Contains(input.FilterDisplayName))
                 .Where(y => input.UserId == null || input.UserId == Guid.Empty || y.x.eci.UserId == input.UserId);

            if (!input.Sorting.IsNullOrEmpty())
            {
                tempQuery = tempQuery.OrderBy(input.Sorting);
            }

            var resQuery = tempQuery.Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .Select(x => new ExamCatInstructDTO
                {
                    Id = x.x.eci.Id,
                    ExamCategoryId = x.x.eci.ExamCategoryId,
                    UserId = x.x.eci.UserId,
                    UserDisplayName = x.u.DisplayName,
                    ExamCategoryName = x.x.ec.Name
                });

            var res = new PagedResultDto<ExamCatInstructDTO>(query.Count(), resQuery.ToList());
            return new ResponseWrapper<
                PagedResultDto<ExamCatInstructDTO>>(res, "Successful");
        }
    }
}
