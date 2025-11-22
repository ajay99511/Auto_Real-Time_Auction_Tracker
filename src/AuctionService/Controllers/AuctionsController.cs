using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.CQRS.Commands.Create;
using AuctionService.CQRS.Query.Get.AuctionById;
using AuctionService.CQRS.Query.List.AuctionList;
using AuctionService.Data;
using AuctionService.Dtos;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionsController: ControllerBase
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMediator mediator1;
        private readonly IValidator<CreateAuctionDto> validator;
        public AuctionsController(AuctionDbContext auctionDbContext,IMapper mapper,IPublishEndpoint publishEndpoint
        ,IMediator mediator,IValidator<CreateAuctionDto> _validator)
        {
            _context = auctionDbContext;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            mediator1 = mediator;
            validator = _validator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> Getall(string date)
        {
            var auctions = await mediator1.Send(new AuctionListQuery(date));
            return Ok(auctions);

            // var query = _context.Auctions.OrderBy(x=>x.item.Make).AsQueryable();
            // if(!string.IsNullOrEmpty(date))
            // {
            //     query= query.Where(x=>x.UpdatedAt
            //     .CompareTo(DateTime.Parse(date).ToUniversalTime())>0);
            // }

            // return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();

            // var auctions = await _context.Auctions
            // .Include(x=>x.item)
            // .OrderBy(x=>x.item.Make)
            // .ToListAsync();
            // return _mapper.Map<List<AuctionDto>>(auctions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            return await mediator1.Send(new AuctionByIdQuery(id));
            // var auction = await _context.Auctions
            // .Include(x=>x.item)
            // .FirstOrDefaultAsync(x=>x.Id == id);
            // if(auction == null)
            // {
            //     return NotFound();
            // }
            // return _mapper.Map<AuctionDto>(auction);
        }

        // [Authorize]
        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
        {
            var validationResult = await validator.ValidateAsync(createAuctionDto);
            if(!validationResult.IsValid) return BadRequest(Results.ValidationProblem(validationResult.ToDictionary()));
            var newAuction = await mediator1.Send(new CreateNewAuctionCommand(createAuctionDto));
            return newAuction;

            // var auction = _mapper.Map<Auction>(createAuctionDto);
            // auction.Seller = User.Identity.Name;
            // _context.Auctions.Add(auction);
            // var newAuction = _mapper.Map<AuctionDto>(auction);
            // await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));
            // var result = await _context.SaveChangesAsync()>0;
            // if(!result) return BadRequest(new ProblemDetails{Title="Cannot create object"});
            // if(!result) return BadRequest("Could not save to the database");
            // return _mapper.Map<AuctionDto>(auction);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateDto)
        {
            var auction = await _context.Auctions
            .Include(x=>x.item)
            .FirstOrDefaultAsync(x=>x.Id==id);
            if(auction == null)
            {
                return NotFound();
            }
            if(auction.Seller != User.Identity.Name)
            {
                return Forbid();
            }
            auction.item.Model = updateDto.Model ?? auction.item.Model;
            auction.item.Make = updateDto.Make ?? auction.item.Make;
            auction.item.Year = updateDto.Year ?? auction.item.Year;
            auction.item.Mileage = updateDto.Mileage ?? auction.item.Mileage;
            auction.item.Color = updateDto.Color ?? auction.item.Color;
            
            await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction));
            var result = await _context.SaveChangesAsync()>0;
            if(result) return Ok();
            return BadRequest("Cannot Update the Auction Data");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if(auction == null) return NotFound();
            if(auction.Seller != User.Identity.Name) return Forbid();
            _context.Auctions.Remove(auction);
            await _publishEndpoint.Publish(new AuctionDeleted{Id = auction.Id.ToString()});
            var result = await _context.SaveChangesAsync()>0;
            if(result) return Ok();
            return BadRequest(new ProblemDetails{Title="Error while deleting the items"});
            //Delete Request
        }
    }
}