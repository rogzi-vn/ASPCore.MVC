using ASPCoreMVC._Commons;
using ASPCoreMVC._Commons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace ASPCoreMVC.TCUEnglish.ExamQuestionGroups
{
    public class ExamQuestionGroupService : WrapperCrudAppService<
        ExamQuestionGroup,
        QuestionGroupDTO,
        Guid, GetQuestionGroupDTO>, IExamQuestionGroupService
    {
        public ExamQuestionGroupService(IRepository<ExamQuestionGroup, Guid> repo) : base(repo)
        {

        }

        public override Task<ResponseWrapper<QuestionGroupDTO>> CreateAsync(QuestionGroupDTO input)
        {
            return base.CreateAsync(input);
        }

        protected override async Task<IQueryable<ExamQuestionGroup>> CreateFilteredQueryAsync(GetQuestionGroupDTO input)
        {
            var query = await Repository.GetQueryableAsync();
            return query.Where(x => x.SkillPartId == input.SkillPartId &&
            (input.Filter.IsNullOrEmpty() || x.Name.Contains(input.Filter)));
        }

        protected override IQueryable<ExamQuestionGroup> ApplySorting(IQueryable<ExamQuestionGroup> query, GetQuestionGroupDTO input)
        {
            if (input.Sorting.IsNullOrEmpty())
                return query;
            return query.OrderBy(input.Sorting);
        }

    }
}
