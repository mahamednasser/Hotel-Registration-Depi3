using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Estra7a.Web.ViewModels
{
    public class RoomFormViewModel
    {
        [Required(ErrorMessage = "Room Name is Requied")]
        public string? Name { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "Description can't be longer than 300 characters.")]
        [Required]
        public string? Description { get; set; } = string.Empty;


        [Required]
        [Range(50, 500, ErrorMessage = "Area must be between 50 and 500 square meters.")]
        public double Area { get; set; }


        [Required(ErrorMessage = "Number of rooms is required.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Number of rooms must be greater than 0.")]
        [Display(Name = "Number Of Rooms")]
        public int NumberOfRooms { get; set; }

        [Required(ErrorMessage = "Capacity of rooms is required.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Number of rooms must be greater than 0.")]
        public int Capacity { get; set; }


        [Required(ErrorMessage = "Price per night is required.")]
        [Range(500, double.MaxValue, ErrorMessage = "Price per night must be at least 500")]
        [Display(Name = "Price Per Night")]

        public decimal PricePerNight { get; set; }


        public int RoomtypeId { get; set; }


        [BindNever]
        public IEnumerable<SelectListItem> RoomTypes { get; set; } = Enumerable.Empty<SelectListItem>();
        // just for displaing
    }
}
