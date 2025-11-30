using Estra7a.Services.DTO;
using Estra7a.Web.Validations;


namespace Estra7a.Web.ViewModels
{
    public class EditRoomViewModel : RoomFormViewModel
    {
        public int Id { get; set; }

        public string? CurrentBaseImageName { get; set; }

        public bool DeleteCover { get; set; }

        [AllowedExtension(new[] { ".jpg", ".png", ".jpeg", ".webp" })]
        [MaximumSize(5), DeleteBase]
        public IFormFile? NewCoverImage { get; set; }


        public List<ExistingImageDto> CurrentAdditionalImages { get; set; } = new();


        [AllowedExtension(new[] { ".jpg", ".png", ".jpeg", ".webp" })]
        [MaximumSize(5)]
        public List<IFormFile>? NewAdditionalImages { get; set; }

        public List<int> AdditionalImagesToDelete { get; set; } = new();


    }

    // delete it
    public class ExistingImage
    {
        public int Id { get; set; }
        public string FileName { get; set; }
    }
}
