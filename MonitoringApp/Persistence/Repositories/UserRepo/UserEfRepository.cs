using System.Linq.Expressions;
using Domain;
using Domain.Users;
using Persistence.Context;

namespace Persistence.Repositories.UserRepo;

public class UserEfRepository(IDatabaseContext context) : IUserRepository {
    public void Add(User entity) {
        context.Users.Add(entity);
    }

    public IEnumerable<User> Find(Expression<Func<User, bool>> predicate) {
        return context.Users.Where(predicate).ToList();
    }

    public User? Get(int id) {
        return context.Users.FirstOrDefault(u => u.Id == id);
    }

    public IEnumerable<User> GetAll() {
        return context.Users.ToList();
    }

    public User? GetByUsername(string username) {
        return context.Users.FirstOrDefault(u => u.Username == username);
    }
}