using Core.Abstractions;
using Core.Entities;
using MediatR;

namespace Core.Features.Queries;

public class GetCustomerQuery : IRequest<Customer>, ICacheableQuery
{
    public int Id { get; set; }
    public bool BypassCache { get; set; }
    public string CacheKey => $"Customer-{Id}";
    public TimeSpan? SlidingExpiration { get; set; }
}