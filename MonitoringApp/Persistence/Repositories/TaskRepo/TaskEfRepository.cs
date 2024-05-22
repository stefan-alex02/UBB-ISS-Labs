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
        return _context.Tasks.Where(predicate);
    }

    public Task? Get(int id) {
        return _context.Tasks.Find(id);
    }

    public IEnumerable<Task> GetAll() {
        return _context.Tasks.ToList();
    }
    
    public void Update(Task entity) {
        _context.Tasks.Update(entity);
    }
    
    public void Remove(Task entity) {
        _context.Tasks.Remove(entity);
    }
}