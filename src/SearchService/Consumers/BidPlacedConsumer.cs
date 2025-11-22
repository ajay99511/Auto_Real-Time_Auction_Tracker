using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            Console.WriteLine("--> Consuming Bid Placed Auction");  
            var item = await DB.Find<Item>().OneAsync(context.Message.AuctionId);
            if(item.CurrentHighBid == null ||
            context.Message.Amount>item.CurrentHighBid ||
            context.Message.BidStatus.Contains("Finished"))
            {
                item.CurrentHighBid = context.Message.Amount;
                await item.SaveAsync();
            }
            // throw new NotImplementedException();
        }
    }
}