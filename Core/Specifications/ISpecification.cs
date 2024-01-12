using System;
using System.Linq.Expressions;

namespace Core.Specifications;

public interface ISpecification<T>
{
    // An expression that takes a function that takes a type and what it is returning.
    // what is the criteria of what we want to get.
    Expression<Func<T, bool>>? Criteria { get; } // where criteria
    List<Expression<Func<T, object>>> Includes { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}
