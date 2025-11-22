using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.Dtos;
using AuctionService.Entities;
using AutoMapper;
using Contracts;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AuctionService.CQRS.Commands.Create
{
    public class CreateNewAuctionHandler(AuctionDbContext auctionDbContext,IMapper _mapper,
    IHttpContextAccessor httpContextAccessor,IPublishEndpoint _publishEndpoint) : IRequestHandler<CreateNewAuctionCommand, AuctionDto>
    {
        public async Task<AuctionDto> Handle(CreateNewAuctionCommand request, CancellationToken cancellationToken
        )
        {
            var auction = _mapper.Map<Auction>(request.CreateAuctionDto);
            auction.Seller = httpContextAccessor.HttpContext.User.Identity.Name;
            auctionDbContext.Auctions.Add(auction);
            var newAuction = _mapper.Map<AuctionDto>(auction);
            await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));
            var result = await auctionDbContext.SaveChangesAsync()>0;
            if(!result) throw new Exception("Could not save to the database");
            return newAuction;
            // throw new NotImplementedException();
        }
    }
}