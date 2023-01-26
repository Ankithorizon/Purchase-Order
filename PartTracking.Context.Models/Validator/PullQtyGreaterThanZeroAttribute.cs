using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PartTracking.Context.Models.Validator
{
    public class PullQtyGreaterThanZeroAttribute : ValidationAttribute
    {
        public PullQtyGreaterThanZeroAttribute()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (int)value;

            if (currentValue < 1)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
