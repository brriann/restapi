using Microsoft.AspNetCore.Mvc;
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
      [HttpGet(Name = nameof(GetRooms))]
      public IActionResult GetRooms()
      {
         throw new NotImplementedException();
      }
   }
}
