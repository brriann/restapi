using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restapi.Models
{
   public abstract class Resource
   {
      [JsonProperty(Order = -2)] // -2 : JSON.NET, this property will be at top of all serialized responses
      public string Href { get; set; } // self-referential link, replaces Id property, this is RESTful!
   }
}
