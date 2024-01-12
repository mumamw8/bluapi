using System;
using Core.Specifications;

namespace Core.Interfaces;

public interface IGenericRepository<T>
{
    Task<T> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T> GetEntityWithSpec(ISpecification<T> spec);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    Task<int> CountAsync(ISpecification<T> spec);
    // all in memory. Unit of work save these changes
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}