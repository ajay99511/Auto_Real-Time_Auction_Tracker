using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.Entities;
using Contracts;
using MassTransit;

namespace AuctionService.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly AuctionDbContext _context;
        public AuctionFinishedConsumer(AuctionDbContext auctionDbContext)
        {
            _context = auctionDbContext;
        }
        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
            Console.WriteLine("---> Auction Finished Consumer");
            var auction = await _context.Auctions.FindAsync(context.Message.AuctionId);
            if(context.Message.ItemSold)
            {
                auction.SoldAmount = context.Message.Amount;
                auction.Winner = context.Message.Winner;
            }
            auction.status = auction.SoldAmount>auction.ReservePrice ? 
            Status.Finished : Status.ReserveNotMet;
            await _context.SaveChangesAsync();
        }
    }
}