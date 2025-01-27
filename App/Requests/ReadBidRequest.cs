using BidFunctionApp.Models;
using MediatR;

namespace BidFunctionApp.Requests;

public record ReadBidRequest(Guid ReferenceId) : IRequest<Bid>;