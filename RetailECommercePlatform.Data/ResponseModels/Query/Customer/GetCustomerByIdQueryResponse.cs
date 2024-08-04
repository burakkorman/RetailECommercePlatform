namespace RetailECommercePlatform.Data.ResponseModels.Query.Customer;

public class GetCustomerByIdQueryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
}