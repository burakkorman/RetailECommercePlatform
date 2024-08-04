using MediatR;
using RetailECommercePlatform.Data.ResponseModels.Query.Customer;

namespace RetailECommercePlatform.Data.RequestModels.Query.Customer;

public class GetCustomerByIdRequest : IRequest<GetCustomerByIdQueryResponse>
{
    public Guid Id { get; set; }
}