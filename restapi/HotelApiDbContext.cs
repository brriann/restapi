using Microsoft.EntityFrameworkCore;
using restapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restapi
{
   public class HotelApiDbContext : DbContext
   {
      public HotelApiDbContext(DbContextOptions options)
         : base(options) { }

      public DbSet<RoomEntity> Rooms { get; set; }
   }
}
