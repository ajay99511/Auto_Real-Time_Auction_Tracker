using AuctionService.Dtos;
using MediatR;

namespace AuctionService.CQRS.Commands.Create;

public record CreateNewAuctionCommand(CreateAuctionDto CreateAuctionDto) : IRequest<AuctionDto>;