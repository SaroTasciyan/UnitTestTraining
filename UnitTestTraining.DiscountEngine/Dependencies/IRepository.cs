using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UnitTestTraining.DiscountEngine.Dependencies
{
    public interface IRepository
    {
        int Count();
        long LongCount();
    }

    public interface IRepository<T> : IRepository
    {
        IEnumerable<T> Get();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
    }
}