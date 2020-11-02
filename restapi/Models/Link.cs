using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace restapi.Models
{
   public class Link
   {
      public const string GET_METHOD = "GET";

      public static Link To(string routeName, object routeValues = null)
         => new Link
         {
            RouteName = routeName,
            RouteValues = routeValues,
            Method = GET_METHOD,
            Relations = null
         };

      [JsonProperty(Order = -4)]
      public string Href { get; set; }

      [JsonProperty(Order = -3, PropertyName = "rel", NullValueHandling = NullValueHandling.Ignore)]
      public string[] Relations { get; set; }

      [JsonProperty(Order = -2, DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore)]
      [DefaultValue(GET_METHOD)]
      public string Method { get; set; }

      // stores route name before rewrite by LinkReWritingFilter
      [JsonIgnore]
      public string RouteName { get; set; }

      // stores route parameters before rewrite by LinkReWritingFilter
      [JsonIgnore]
      public object RouteValues { get; set; }
   }
}
