using Domain;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public interface IDatabaseContext : IDisposable {
    public DbSet<User> Users { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Domain.Task> Tasks { get; set; }
    
    public int SaveChanges();
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}