using ASPCoreMVC.Common;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace ASPCoreMVC.AppUsers
{
    public class AppUserDTO : EntityDto<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Roles { get; private set; }
        public bool EmailConfirmed { get; private set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; private set; }
        public string Picture { get; set; }
        public string Nickname { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string IdentityCardNumber { get; set; }
        public DateTime? BirthDay { get; set; }
        public GenderTypes Gender { get; set; }
        public string ShortBio { get; set; }

        public void SetRoles(params string[] roles)
        {
            Roles = new List<string>(roles).JoinAsString(", ");
        }

    }
}
