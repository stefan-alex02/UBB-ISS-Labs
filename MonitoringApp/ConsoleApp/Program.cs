// See https://aka.ms/new-console-template for more information

using System.Configuration;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories.AttendanceRepo;
using Persistence.Repositories.TaskRepo;
using Persistence.Repositories.UserRepo;
using Persistence.UnitOfWork;

var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
optionsBuilder.UseSqlite(ConfigurationManager
    .ConnectionStrings["DefaultConnection"]
    .ConnectionString.Replace("{Dir}", 
        Directory
            .GetParent(Environment.CurrentDirectory)!
            .Parent!
            .Parent!
            .Parent!
            .FullName));

IDatabaseContext dataBaseContext = new DatabaseContext(optionsBuilder.Options);
IUserRepository userRepository = new UserEfRepository(dataBaseContext);
IAttendanceRepository attendanceRepository = new AttendanceEfRepository(dataBaseContext);
ITaskRepository taskRepository = new TaskEfRepository(dataBaseContext);

IUnitOfWork unitOfWork = 
    new UnitOfWork(dataBaseContext, userRepository, attendanceRepository, taskRepository);

var users = userRepository.GetAll();
var attendances = attendanceRepository.GetAll();
var tasks = taskRepository.GetAll();

// userRepository.Add(new Employee(0, "b", "Bob", "b"));

unitOfWork.SaveChanges();

