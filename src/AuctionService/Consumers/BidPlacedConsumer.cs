using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlaced>
    {
        private readonly AuctionDbContext _context;     
        public BidPlacedConsumer(AuctionDbContext context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<BidPlaced> context)
        {
            Console.WriteLine("---> Consuming Bid Placed Consumer");
            var auction = await _context.Auctions.FindAsync(context.Message.AuctionId);

            if(auction.CurrentHighBid == null || context.Message.BidStatus.Contains("Accepted")
            || context.Message.Amount>auction.CurrentHighBid)
            {
                auction.CurrentHighBid = context.Message.Amount;
                await _context.SaveChangesAsync();
            }
            // throw new NotImplementedException();
        }
    }
}