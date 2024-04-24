using System.Linq.Expressions;
using Persistence.Context;
using Task = Domain.Task;

namespace Persistence.Repositories.TaskRepo;

public class TaskEfRepository(IDatabaseContext context) : ITaskRepository {
    private readonly IDatabaseContext _context = context;

    public void Add(Task entity) {
        _context.Tasks.Add(entity);
    }

    public IEnumerable<Task> Find(Expression<Func<Task, bool>> predicate) {
        throw new NotImplementedException();
    }

    public Task? Get(int id) {
        throw new NotImplementedException();
    }

    public IEnumerable<Task> GetAll() {
        return _context.Tasks.ToList();
    }

    public void Remove(Task entity) {
        throw new NotImplementedException();
    }
}