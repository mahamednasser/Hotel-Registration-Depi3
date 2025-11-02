using System.ComponentModel.DataAnnotations;
namespace Estra7a.Web.Validations
{
    internal class MaximumSizeAttribute : ValidationAttribute
    {
        private readonly long _maxFileSizeInBytes;
        public MaximumSizeAttribute(long maxFileSizeInBytes )
        {

            _maxFileSizeInBytes = maxFileSizeInBytes * 1024 * 1024;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file is not null)
            {
                if (file.Length > _maxFileSizeInBytes)
                {
                    return new ValidationResult($"File size must be less than {_maxFileSizeInBytes / 1024 / 1024} MB.");
                }

            }
            if (file is IEnumerable<IFormFile> files) 
            {
                foreach (var _file in files)
                {
                    if (_file.Length > _maxFileSizeInBytes) 
                    {
                        return new ValidationResult($"File '{file.FileName}' size must be less than {_maxFileSizeInBytes / 1024 / 1024} MB.");
                    }

                }
            }
            return ValidationResult.Success;
        }
    }
}
