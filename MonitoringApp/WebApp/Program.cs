using System.Text;
using Business.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using Persistence.Repositories.AttendanceRepo;
using Persistence.Repositories.TaskRepo;
using Persistence.Repositories.UserRepo;
using Persistence.UnitOfWork;
using WebApp.Authentication;
using WebApp.Notification;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();
string connectionString = configuration.GetConnectionString("DefaultConnection")!
    .Replace("{Dir}", Directory.GetParent(Environment.CurrentDirectory)!.FullName);

builder.Services
    .AddDbContext<DatabaseContext>(options => options.UseSqlite(connectionString))
    .AddScoped<IDatabaseContext, DatabaseContext>()
    .AddScoped<IUserRepository, UserEfRepository>()
    .AddScoped<ITaskRepository, TaskEfRepository>()
    .AddScoped<IAttendanceRepository, AttendanceEfRepository>()
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<UserService>()
    .AddScoped<TaskService>()
    .AddScoped<AttendanceService>()
    .AddScoped<RefreshTokenMiddleware>();

var jwtConfig = builder.Configuration.GetSection("Jwt");

TokenValidationParameters validationParameters = new TokenValidationParameters {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = configuration["Jwt:Issuer"],
    ValidAudience = configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
    ClockSkew = TimeSpan.Zero
};

builder.Services.Configure<JwtOptions>(options => {
    options.TokenValidationParameters = validationParameters;
});
    
builder.Services.AddSingleton<JwtService>(serviceProvider => {
    var issuer = jwtConfig["Issuer"];
    var audience = jwtConfig["Audience"];
    var key = jwtConfig["Key"];
    var tokenLifetime = TimeSpan.Parse(jwtConfig["TokenLifetime"]);
    
    return new JwtService(issuer, audience, key, tokenLifetime);
});

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.ReferenceHandler = 
        System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAngularApp",
        policyBuilder => {
            policyBuilder.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Authorization", "abc");
        });
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = validationParameters;
    });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<NotificationHub>("/notificationHub");

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RefreshTokenMiddleware>();

app.MapControllers();

app.Run();