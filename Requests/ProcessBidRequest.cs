using MediatR;

namespace BidFunctionApp.Requests;

public record ProcessBidRequest(string BidData) : IRequest<string>;