using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.CQRS.Query.Get.AuctionById
{
    public class AuctionByIdHandler(AuctionDbContext auctionDbContext,IMapper _mapper) : IRequestHandler<AuctionByIdQuery, AuctionDto>
    {
        public async Task<AuctionDto> Handle(AuctionByIdQuery request, CancellationToken cancellationToken)
        {
            var auction = await auctionDbContext.Auctions
            .Include(x=>x.item)
            .FirstOrDefaultAsync(x=>x.Id == request.id);
            if(auction == null)
            {
                throw new KeyNotFoundException("Auction is not existed");
            }
            return _mapper.Map<AuctionDto>(auction);
            // throw new NotImplementedException();
        }
    }
}