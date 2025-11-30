using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Estra7a.Services.DTO
{
    public class EditRoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Area { get; set; }
        public int NumberOfRooms { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public int RoomtypeId { get; set; }


        
        public List<int> SelectedFeatureIds { get; set; } = new(); 

      
        public List<int> CurrentFeatures { get; set; } = new();


        public string? CurrentCoverImageName { get; set; }
        public bool DeleteCover { get; set; }
        public string? NewCoverImageName { get; set; }

        public List<ExistingImageDto> CurrentAdditionalImages { get; set; } = new();
        public List<int> AdditionalImagesToDelete { get; set; } = new();
        public List<string> NewAdditionalImages { get; set; } = new();

    }

    public class ExistingImageDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
    }


}