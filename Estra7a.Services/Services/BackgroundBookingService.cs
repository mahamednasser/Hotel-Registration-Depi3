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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;

                var nextRun = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);


                if (now > nextRun)
                    nextRun = nextRun.AddDays(1);


                var delay = nextRun - now;

                await Task.Delay(delay, stoppingToken);


                await CleanExpiredBookings();

                //Console.WriteLine($"Background task ran at {DateTime.Now}");
            }
        }


        private async Task CleanExpiredBookings()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();



                var expiredBookingsList = unitOfWork.Booking
      .GetAll(b => b.CheckOutDate <= DateTime.UtcNow && b.IsActive)
      .ToList();

                var expiredBookings = expiredBookingsList
                    .Select(x => unitOfWork.Booking.GetById(b => b.BookingId == x.BookingId, tracked: true))
                    .ToList();




                if (expiredBookings.Any())
                {
                    foreach (var booking in expiredBookings)
                    {
                        var room = unitOfWork.Room.GetById(r => r.RoomId == booking.RoomId, tracked: true);
                        if (room != null)
                        {
                            room.NumberOfAvailableRooms += booking.room_count;
                        }

                        booking.IsActive = false;
                        //Console.WriteLine($"Searching expired at {DateTime.Now}");
                        //Console.WriteLine($"Expired count = {expiredBookings.Count}");

                    }
                    unitOfWork.save();
                    await unitOfWork.SaveAsync();

                }
            }
        }
    }
}