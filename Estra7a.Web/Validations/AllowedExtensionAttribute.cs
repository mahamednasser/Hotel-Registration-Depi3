
namespace Estra7a.Web.Validations
{
    public class AllowedExtensionAttribute : ValidationAttribute
    {
        private readonly string[] _allowdExtensions;
        public AllowedExtensionAttribute(string[] allowdExtensions)
        {
            _allowdExtensions = allowdExtensions;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file is not null)
            {
                var extensions = Path.GetExtension(file.FileName).ToLower();
                if (!_allowdExtensions.Contains(extensions))
                {
                    return new ValidationResult($"Only these extensions are allowed: {string.Join(", ", _allowdExtensions)}");
                }
            }
            else if (value is IEnumerable<IFormFile> files)
            {
                foreach (var f in files)
                {
                    var ext = Path.GetExtension(f.FileName).ToLower();
                    if (!_allowdExtensions.Contains(ext))
                    {
                        return new ValidationResult($"File '{f.FileName}' has an invalid extension. Allowed: {string.Join(", ", _allowdExtensions)}");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
