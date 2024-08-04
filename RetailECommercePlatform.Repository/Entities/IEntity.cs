namespace RetailECommercePlatform.Repository.Entities;

public interface IEntity
{
}

public interface IEntity<out TKey> : IEntity where TKey : IEquatable<TKey>
{
    public TKey Id { get; }
    public DateTime CreatedAt { get; set; }
}