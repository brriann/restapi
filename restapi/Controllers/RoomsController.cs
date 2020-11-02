using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restapi.Models;
using restapi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restapi.Controllers
{
   [Route("/[controller]")] // equivalent to "/Rooms"
   [ApiController]
   public class RoomsController : ControllerBase
   {
      private readonly IRoomService _roomService;

      public RoomsController(IRoomService roomService)
      {
         _roomService = roomService;
      }

      [HttpGet(Name = nameof(GetRooms))]
      public IActionResult GetRooms()
      {
         throw new NotImplementedException();
      }

      // GET /rooms/{roomId}
      [HttpGet("{roomId}", Name = nameof(GetRoomById))]
      [ProducesResponseType(404)]
      [ProducesResponseType(200)]
      public async Task<ActionResult<Room>> GetRoomById(Guid roomId)
      {
         var room = await _roomService.GetRoomAsync(roomId);
         if (room == null)
         {
            return NotFound();
         }

         return room;
      }
   }
}
