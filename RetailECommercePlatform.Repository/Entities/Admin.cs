namespace RetailECommercePlatform.Repository.Entities;

public class Admin: MongoDbEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}