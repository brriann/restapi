using AutoMapper;
using Microsoft.EntityFrameworkCore;
using restapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restapi.Services
{
   public class DefaultRoomService : IRoomService
   {
      private readonly HotelApiDbContext _context;
      private readonly IMapper _mapper;

      public DefaultRoomService(HotelApiDbContext context, IMapper mapper)
      {
         _context = context;
         _mapper = mapper;
      }
      public async Task<Room> GetRoomAsync(Guid id)
      {
         var entity = await _context.Rooms
            .SingleOrDefaultAsync(x => x.Id == id);

         if (entity == null)
         {
            return null;
         }

         return _mapper.Map<Room>(entity);

         return new Room
         {
            Href = null, //Url.Link(nameof(GetRoomById), new { roomId = entity.Id }),
            Name = entity.Name,
            Rate = entity.Rate / 100.0m
         };
      }
   }
}
