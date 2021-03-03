using ASPCoreMVC.Web.Models;
using AutoMapper;
using Volo.Abp.Identity;

namespace ASPCoreMVC.Web
{
    public class ASPCoreMVCWebAutoMapperProfile : Profile
    {
        public ASPCoreMVCWebAutoMapperProfile()
        {
            //Define your AutoMapper configuration here for the Web project.
            CreateMap<IdentityRoleDto, IdentityRoleCreateOrUpdateDtoBase>();
            CreateMap<IdentityRoleDto, IdentityRoleUpdateDto>();
        }
    }
}
