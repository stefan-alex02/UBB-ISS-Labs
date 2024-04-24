using Persistence.Context;
using Persistence.Repositories.AttendanceRepo;
using Persistence.Repositories.TaskRepo;
using Persistence.Repositories.UserRepo;

namespace Persistence.UnitOfWork;

public interface IUnitOfWork : IDisposable {
    public IDatabaseContext DatabaseContext { get; }
    public IUserRepository UserRepository { get; }
    public IAttendanceRepository AttendanceRepository { get; }
    public ITaskRepository TaskRepository { get; }
    
    public int SaveChanges();
    public Task SaveChangesAsync();
}