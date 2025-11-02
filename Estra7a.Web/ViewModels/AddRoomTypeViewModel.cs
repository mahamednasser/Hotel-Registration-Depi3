using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Estra7a.Web.ViewModels
{
    public class AddRoomTypeViewModel
    {

        [Required]
        public string RoomTypeName { get; set; }
        [Required]
        public string RoomTypeDescription {  get; set; }
    }
}
