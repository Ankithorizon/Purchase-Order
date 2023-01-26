using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PartTracking.Context.Models.Validator
{
    public class PullQtyAvailableQtyRequireQtyAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;
        private readonly string _comparisonPropertyReq;

        public PullQtyAvailableQtyRequireQtyAttribute(string comparisonProperty, string comparisonPropertyReq)
        {
            _comparisonProperty = comparisonProperty;
            _comparisonPropertyReq = comparisonPropertyReq;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (int)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            var propertyReq = validationContext.ObjectType.GetProperty(_comparisonPropertyReq);

            if (property == null || propertyReq == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = (int)property.GetValue(validationContext.ObjectInstance);
            var comparisonValueReq = (int)propertyReq.GetValue(validationContext.ObjectInstance);

            if (currentValue > comparisonValue || currentValue > comparisonValueReq)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
