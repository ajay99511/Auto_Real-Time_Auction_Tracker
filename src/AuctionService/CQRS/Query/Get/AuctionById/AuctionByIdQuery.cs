using AuctionService.Dtos;
using MediatR;

namespace AuctionService.CQRS.Query.Get.AuctionById;
public record AuctionByIdQuery(Guid id) : IRequest<AuctionDto>;