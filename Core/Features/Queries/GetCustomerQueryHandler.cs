using Core.Abstractions;
using Core.Entities;
using MediatR;

namespace Core.Features.Queries;

public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, Customer>
{
    private readonly ICustomerService _customerService;
    public GetCustomerQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    
    public async Task<Customer> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = _customerService.GetCustomer(request.Id);
        return customer;
    }
}