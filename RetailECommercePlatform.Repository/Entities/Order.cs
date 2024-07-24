namespace RetailECommercePlatform.Repository.Entities;

public class Order : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid CustomerId { get; set; }
}