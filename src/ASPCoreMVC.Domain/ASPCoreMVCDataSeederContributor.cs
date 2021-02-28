using ASPCoreMVC.TCUEnglish.AppFiles;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamQuestionGroups;
using ASPCoreMVC.TCUEnglish.ExamSkillCategories;
using ASPCoreMVC.TCUEnglish.ExamSkillParts;
using ASPCoreMVC.TCUEnglish.GrammarCategories;
using ASPCoreMVC.TCUEnglish.VocabularyTopics;
using ASPCoreMVC.TCUEnglish.WordClasses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace ASPCoreMVC
{
    /// <summary>
    /// Tạo các mẫu dữ liệu mặc định đầu tiên
    /// </summary>
    public class ASPCoreMVCDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<AppFile, Guid> _AppFileRepository;
        private readonly IRepository<ExamCategory, Guid> _ExamCategoryRepository;
        private readonly IRepository<ExamSkillCategory, Guid> _ExamSkillCategoryRepository;
        private readonly IRepository<ExamSkillPart, Guid> _ExamSkillPartRepository;
        private readonly IRepository<ExamQuestionGroup, Guid> _ExamQuestionGroupRepository;

        private readonly IRepository<WordClass, Guid> _WordClassRepository;
        private readonly IRepository<VocabularyTopic, Guid> _VocabularyTopicRepository;
        private readonly IRepository<GrammarCategory, Guid> _GrammarCategoryRepository;
        public ASPCoreMVCDataSeederContributor(
            IRepository<AppFile, Guid> AppFileRepository,
            IRepository<ExamCategory, Guid> _ExamCategoryRepository,
            IRepository<ExamSkillCategory, Guid> _ExamSkillCategoryRepository,
            IRepository<ExamSkillPart, Guid> _ExamSkillPartRepository,
            IRepository<ExamQuestionGroup, Guid> _ExamQuestionGroupRepository,
            IRepository<WordClass, Guid> _WordClassRepository,
            IRepository<VocabularyTopic, Guid> _VocabularyTopicRepository,
            IRepository<GrammarCategory, Guid> _GrammarCategoryRepository)
        {
            _AppFileRepository = AppFileRepository;

            this._ExamCategoryRepository = _ExamCategoryRepository;
            this._ExamSkillCategoryRepository = _ExamSkillCategoryRepository;
            this._ExamSkillPartRepository = _ExamSkillPartRepository;
            this._ExamQuestionGroupRepository = _ExamQuestionGroupRepository;
            this._WordClassRepository = _WordClassRepository;
            this._VocabularyTopicRepository = _VocabularyTopicRepository;
            this._GrammarCategoryRepository = _GrammarCategoryRepository;

        }
        public async Task SeedAsync(DataSeedContext context)
        {
            //await SeedDefaultDirectory(context);
            await SeedBaseData(context);
            await SeedWordClasses(context);
            await SeedVocabularyTopics(context);
            await SeedGrammarCategories(context);
        }

        private async Task SeedGrammarCategories(DataSeedContext context)
        {
            if (await _GrammarCategoryRepository.CountAsync() <= 0)
            {
                await _GrammarCategoryRepository.InsertManyAsync(DefaultGrammarCategories.GrammaraCategories);
            }
        }

        private async Task SeedWordClasses(DataSeedContext context)
        {
            if (await _WordClassRepository.CountAsync() <= 0)
            {
                await _WordClassRepository.InsertManyAsync(DefaultWordClasses.WordClasses);
            }
        }

        private async Task SeedVocabularyTopics(DataSeedContext context)
        {
            if (await _VocabularyTopicRepository.CountAsync() <= 0)
            {
                await _VocabularyTopicRepository.InsertManyAsync(DefaultVocabularyTopics.VocabularyTopics);
            }
        }

        private async Task SeedBaseData(DataSeedContext context)
        {
            if (await _ExamCategoryRepository.CountAsync() <= 0)
                await _ExamCategoryRepository.InsertAsync(DefaultExamCategories.B1, autoSave: true);

            if (await _ExamSkillCategoryRepository.CountAsync() <= 0)
                await _ExamSkillCategoryRepository.InsertManyAsync(new List<ExamSkillCategory>
            {
                DefaultExamSkillCategories.B1_Listening,
                DefaultExamSkillCategories.B1_Reading,
                DefaultExamSkillCategories.B1_Writing,
                DefaultExamSkillCategories.B1_Speaking,
            }, autoSave: true);

            if (await _ExamSkillPartRepository.CountAsync() <= 0)
                await _ExamSkillPartRepository.InsertManyAsync(new List<ExamSkillPart>
            {
                DefaultExamSkillParts.B1_Listening_Part1,
                DefaultExamSkillParts.B1_Listening_Part2,
                DefaultExamSkillParts.B1_Reading_Part1,
                DefaultExamSkillParts.B1_Reading_Part2,
                DefaultExamSkillParts.B1_Reading_Part3,
                DefaultExamSkillParts.B1_Reading_Part4,
                DefaultExamSkillParts.B1_Writing_Part1,
                DefaultExamSkillParts.B1_Writing_Part2,
                DefaultExamSkillParts.B1_Speaking_Part1
            }, autoSave: true);
        }

        private async Task SeedDefaultDirectory(DataSeedContext context)
        {
            if (!await _AppFileRepository.AnyAsync(x => x.Id == AppFileDefaults.RootDirectory.Id))
            {
                await _AppFileRepository.InsertAsync(AppFileDefaults.RootDirectory);
            }

            if (!await _AppFileRepository.AnyAsync(x => x.Id == AppFileDefaults.UserAvatarsDirectory.Id))
            {
                await _AppFileRepository.InsertAsync(AppFileDefaults.UserAvatarsDirectory);
            }
            if (!await _AppFileRepository.AnyAsync(x => x.Id == AppFileDefaults.AppPhotosDirectory.Id))
            {
                await _AppFileRepository.InsertAsync(AppFileDefaults.AppPhotosDirectory);
            }
            if (!await _AppFileRepository.AnyAsync(x => x.Id == AppFileDefaults.AppVideosDirectory.Id))
            {
                await _AppFileRepository.InsertAsync(AppFileDefaults.AppVideosDirectory);
            }
            if (!await _AppFileRepository.AnyAsync(x => x.Id == AppFileDefaults.AppAudiosDirectory.Id))
            {
                await _AppFileRepository.InsertAsync(AppFileDefaults.AppAudiosDirectory);
            }
            if (!await _AppFileRepository.AnyAsync(x => x.Id == AppFileDefaults.AppCommonsDirectory.Id))
            {
                await _AppFileRepository.InsertAsync(AppFileDefaults.AppCommonsDirectory);
            }
            if (!await _AppFileRepository.AnyAsync(x => x.Id == AppFileDefaults.AppDocumentsDirectory.Id))
            {
                await _AppFileRepository.InsertAsync(AppFileDefaults.AppDocumentsDirectory);
            }
        }
    }
}
