using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Threading;

namespace ASPCoreMVC
{
    public static class ASPCoreMVCModuleExtensionConfigurator
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                ConfigureExistingProperties();
                ConfigureExtraProperties();
            });
        }

        private static void ConfigureExistingProperties()
        {
            /* You can change max lengths for properties of the
             * entities defined in the modules used by your application.
             *
             * Example: Change user and role name max lengths

               IdentityUserConsts.MaxNameLength = 99;
               IdentityRoleConsts.MaxNameLength = 99;

             * Notice: It is not suggested to change property lengths
             * unless you really need it. Go with the standard values wherever possible.
             *
             * If you are using EF Core, you will need to run the add-migration command after your changes.
             */
        }

        private static void ConfigureExtraProperties()
        {
            /* You can configure extra properties for the
             * entities defined in the modules used by your application.
             *
             * This class can be used to define these extra properties
             * with a high level, easy to use API.
             *
             * Example: Add a new property to the user entity of the identity module

               ObjectExtensionManager.Instance.Modules()
                  .ConfigureIdentity(identity =>
                  {
                      identity.ConfigureUser(user =>
                      {
                          user.AddOrUpdateProperty<string>( //property type: string
                              "SocialSecurityNumber", //property name
                              property =>
                              {
                                  //validation rules
                                  property.Attributes.Add(new RequiredAttribute());
                                  property.Attributes.Add(new StringLengthAttribute(64) {MinimumLength = 4});

                                  //...other configurations for this property
                              }
                          );
                      });
                  });

             * See the documentation for more:
             * https://docs.abp.io/en/latest/Module-Entity-Extensions
             */
            ObjectExtensionManager.Instance.Modules().ConfigureIdentity(identity =>
            {
                identity.ConfigureUser(user =>
                {
                    user.AddOrUpdateProperty<string>(
                        AppUsersConsts.DisplayNamePropertyName,
                        options =>
                        {
                            options.Attributes.Add(
                                new StringLengthAttribute(AppUsersConsts.DisplayNameMaxLength)
                            );
                        }
                    );
                    user.AddOrUpdateProperty<string>(
                        AppUsersConsts.PicturePropertyName,
                        options =>
                        {
                            options.Attributes.Add(
                                new StringLengthAttribute(AppUsersConsts.PictureMaxLength)
                            );
                        }
                    );
                    user.AddOrUpdateProperty<string>(
                        AppUsersConsts.NicknamePropertyName,
                        options =>
                        {
                            options.Attributes.Add(
                                new StringLengthAttribute(AppUsersConsts.NicknameMaxLength)
                            );
                        }
                    );
                    user.AddOrUpdateProperty<string>(
                        AppUsersConsts.WebsitePropertyName,
                        options =>
                        {
                            options.Attributes.Add(
                                new StringLengthAttribute(AppUsersConsts.WebsiteMaxLength)
                            );
                        }
                    );
                    user.AddOrUpdateProperty<string>(
                        AppUsersConsts.AddressPropertyName,
                        options =>
                        {
                            options.Attributes.Add(
                                new StringLengthAttribute(AppUsersConsts.AddressMaxLength)
                            );
                        }
                    );
                    user.AddOrUpdateProperty<string>(
                        AppUsersConsts.IdentityCardNumberPropertyName,
                        options =>
                        {
                            options.Attributes.Add(
                                new StringLengthAttribute(AppUsersConsts.IdentityCardNumberMaxLength)
                            );
                        }
                    );
                    user.AddOrUpdateProperty<int>(
                        AppUsersConsts.BirthDay,
                        options =>
                        {

                        }
                    );
                    user.AddOrUpdateProperty<DateTime>(
                        AppUsersConsts.Gender,
                        options =>
                        {

                        }
                    );
                    user.AddOrUpdateProperty<string>(
                         AppUsersConsts.ShortBioPropertyName,
                         options =>
                         {
                             options.Attributes.Add(
                               new StringLengthAttribute(AppUsersConsts.ShortBioMaxLength)
                           );
                         }
                     );
                });
            });
        }
    }
}
