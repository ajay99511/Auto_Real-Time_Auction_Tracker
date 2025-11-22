using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Dtos;
using FluentValidation;
using MediatR;

namespace AuctionService.RequestPipeline
{
    public class DataValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, 
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(next);

            if(validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    validators.Select(x=>x.ValidateAsync(context,cancellationToken))
                ).ConfigureAwait(false);

                var failures = validationResults.
                Where(x=>x.Errors.Count()>0)
                .SelectMany(x=>x.Errors)
                .ToList();

                if(failures.Count()>0)
                {
                    throw new ValidationException(failures);
                }
            }
            return await next().ConfigureAwait(false);
            // throw new NotImplementedException();
        }
    }
}