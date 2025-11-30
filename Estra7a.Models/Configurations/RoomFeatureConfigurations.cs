using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estra7a.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estra7a.Models.Configurations
{
    public class RoomFeatureConfigurations : IEntityTypeConfiguration<RoomFeature>
    {
        public void Configure(EntityTypeBuilder<RoomFeature> builder)
        {
            builder.HasData(
                  new RoomFeature { Id = 2, Name = "Free WiFi", IconPath = "wifi-svgrepo-com.svg" },
                  new RoomFeature { Id = 3, Name = "Sea view", IconPath = "sea-and-sun-svgrepo-com.svg" },
                  new RoomFeature { Id = 4, Name = "Pool view", IconPath = "pool-svgrepo-com.svg" },
                  new RoomFeature { Id = 5, Name = "Smart TV", IconPath = "smart-tv-svgrepo-com.svg" },
                  new RoomFeature { Id = 6, Name = "Air Conditioning", IconPath = "air-conditioning-svgrepo-com.svg" },
                  new RoomFeature { Id = 7, Name = "Minibar", IconPath = "minibar-svgrepo-com.svg" },
                  new RoomFeature { Id = 8, Name = "Electric Kettle", IconPath = "electric-kettle-svgrepo-com.svg" },
                  new RoomFeature { Id = 9, Name = "Microwave", IconPath = "microwave-svgrepo-com.svg" },
                  new RoomFeature { Id = 10, Name = "Coffee Maker", IconPath = "coffee-maker-svgrepo-com.svg" },
                  new RoomFeature { Id = 11, Name = "Balcony", IconPath = "balcony-window-svgrepo-com.svg" },
                  new RoomFeature { Id = 12, Name = "Safety Deposit Box", IconPath = "safety-box-svgrepo-com.svg" },
                  new RoomFeature { Id = 13, Name = "Hairdryer", IconPath = "hairdryer-on-2-svgrepo-com.svg" },
                  new RoomFeature { Id = 14, Name = "Soundproofing", IconPath = "sound-proof-svgrepo-com.svg" },
                  new RoomFeature { Id = 16, Name = "Disability-Friendly", IconPath = "disability-svgrepo-com.svg" },
                  new RoomFeature { Id = 17, Name = "Heating", IconPath = "heating-furnace-svgrepo-com.svg" },
                  new RoomFeature { Id = 18, Name = "Pets Allowed", IconPath = "dog-pet-allowed-hotel-signal-svgrepo-com.svg" },
                  new RoomFeature { Id = 19, Name = "Bathrobe & Slippers", IconPath = "bathrobe-male-svgrepo-com.svg" },
                  new RoomFeature { Id = 20, Name = "Complimentary Toiletries", IconPath = "toiletries-svgrepo-com.svg" },
                  new RoomFeature { Id = 21, Name = "In-room games", IconPath = "games-2-svgrepo-com.svg" },
                  new RoomFeature { Id = 22, Name = "Free parking", IconPath = "parking-svgrepo-com.svg" },
                  new RoomFeature { Id = 23, Name = "Fitness Center", IconPath = "gym-workout-svgrepo-com.svg" }
            );
        }
    }
}
