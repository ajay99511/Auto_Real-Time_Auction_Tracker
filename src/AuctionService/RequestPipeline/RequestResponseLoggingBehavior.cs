using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace AuctionService.RequestPipeline
{
    public class RequestResponseLoggingBehavior<TRequest,TResponse> 
    (ILogger<RequestResponseLoggingBehavior<TRequest,TResponse>> logger)
    : IPipelineBehavior<TRequest,TResponse> where TRequest: class
    {
        public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
        {
            var guid = Guid.NewGuid();
            var requestJson = JsonSerializer.Serialize(request);
            logger.LogInformation("Handling the request {guid} : {requestJson}", guid, requestJson);
            var response = await next();
            var responseJson = JsonSerializer.Serialize(response);
            logger.LogInformation("{responseJson}", responseJson);
            return response;
        }
    }
}