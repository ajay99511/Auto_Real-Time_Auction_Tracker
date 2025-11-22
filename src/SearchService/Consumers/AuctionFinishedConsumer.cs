using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly IMapper mapper;
        public AuctionFinishedConsumer(IMapper _mapper)
        {
            mapper = _mapper;
        }
        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            Console.WriteLine("--> Consuming Bid Placed Auction");  
            var item = await DB.Find<Item>().OneAsync(context.Message.AuctionId);
            if(context.Message.ItemSold)
            {
                item.Winner = context.Message.Winner;
                item.SoldAmount = (int)context.Message.Amount;
            }
            item.Status = "Finished";
            await item.SaveAsync();
            // throw new NotImplementedException();
        }
    }
}