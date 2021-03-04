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
        public string Email { get; set; }
        public bool EmailConfirmed { get; private set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; private set; }
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

        public bool CheckChange(AppUserProfileDTO dest)
        {
            if (dest.Name != Name)
                return true;
            if (dest.Surname != Surname)
                return true;
            if (dest.DisplayName != DisplayName)
                return true;
            if (dest.Email != Email)
                return true;
            if (dest.PhoneNumber != PhoneNumber)
                return true;
            if (dest.Picture != Picture)
                return true;
            if (dest.Nickname != Nickname)
                return true;
            if (dest.Website != Website)
                return true;
            if (dest.Address != Address)
                return true;
            if (dest.IdentityCardNumber != IdentityCardNumber)
                return true;
            if (dest.BirthDay != BirthDay)
                return true;
            if (dest.Gender != Gender)
                return true;
            if (dest.ShortBio != ShortBio)
                return true;
            return false;
        }

    }
}
