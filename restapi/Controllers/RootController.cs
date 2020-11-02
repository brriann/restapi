using Microsoft.AspNetCore.Mvc;
using restapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restapi.Controllers
{
   [Route("/")]
   [ApiController]
   [ApiVersion("1.0")]
   public class RootController : ControllerBase
   {
      [HttpGet(Name = nameof(GetRoot))]
      [ProducesResponseType(200)]
      public IActionResult GetRoot()
      {
         var response = new RootResponse
         {
            Self = Link.To(nameof(GetRoot)),
            Rooms = Link.To(nameof(RoomsController.GetRooms)),
            Info = Link.To(nameof(InfoController.GetInfo))
         };

         return Ok(response);
      }
   }
}
