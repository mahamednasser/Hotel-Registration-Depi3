namespace Estra7a.Web.Validations
{
    public class DeleteBaseAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var viewModel = validationContext.ObjectInstance as EditRoomViewModel;
            if (viewModel == null)
                return ValidationResult.Success;


            if (viewModel.DeleteCover == true && value == null)
            {
                return new ValidationResult(
                    "You must upload a new cover image when deleting the existing one."
                );
            }

            return ValidationResult.Success;
        }

    }
}
