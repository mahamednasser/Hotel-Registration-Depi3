using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Estra7a.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estra7a.Models.Configurations
{
    public class RoomConfigurations : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.Property(r => r.Name)
                .HasMaxLength(100);

            builder.Property(r => r.Description)
               .HasMaxLength(500);

            builder.Property(rt => rt.PricePerNight)
                .HasColumnType("decimal(18,2)");

        }
    }
}
