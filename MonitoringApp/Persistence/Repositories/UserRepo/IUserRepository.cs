using Domain;
using Domain.Users;

namespace Persistence.Repositories.UserRepo;

public interface IUserRepository : IRepository<User, int> {
    public User? GetByUsername(string username);
}