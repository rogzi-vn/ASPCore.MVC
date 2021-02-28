using ASPCoreMVC;
using ASPCoreMVC.Common;
using ASPCoreMVC.Helpers.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace ASPCoreMVC.AppUsers
{
    public class AppUserProfileDTO : EntityDto<Guid>
    {
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
        public string Email { get; protected set; }
        public bool EmailConfirmed { get; protected set; }
        public string PhoneNumber { get; protected set; }
        public bool PhoneNumberConfirmed { get; protected set; }
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
