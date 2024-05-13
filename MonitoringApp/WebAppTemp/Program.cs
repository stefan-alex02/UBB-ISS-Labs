using System.Configuration;
using Business.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Persistence.Context;
using Persistence.Repositories.AttendanceRepo;
using Persistence.Repositories.TaskRepo;
using Persistence.Repositories.UserRepo;
using Persistence.UnitOfWork;
using WebApp.Notifications;
using ConfigurationManager = System.Configuration.ConfigurationManager;

internal class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection")!
            .Replace("{Dir}", Directory.GetParent(Environment.CurrentDirectory)!.FullName);

        builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlite(connectionString));

        builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();

        builder.Services.AddScoped<IUserRepository, UserEfRepository>();
        builder.Services.AddScoped<IAttendanceRepository, AttendanceEfRepository>();
        builder.Services.AddScoped<ITaskRepository, TaskEfRepository>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<AttendanceService>();
        builder.Services.AddScoped<TaskService>();
        
        builder.Services.AddControllers().AddJsonOptions(options => {
            options.JsonSerializerOptions.ReferenceHandler = 
                System.Text.Json.Serialization.ReferenceHandler.Preserve;
        });

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddSignalR();
        
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder.WithOrigins("http://localhost:4200")
                        .WithMethods("GET", "POST", "OPTIONS")
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed(origin => true); // Allow any origin
                });
        });

        var app = builder.Build();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.MapHub<NotificationHub>("/notificationHub");
        // app.UseEndpoints(endpoints => {
        //     endpoints.MapHub<NotificationHub>("/notificationHub");
        // });

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(); // Ensure this is after UseRouting() and before UseAuthorization()

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
        
    }
}