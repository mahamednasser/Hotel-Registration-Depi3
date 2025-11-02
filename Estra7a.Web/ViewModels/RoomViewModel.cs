namespace Estra7a.Web.ViewModels
{
    public class RoomViewModel
    {
       
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal PricePerNight { get; set; }
            public string? Description { get; set; }
            public double Area { get; set; }
            public int Capacity { get; set; }
            public string BaseImageUrl { get; set; } = string.Empty;
            public int NumberOfRooms { get; set; }
            public double? Rate {  get; set; }   
            public string? RoomTypeDescription {  get; set; }
             public List<string> RoomImages { get; set; } = new();

            public int NumberOfAvailableRooms { get; set; }
            public string RoomTypeName { get; set; } = string.Empty;
            public bool IsFavorite { get; set; } = false;
    }
}
