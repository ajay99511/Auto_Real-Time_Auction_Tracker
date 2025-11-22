using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.CQRS.Query.List.AuctionList;
using AuctionService.Data;
using AuctionService.Dtos;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.CQRS.Query.List
{
    public class AuctionListQueryHandler(AuctionDbContext auctionDbContext,IMapper _mapper) :
    IRequestHandler<AuctionListQuery, List<AuctionDto>>
    {
        public async Task<List<AuctionDto>> Handle(AuctionListQuery request, CancellationToken cancellationToken)
        {
             var query = auctionDbContext.Auctions.OrderBy(x=>x.item.Make).AsQueryable();
            if(!string.IsNullOrEmpty(request.date))
            {
                query= query.Where(x=>x.UpdatedAt
                .CompareTo(DateTime.Parse(request.date).ToUniversalTime())>0);
            }
            return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}