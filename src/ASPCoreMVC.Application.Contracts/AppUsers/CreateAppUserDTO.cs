using ASPCoreMVC;
using ASPCoreMVC.Common;
using ASPCoreMVC.Helpers.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.AppUsers
{
    public class CreateAppUserDTO : EntityDto<Guid>
    {
        public Guid? TenantId { get; set; }
        [Required]
        [MaxLength(64)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(64)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Surname { get; set; }
        [Required]
        [MaxLength(AppUsersConsts.DisplayNameMaxLength)]
        [CheckDisplayname(nameof(Name), nameof(Surname))]
        public string DisplayName { get; set; }
        [Required]
        [MaxLength(128)]
        public string Email { get; set; }
        [MaxLength(14)]
        public string PhoneNumber { get; set; }
        [MaxLength(AppUsersConsts.PictureMaxLength)]
        public string Picture { get; set; }
        [MaxLength(AppUsersConsts.NicknameMaxLength)]
        public string Nickname { get; set; }
        [MaxLength(AppUsersConsts.WebsiteMaxLength)]
        public string Website { get; set; }
        [MaxLength(AppUsersConsts.AddressMaxLength)]
        public string Address { get; set; }
        [MaxLength(AppUsersConsts.IdentityCardNumberMaxLength)]
        public string IdentityCardNumber { get; set; }
        public DateTime? BirthDay { get; set; }
        public GenderTypes Gender { get; set; }
        [MaxLength(AppUsersConsts.ShortBioMaxLength)]
        public string ShortBio { get; set; }
    }
}
