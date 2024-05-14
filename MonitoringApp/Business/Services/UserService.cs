using Business.Exceptions;
using Business.Utils;
using Domain;
using Domain.Users;
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
        if (!PasswordService.VerifyPasswordHash(password, user.Password)) {
            throw new AuthenticationException("Invalid password");
        }
        
        return user;
    }
    
    public void RegisterEmployee(string username, string password, string name) {
        if (_unitOfWork.UserRepository.GetByUsername(username) is null) {
            throw new RegisterException("Username is already taken");
        }

        User user = new Employee(username, password, name);
        
        _unitOfWork.UserRepository.Add(user);
        _unitOfWork.SaveChanges();
    }
    
    public User GetById(int id) =>
        _unitOfWork.UserRepository.Get(id) ?? throw new NotFoundException("User not found");
    
    // public IEnumerable<Manager> GetAllManagers() =>
    //     _unitOfWork.UserRepository
    //         .Find(u => u.UserRole == UserRole.Manager).Select(u => (Manager)u);
}