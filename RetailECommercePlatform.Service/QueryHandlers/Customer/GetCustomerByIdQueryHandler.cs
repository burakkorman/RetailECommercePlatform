using MediatR;
using RetailECommercePlatform.Data.RequestModels.Query.Customer;
using RetailECommercePlatform.Data.ResponseModels.Query.Customer;

namespace RetailECommercePlatform.Service.QueryHandlers.Customer;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdRequest, GetCustomerByIdQueryResponse>
{
    public Task<GetCustomerByIdQueryResponse> Handle(GetCustomerByIdRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new GetCustomerByIdQueryResponse()
        {
            Name = "burak"
        });
    }
}