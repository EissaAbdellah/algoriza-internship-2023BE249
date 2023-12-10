using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Core.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {

        private readonly int _maxFileSize;
        private readonly int _maxFileSizeInMB;

        public MaxFileSizeAttribute(int maxFileSize, int maxFileSizeInMB)
        {
            _maxFileSize = maxFileSize;
            _maxFileSizeInMB = maxFileSizeInMB;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file is not null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult($"Maximum allowed size is {_maxFileSizeInMB} MB");
                }
            }

            return ValidationResult.Success;
        }



    }
}
