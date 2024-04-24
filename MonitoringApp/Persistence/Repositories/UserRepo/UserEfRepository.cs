using System.Linq.Expressions;
using Domain;
using Persistence.Context;

namespace Persistence.Repositories.UserRepo;

public class UserEfRepository(IDatabaseContext context) : IUserRepository, IDisposable {
    private readonly IDatabaseContext _context = context;

    public void Add(User entity) {
        _context.Users.Add(entity);
    }

    public IEnumerable<User> Find(Expression<Func<User, bool>> predicate) {
        return _context.Users.Where(predicate).ToList();
    }

    public User? Get(int id) {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public IEnumerable<User> GetAll() {
        return _context.Users.ToList();
    }

    public void Remove(User entity) {
        throw new NotImplementedException();
    }

    public User? GetByUsername(string username) {
        return _context.Users.FirstOrDefault(u => u.Username == username);
    }

    public void Dispose() {
        _context.Dispose();
    }
}