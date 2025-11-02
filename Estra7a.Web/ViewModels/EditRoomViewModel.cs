using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estra7a.Web.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Estra7a.Web.ViewModels
{
    public class EditRoomViewModel : RoomFormViewModel
    { 
         public int Id { get; set; }

        // just for diplaing
         public string? CurrentCover { get; set; }
         public List<string>? CurrentAdditionalImages { get; set; }


        [AllowedExtension(new[] { ".jpg", ".png", ".jpeg", ".webp" }),
         MaximumSize(5)]
        public IFormFile? BaseImage { get; set; } = default!;




    }
}
