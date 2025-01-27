using BidFunctionApp.Models;
using MediatR;

namespace BidFunctionApp.Requests;

public record ProcessBidRequest(Bid BidData) : IRequest<string>;