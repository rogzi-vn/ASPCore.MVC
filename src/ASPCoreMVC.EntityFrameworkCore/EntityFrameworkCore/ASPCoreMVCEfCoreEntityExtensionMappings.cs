using ASPCoreMVC.Users;
using Microsoft.EntityFrameworkCore;
using System;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace ASPCoreMVC.EntityFrameworkCore
{
    public static class ASPCoreMVCEfCoreEntityExtensionMappings
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            ASPCoreMVCGlobalFeatureConfigurator.Configure();
            ASPCoreMVCModuleExtensionConfigurator.Configure();

            OneTimeRunner.Run(() =>
            {
                /* You can configure extra properties for the
                 * entities defined in the modules used by your application.
                 *
                 * This class can be used to map these extra properties to table fields in the database.
                 *
                 * USE THIS CLASS ONLY TO CONFIGURE EF CORE RELATED MAPPING.
                 * USE ASPCoreMVCModuleExtensionConfigurator CLASS (in the Domain.Shared project)
                 * FOR A HIGH LEVEL API TO DEFINE EXTRA PROPERTIES TO ENTITIES OF THE USED MODULES
                 *
                 * Example: Map a property to a table field:

                     ObjectExtensionManager.Instance
                         .MapEfCoreProperty<IdentityUser, string>(
                             "MyProperty",
                             (entityBuilder, propertyBuilder) =>
                             {
                                 propertyBuilder.HasMaxLength(128);
                             }
                         );

                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities
                 */
                ObjectExtensionManager.Instance
                .MapEfCoreProperty<IdentityUser, string>(
                    nameof(AppUser.DisplayName),
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(AppUsersConsts.DisplayNameMaxLength);
                    }
                ).MapEfCoreProperty<IdentityUser, string>(
                    nameof(AppUser.Picture),
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(AppUsersConsts.PictureMaxLength);
                    }
                ).MapEfCoreProperty<IdentityUser, string>(
                    nameof(AppUser.Nickname),
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(AppUsersConsts.NicknameMaxLength);
                    }
                ).MapEfCoreProperty<IdentityUser, string>(
                    nameof(AppUser.Website),
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(AppUsersConsts.WebsiteMaxLength);
                    }
                ).MapEfCoreProperty<IdentityUser, string>(
                    nameof(AppUser.Address),
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(AppUsersConsts.AddressMaxLength);
                    }
                ).MapEfCoreProperty<IdentityUser, string>(
                    nameof(AppUser.IdentityCardNumber),
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(AppUsersConsts.IdentityCardNumberMaxLength);
                    }
                ).MapEfCoreProperty<IdentityUser, DateTime>(
                    nameof(AppUser.BirthDay),
                    (entityBuilder, propertyBuilder) =>
                    {
                    }
                ).MapEfCoreProperty<IdentityUser, int>(
                    nameof(AppUser.Gender),
                    (entityBuilder, propertyBuilder) =>
                    {
                    }
                ).MapEfCoreProperty<IdentityUser, string>(
                    nameof(AppUser.ShortBio),
                    (entityBuilder, propertyBuilder) =>
                    {
                        propertyBuilder.HasMaxLength(AppUsersConsts.ShortBioMaxLength);
                    }
                );
            });
        }
    }
}
