using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estra7a.Services.DTO;
namespace Estra7a.Services.ServiceContracts
{
    public interface IBookingService
    {
        Task<int> CreateBookingAsync(AddBookingDto dto);
        IEnumerable<BookingDto> GetBookingsByUser(string userId);
        BookingDto GetBookingDetails(int id);
    }
}
