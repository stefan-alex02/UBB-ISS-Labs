using System.Linq.Expressions;
using Domain;

namespace Persistence.Repositories;

public interface IRepository<TE, in TId> where TE : Entity<TId> {
    public void Add(TE entity);

    public void AddRange(IEnumerable<TE> entities) {
        throw new NotImplementedException();
    }

    public IEnumerable<TE> Find(Expression<Func<TE, bool>> predicate);

    public TE? Get(TId id);

    public IEnumerable<TE> GetAll();

    public void Remove(TE entity);

    public void RemoveRange(IEnumerable<TE> entities) {
        throw new NotImplementedException();
    }
}