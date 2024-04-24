using Business.Exceptions;
using Domain;
using Persistence.Repositories.UserRepo;
using Persistence.UnitOfWork;

namespace Business.Services;

public class UserService(IUnitOfWork unitOfWork) {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public User Authenticate(string username, string password) {
        User? user = _unitOfWork.UserRepository.GetByUsername(username);
        
        if (user is null) {
            throw new AuthenticationException("User not found");
        }
        
        if (user.Password != password) {
            throw new AuthenticationException("Invalid password");
        }
        
        // if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) {
        //     throw new Exception("Invalid password");
        // }
        
        return user;
    }
    
    public User GetById(int id) =>
        _unitOfWork.UserRepository.Get(id) ?? 
        throw new NotFoundException("User not found");

    public IEnumerable<Manager> GetAllManagers() =>
        _unitOfWork.UserRepository
            .Find(u => u.UserRole == UserRole.Manager).Select(u => (Manager)u);
}