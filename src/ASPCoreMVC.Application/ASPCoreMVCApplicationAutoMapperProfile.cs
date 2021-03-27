using ASPCoreMVC.AppFiles;
using ASPCoreMVC.AppUsers;
using ASPCoreMVC.TCUEnglish.AppFiles;
using ASPCoreMVC.TCUEnglish.ExamAnswers;
using ASPCoreMVC.TCUEnglish.ExamCategories;
using ASPCoreMVC.TCUEnglish.ExamCatInstructors;
using ASPCoreMVC.TCUEnglish.ExamDataLibraries;
using ASPCoreMVC.TCUEnglish.ExamLogs;
using ASPCoreMVC.TCUEnglish.ExamQuestionContainers;
using ASPCoreMVC.TCUEnglish.ExamQuestionGroups;
using ASPCoreMVC.TCUEnglish.ExamQuestions;
using ASPCoreMVC.TCUEnglish.ExamSkillCategories;
using ASPCoreMVC.TCUEnglish.ExamSkillParts;
using ASPCoreMVC.TCUEnglish.GrammarCategories;
using ASPCoreMVC.TCUEnglish.Grammars;
using ASPCoreMVC.TCUEnglish.MesGroups;
using ASPCoreMVC.TCUEnglish.MessGroups;
using ASPCoreMVC.TCUEnglish.Notifications;
using ASPCoreMVC.TCUEnglish.SkillCategories;
using ASPCoreMVC.TCUEnglish.SkillParts;
using ASPCoreMVC.TCUEnglish.UserMessages;
using ASPCoreMVC.TCUEnglish.UserNotes;
using ASPCoreMVC.TCUEnglish.Vocabularies;
using ASPCoreMVC.TCUEnglish.VocabularyTopics;
using ASPCoreMVC.TCUEnglish.WordClasses;
using ASPCoreMVC.Users;
using AutoMapper;
using Volo.Abp.Identity;

namespace ASPCoreMVC
{
    public class ASPCoreMVCApplicationAutoMapperProfile : Profile
    {
        public ASPCoreMVCApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<AppFile, AppFileDTO>();
            //CreateMap<IdentityUser, AppCoreUsers>().ReverseMap().Ignore(x => x.ExtraProperties);

            CreateMap<AppUser, AppUserDTO>().ReverseMap();
            CreateMap<AppUser, AppUserProfileDTO>().ReverseMap();
            CreateMap<AppUserProfileDTO, UpdateAppUserProfileDTO>();
            CreateMap<UpdateAppUserProfileDTO, AppUser>().ReverseMap();
            CreateMap<AppUserDTO, CreateAppUserDTO>().ReverseMap();
            CreateMap<AppUser, CreateAppUserDTO>().ReverseMap();
            CreateMap<CreateAppUserDTO, AppUser>().ReverseMap();
            CreateMap<AppUserDTO, AppUserProfileDTO>().ReverseMap();
            CreateMap<CreateAppUserDTO, AppUserProfileDTO>().ReverseMap();
            CreateMap<CreateAppUserDTO, IdentityUser>();
            CreateMap<CreateAppUserDTO, UpdateAppUserProfileDTO>();

            CreateMap<ExamCategory, ExamCategoryDTO>().ReverseMap();
            CreateMap<ExamCategory, ExamCategoryBaseDTO>();
            CreateMap<ExamCategory, CreateUpdateExamCategoryDTO>().ReverseMap();
            CreateMap<ExamCategoryDTO, CreateUpdateExamCategoryDTO>().ReverseMap();

            CreateMap<ExamSkillCategory, SkillCategoryDTO>().ReverseMap();
            CreateMap<ExamSkillCategory, SkillCategoryBaseDTO>();
            CreateMap<ExamSkillCategory, SkillCatMinifyDTO>();
            CreateMap<ExamSkillCategory, CreateUpdateSkillCategoryDTO>().ReverseMap();
            CreateMap<SkillCategoryDTO, CreateUpdateSkillCategoryDTO>().ReverseMap();

            CreateMap<ExamSkillPart, SkillPartDTO>().ReverseMap();
            CreateMap<ExamSkillPart, SkillPartBaseDTO>();
            CreateMap<ExamSkillPart, SkillPartMinifyDTO>();
            CreateMap<ExamSkillPart, CreateUpdateSkillPartDTO>().ReverseMap();
            CreateMap<SkillPartDTO, CreateUpdateSkillPartDTO>().ReverseMap();

            CreateMap<ExamQuestionContainer, ExamQuestionContainerDTO>().ReverseMap();
            CreateMap<ExamQuestion, ExamQuestionDTO>().ReverseMap();
            CreateMap<ExamAnswer, ExamAnswerDTO>().ReverseMap();

            CreateMap<ExamQuestionGroup, QuestionGroupDTO>().ReverseMap();

            CreateMap<Grammar, GrammarDTO>().ReverseMap();
            CreateMap<Grammar, GrammarBaseDTO>();
            CreateMap<Grammar, GrammarSimpify>();

            CreateMap<GrammarCategory, GrammarCategoryDTO>().ReverseMap();
            CreateMap<GrammarCategory, GrammarCategoryBaseDTO>();

            CreateMap<WordClass, WordClassDTO>().ReverseMap();

            CreateMap<VocabularyTopic, VocabularyTopicDTO>().ReverseMap();
            CreateMap<VocabularyTopic, VocabularyTopicBaseDTO>();

            CreateMap<Vocabulary, VocabularyDTO>().ReverseMap();
            CreateMap<VocabularyDTO, VocabularyBaseDTO>();
            CreateMap<Vocabulary, VocabularySearchResultDTO>();
            CreateMap<Vocabulary, VocabularyBaseDTO>();

            CreateMap<UserNote, UserNoteDTO>().ReverseMap();
            CreateMap<UserNote, UserNoteBaseDTO>();

            CreateMap<ExamCatInstructor, ExamCatInstructDTO>().ReverseMap();
            CreateMap<CreateUpdateExamCatInstructDTO, ExamCatInstructor>();

            CreateMap<MessGroup, MessGroupDTO>().ReverseMap();

            CreateMap<UserMessage, UserMessageDTO>().ReverseMap();

            CreateMap<ExamLog, ExamLogDTO>().ReverseMap();
            CreateMap<ExamLogDTO, ExamLogBaseDTO>();
            CreateMap<ExamLog, ExamLogBaseDTO>();

            CreateMap<Notification, NotificationDTO>().ReverseMap();
        }
    }
}
