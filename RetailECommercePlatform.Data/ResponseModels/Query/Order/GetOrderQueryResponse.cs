namespace RetailECommercePlatform.Data.ResponseModels.Query.Order;

public class GetOrderQueryResponse
{
    public string Id { get; set; }
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public List<OrderProductModel> Products { get; set; }
    public decimal TotalPrice { get; set; }
    public string State { get; set; }
}

public class OrderProductModel
{
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}