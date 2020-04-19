using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectroEshop.CustomCode
{
    //public class RequiredIfAttribute
    //{
    //}
    public enum Operator
    {
        EqualTo,
        NotEqualTo
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RequiredIfAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessageFormatString = "The {0} field is required.";

        private readonly string _dependentPropertyName;
        private readonly Operator _dependentPropertyComparison;
        private readonly object _dependentPropertyValue;


        public RequiredIfAttribute(string dependentPropertyName, Operator dependentPropertyComparison, object dependentPropertyValue)
        {
            _dependentPropertyName = dependentPropertyName;
            _dependentPropertyComparison = dependentPropertyComparison;
            _dependentPropertyValue = dependentPropertyValue;

            ErrorMessage = DefaultErrorMessageFormatString;
        }

        private bool ValidateDependentProperty(object actualPropertyValue)
        {
            switch (_dependentPropertyComparison)
            {
                case Operator.NotEqualTo:
                    return actualPropertyValue == null ? _dependentPropertyValue != null : !actualPropertyValue.Equals(_dependentPropertyValue);
                default:
                    return actualPropertyValue == null ? _dependentPropertyValue == null : actualPropertyValue.Equals(_dependentPropertyValue);
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                var dependentProperty = validationContext.ObjectInstance.GetType().GetProperty(_dependentPropertyName);

                Object dependentPropertyValue = null;
                if (dependentProperty != null)
                {
                    dependentPropertyValue = dependentProperty.GetValue(validationContext.ObjectInstance, null); //new object[] { }
                }

                if (ValidateDependentProperty(dependentPropertyValue))
                {
                    return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }
    }
}