using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Dtos;
using AuctionService.Entities;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Auction,AuctionDto>().IncludeMembers(x=>x.item);
            CreateMap<Item,AuctionDto>();
            
            CreateMap<CreateAuctionDto,Auction>()
            .ForMember(d=>d.item,o=>o.MapFrom(s=>s));
            CreateMap<CreateAuctionDto,Item>();

            CreateMap<AuctionDto,AuctionCreated>();
            CreateMap<Auction,AuctionUpdated>().IncludeMembers(x=>x.item);
            CreateMap<Item,AuctionUpdated>();
        }
    }
}