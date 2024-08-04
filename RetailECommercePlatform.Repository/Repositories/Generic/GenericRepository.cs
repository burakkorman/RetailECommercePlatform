using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using RetailECommercePlatform.Repository.Entities;

namespace RetailECommercePlatform.Repository.Repositories.Generic;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly IMongoCollection<T> Collection;
    private readonly MongoDbSettings settings;

    protected GenericRepository(IOptions<MongoDbSettings> options)
    {
        this.settings = options.Value;
        var client = new MongoClient(this.settings.ConnectionString);
        var db = client.GetDatabase(this.settings.Database);
        this.Collection = db.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
    }

    public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
    {
        return predicate == null
            ? Collection.AsQueryable()
            : Collection.AsQueryable().Where(predicate);
    }
    
    public virtual Task<List<T>> FilterAsync(Expression<Func<T, bool>> predicate)
    {
        var collation = new Collation("tr");
        var options = new FindOptions
        {
            Collation = collation
        };
        
        return Collection.Find(predicate, options).ToListAsync();
    }
    
    public virtual Task<List<T>> SearchAsync(FilterDefinition<T> filter)
    {
        return Collection.Find(filter).ToListAsync();
    }

    public virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return Collection.Find(predicate).FirstOrDefaultAsync();
    }

    public virtual Task<T> GetByIdAsync(string id)
    {
        return Collection.Find(Builders<T>.Filter.Eq("_id", new ObjectId(id))).FirstOrDefaultAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        if (entity is MongoDbEntity mongoEntity && string.IsNullOrEmpty(mongoEntity.Id))
        {
            mongoEntity.Id = ObjectId.GenerateNewId().ToString();
        }
        
        var options = new InsertOneOptions { BypassDocumentValidation = false };
        await Collection.InsertOneAsync(entity, options);
        return entity;
    }

    public virtual async Task<bool> AddRangeAsync(IEnumerable<T> entities)
    {
        var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
        return (await Collection.BulkWriteAsync((IEnumerable<WriteModel<T>>)entities, options)).IsAcknowledged;
    }

    public virtual async Task<T> UpdateAsync(string id, T entity)
    {
        return await Collection.FindOneAndReplaceAsync(Builders<T>.Filter.Eq("_id", new ObjectId(id)), entity);
    }

    public virtual async Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate)
    {
        return await Collection.FindOneAndReplaceAsync(predicate, entity);
    }

    public virtual async Task<T> DeleteAsync(string id)
    {
        return await Collection.FindOneAndDeleteAsync(Builders<T>.Filter.Eq("_id", new ObjectId(id)));
    }

    public virtual async Task<T> DeleteAsync(Expression<Func<T, bool>> filter)
    {
        return await Collection.FindOneAndDeleteAsync(filter);
    }
}