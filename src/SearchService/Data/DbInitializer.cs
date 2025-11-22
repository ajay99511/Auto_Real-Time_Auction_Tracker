using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Data
{
    public class DbInitializer
    {
        public static async Task InitDb(WebApplication app)
        {
            await DB.InitAsync("SearchDb",MongoClientSettings
            .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

            await DB.Index<Item>()
            .Key(x=>x.Model,KeyType.Text)
            .Key(x=>x.Make,KeyType.Text)
            .Key(x=>x.Color,KeyType.Text)
            .CreateAsync();

            var count = await DB.CountAsync<Item>();

            using var scope = app.Services.CreateScope();
            var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

            var items = await httpClient.GetItemsForSearchDb();
            Console.WriteLine(items.Count+" are added to the database");
            if(items.Count>0)
            {
                await DB.SaveAsync(items);
            }

            // if(count == 0)
            // {
            //     Console.WriteLine("No Data inside the Database");
            //     var itemdata = await File.ReadAllTextAsync("Data/auctions.json");
            //     var options = new JsonSerializerOptions{PropertyNameCaseInsensitive= true};

            //     var items = JsonSerializer.Deserialize<List<Item>>(itemdata,options);
            //     await DB.SaveAsync(items);
            // }
        }
    }
}