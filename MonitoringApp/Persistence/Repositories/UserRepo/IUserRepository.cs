using Domain;

namespace Persistence.Repositories.UserRepo;

public interface IUserRepository : IRepository<Domain.User, int> {
    public User? GetByUsername(string username);
}