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
    public class AuctionCreatedConsumer(IMapper mapper) : IConsumer<AuctionCreated>
    {
        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            // throw new NotImplementedException();
            Console.WriteLine("---> Auction Created:",context.Message.Id);

            var item = mapper.Map<Item>(context.Message);
            if(item.Model == "Foo") throw new ArgumentException("cannot sell cars");
            await item.SaveAsync();
        }
    }
}