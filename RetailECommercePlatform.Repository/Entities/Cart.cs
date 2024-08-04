namespace RetailECommercePlatform.Repository.Entities;

public class Cart : MongoDbEntity
{
    public string CustomerId { get; set; }
    public string ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}