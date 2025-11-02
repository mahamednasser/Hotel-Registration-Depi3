using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estra7a.Services.Services
{
    public class BackgroundBookingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BackgroundBookingService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected  override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CleanExpiredBookings();


               // await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task CleanExpiredBookings()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var today = DateTime.Today;

                
                var expiredBookings = unitOfWork.Booking
                    .GetAll(b => b.CheckOutDate.Date <= today && b.IsActive)
                    .ToList();

                if (expiredBookings.Any())
                {
                    foreach (var booking in expiredBookings)
                    {
                        var room = unitOfWork.Room.GetById(r => r.RoomId == booking.RoomId);
                        if (room != null)
                        {                          
                            room.NumberOfAvailableRooms += booking.room_count;
                        }

                        booking.IsActive = false;
                    }

                    await unitOfWork.SaveAsync();

                }
            }
        }
    }
}
