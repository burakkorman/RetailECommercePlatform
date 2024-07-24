namespace RetailECommercePlatform.Repository.Entities;

public class Product : BaseEntity
{
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockCount { get; set; }
}