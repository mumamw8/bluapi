using System;
using Core.Interfaces;

namespace Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    public Task<int> Complete()
    {
        throw new NotImplementedException();
    }

    public IGenericRepository<TEntity> Repository<TEntity>()
    {
        throw new NotImplementedException();
    }
}

