using Domain;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Task = Domain.Task;

namespace Persistence.Context;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options), IDatabaseContext {
    public DbSet<User> Users { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Task> Tasks { get; set; }

    public DatabaseContext() 
        : this(new DbContextOptionsBuilder<DatabaseContext>().Options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseSqlite("Data Source=DB/Monitoring.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>()
            .HasDiscriminator<UserRole>("UserRole")
            .HasValue<User>(UserRole.StaffMember)
            .HasValue<Manager>(UserRole.Manager)
            .HasValue<Employee>(UserRole.Employee);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // modelBuilder.Entity<Attendance>()
        //     .HasOne(a => a.MarkedBy)
        //     .WithMany(e => e.Attendances)
        //     .HasForeignKey(a => a.MarkedById);
        //
        // modelBuilder.Entity<Task>()
        //     .HasOne(a => a.AssignedTo)
        //     .WithMany(e => e.Tasks)
        //     .HasForeignKey(a => a.AssignedToId);   
        //
        // modelBuilder.Entity<Task>()
        //     .HasOne(a => a.CreatedBy)
        //     .WithMany(e => e.CreatedTasks)
        //     .HasForeignKey(a => a.CreatedById);  
    }
}