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
    public class RoomTypeConfigurations : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.Property(rt => rt.Name)
                .HasMaxLength(100);

            builder.Property(rt => rt.Description)
                .HasMaxLength(500);   
            
           

        }
    }
}
