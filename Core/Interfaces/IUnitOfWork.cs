using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<TEntity> Repository<TEntity>();
    Task<int> Complete(); // return number of changes to the database.
}

