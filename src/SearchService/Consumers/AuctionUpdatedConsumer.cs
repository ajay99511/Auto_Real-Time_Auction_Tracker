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
    public class AuctionUpdatedConsumer(IMapper mapper) : IConsumer<AuctionUpdated>
    {
        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            // throw new NotImplementedException();

            Console.WriteLine("---> Consuming the Auction UPdate:"+context.Message.Id);
            var Item = mapper.Map<Item>(context.Message);
            var result = await DB.Update<Item>()
            .Match(x=>x.ID == context.Message.Id)
            .ModifyOnly(x=>new {
                x.Color,
                x.Model,
                x.Make,
                x.Mileage,
                x.Year
            },Item).ExecuteAsync();
            if (!result.IsAcknowledged)
            {
                throw new MessageException(typeof(AuctionDeleted),"Problem With MongoDb");
            }
        }
    }
}