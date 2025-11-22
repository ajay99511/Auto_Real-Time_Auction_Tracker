using System.Net;
using MassTransit;
using Polly;
using Polly.Extensions.Http;
using SearchService.Consumers;
using SearchService.Data;
using ZstdSharp.Unsafe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x=>{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.AddConsumer<AuctionDeletedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search",false));
    x.UsingRabbitMq((context,cfg)=>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"],"/",host=>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username","guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password","guest"));
        });
        
        cfg.ConfigureEndpoints(context);
        cfg.UseMessageRetry(r=>r.Interval(3,5));

        // cfg.ReceiveEndpoint("search-auction-created",e=>{
        //     e.UseMessageRetry(r=>r.Interval(3,5));
        //     e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        // });
    });
});
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.
// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Lifetime.ApplicationStarted.Register(async()=>
{
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
});


app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()=> HttpPolicyExtensions.HandleTransientHttpError()
.OrResult(msg=>msg.StatusCode == HttpStatusCode.NotFound)
.WaitAndRetryForeverAsync(_=>TimeSpan.FromSeconds(3));