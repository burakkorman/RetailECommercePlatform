namespace RetailECommercePlatform.Repository.Entities;

public class Customer : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Address { get; set; }
}