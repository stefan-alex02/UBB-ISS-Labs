using Persistence.Context;
using Persistence.Repositories.AttendanceRepo;
using Persistence.Repositories.TaskRepo;
using Persistence.Repositories.UserRepo;

namespace Persistence.UnitOfWork;

public class UnitOfWork(
    IDatabaseContext databaseContext, 
    IUserRepository userRepository, 
    IAttendanceRepository attendanceRepository,
    ITaskRepository taskRepository) 
    : IUnitOfWork {
    public IDatabaseContext DatabaseContext { get; } = databaseContext;
    public IUserRepository UserRepository { get; } = userRepository;
    public IAttendanceRepository AttendanceRepository { get; } = attendanceRepository;
    public ITaskRepository TaskRepository { get; } = taskRepository;

    public int SaveChanges() => DatabaseContext.SaveChanges();

    public async Task SaveChangesAsync() => await DatabaseContext.SaveChangesAsync();

    public void Dispose() => DatabaseContext?.Dispose();
}