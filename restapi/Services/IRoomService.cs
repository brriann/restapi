using restapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restapi.Services
{
   public interface IRoomService
   {
      Task<Room> GetRoomAsync(Guid id);
   }
}
