namespace RetailECommercePlatform.Repository.Entities;

public class Order : MongoDbEntity
{
    public string CustomerId { get; set; }
    public List<Cart> OrderItems { get; set; }
    public decimal TotalPrice { get; set; }
    public int State { get; set; }
}