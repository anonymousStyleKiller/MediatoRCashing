using Core.Abstractions;
using Core.Entities;
using MediatR;

namespace Core.Features.Queries;

public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, List<Customer>>
{
    private readonly ICustomerService customerService;
    public GetCustomerListQueryHandler(ICustomerService customerService)
    {
        this.customerService = customerService;
    }
    public async Task<List<Customer>> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
    {
        var cutomers = customerService.GetCustomerList();
        return cutomers.ToList();
    }
}