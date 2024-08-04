using MongoDB.Bson;

namespace RetailECommercePlatform.Data.ResponseModels.Query.Product;

public class GetProductQueryResponse
{
    public string Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}