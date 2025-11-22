using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Dtos;
using AuctionService.Entities;
using FluentValidation;
using MassTransit.DependencyInjection.Registration;

namespace AuctionService.Validators
{
    public class CreateAuctionValidator : AbstractValidator<CreateAuctionDto>
    {
        public CreateAuctionValidator()
        {
            RuleFor(x=>x.ReservePrice).NotEmpty()
            .GreaterThan(5000).WithMessage("Reserve price should not be empty");
            //Custom Validation Rule using Fluent Validation
            RuleFor(x=>x.Make).Must(isValid).WithMessage("Make should be either alphabet or number");
        }
        //Private method to validate car make
        private bool isValid(string Make)
        {
            return Make.All(c=>char.IsLetterOrDigit(c)||c == '.');
        }
    }
}