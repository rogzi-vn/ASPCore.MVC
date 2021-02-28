using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ASPCoreMVC.Helpers.CustomAttributes
{
    // https://github.com/microsoft/referencesource/blob/master/System.ComponentModel.DataAnnotations/DataAnnotations/CompareAttribute.cs
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [PropertyType(typeof(string), typeof(String))]
    public class CheckDisplaynameAttribute : ValidationAttribute
    {
        public string FirstNameProperty { get; private set; }
        public string LastNameProperty { get; private set; }

        public string FirstNamePropertyNameDisplayName { get; internal set; }
        public string LastNamePropertyNameDisplayName { get; internal set; }
        public CheckDisplaynameAttribute(string firstNamePropertyName, string lastNamePropertyNames)
        {
            if (firstNamePropertyName == null)
            {
                throw new ArgumentNullException("firstNamePropertyName");
            }
            if (lastNamePropertyNames == null)
            {
                throw new ArgumentNullException("lastNamePropertyNames");
            }
            FirstNameProperty = firstNamePropertyName;
            LastNameProperty = lastNamePropertyNames;
        }
        public override bool RequiresValidationContext
        {
            get
            {
                return true;
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var firstName = GetValue(validationContext, FirstNameProperty);
            var lastName = GetValue(validationContext, LastNameProperty);
            var displayNames = new List<string> {
                $"{firstName}".Trim(),
                $"{firstName} {lastName}".Trim(),
                $"{lastName} {firstName}".Trim(),
                $"{lastName}".Trim(),
            }.Where(x => !x.IsNullOrEmpty() && !x.IsNullOrWhiteSpace())
            .Distinct();

            if (!displayNames.Contains(value))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }

        private string GetValue(ValidationContext validationContext, string propertyName)
        {
            PropertyInfo propertyInfo = validationContext.ObjectType.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new Exception(string.Format(CultureInfo.CurrentCulture, "Unknow property name {0}", propertyName));
            }
            return propertyInfo.GetValue(validationContext.ObjectInstance, null).ToString()
                .Split(" ")
                .Where(x => !x.IsNullOrWhiteSpace() && !x.IsNullOrEmpty())
                .ToList()
                .JoinAsString(" ")
                .ToPascalCase();

        }
    }
}
