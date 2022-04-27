using Core.Abstractions;
using Core.Entities;
using MediatR;

namespace Core.Features.Queries;

public class GetCustomerListQuery  : IRequest<List<Customer>>, ICacheableQuery
{
    public int Id { get; set; }
    public bool BypassCache { get; set; }
    public string CacheKey => $"CustomerList";
    public TimeSpan? SlidingExpiration { get; set; }
}