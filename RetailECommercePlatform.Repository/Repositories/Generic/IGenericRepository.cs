using System.Linq.Expressions;
using MongoDB.Driver;
using RetailECommercePlatform.Repository.Entities;

namespace RetailECommercePlatform.Repository.Repositories.Generic;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Get(Expression<Func<T, bool>> predicate = null);
    Task<List<T>> FilterAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> SearchAsync(FilterDefinition<T> filter);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(string id);
    Task<T> AddAsync(T entity);
    Task<bool> AddRangeAsync(IEnumerable<T> entities);
    Task<T> UpdateAsync(string id, T entity);
    Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate);
    Task<T> DeleteAsync(string id);
    Task<T> DeleteAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
}