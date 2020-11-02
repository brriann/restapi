﻿using AutoMapper;
using restapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restapi.Infrastructure
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<RoomEntity, Room>()
            .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate / 100.0m));
         // TODO Url.Link
      }
   }
}
