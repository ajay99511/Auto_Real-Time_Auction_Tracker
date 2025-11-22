using AuctionService.Dtos;
using MediatR;

namespace AuctionService.CQRS.Query.List.AuctionList;
public record AuctionListQuery(string date) : IRequest<List<AuctionDto>>;