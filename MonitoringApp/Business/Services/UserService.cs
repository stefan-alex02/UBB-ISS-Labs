using System.Text.RegularExpressions;
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
        if (username is null || password is null) {
            throw new AuthenticationException("Fields cannot be empty");
        }
        
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
        if (_unitOfWork.UserRepository.GetByUsername(username) is not null) {
            throw new RegisterException("Username is already taken");
        }
        
        if (username is null || password is null || name is null) {
            throw new RegisterException("Fields cannot be empty");
        }
        if (username.Length < 2) {
            throw new RegisterException("Username must be at least 2 characters long");
        }
        if (password.Length < 2) {
            throw new RegisterException("Password must be at least 4 characters long");
        }
        if (!Regex.IsMatch(password, @"([a-zA-Z].*\d)|(\d.*[a-zA-Z])")) {
            throw new RegisterException("Password must contain at least one letter and one number");
        }

        User user = new Employee(username, name, PasswordService.HashPassword(password));
        
        _unitOfWork.UserRepository.Add(user);
        _unitOfWork.SaveChanges();
    }
    
    public User GetById(int id) =>
        _unitOfWork.UserRepository.Get(id) ?? throw new NotFoundException("User not found");
    
    // public IEnumerable<Manager> GetAllManagers() =>
    //     _unitOfWork.UserRepository
    //         .Find(u => u.UserRole == UserRole.Manager).Select(u => (Manager)u);
}