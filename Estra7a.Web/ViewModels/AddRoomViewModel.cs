using System.ComponentModel.DataAnnotations;
using Estra7a.Web.Validations;

namespace Estra7a.Web.ViewModels
{
    public class AddRoomViewModel : RoomFormViewModel
    {
        


        [Required(ErrorMessage = "Cover Image is Requied")]
        [AllowedExtension(new[] { ".jpg", ".png", ".jpeg", ".webp" }),
            MaximumSize(5)]
        public IFormFile BaseImage { get; set; } = default!;


        [AllowedExtension(new[] { ".jpg", ".png", ".jpeg", ".webp" }),
            MaximumSize(5)] // 5megabyte
        public List<IFormFile> RoomImages { get; set; } = new();

        
     
         
    }
}
