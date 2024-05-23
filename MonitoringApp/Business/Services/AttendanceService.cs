using Business.Exceptions;
using Domain;
using Domain.Users;
using Persistence.UnitOfWork;

namespace Business.Services;

public class AttendanceService(IUnitOfWork unitOfWork) {
    public IEnumerable<Attendance> GetUnfinishedAttendances() {
        return unitOfWork.AttendanceRepository
            .Find(a => a.End == null);
    }
    
    public Attendance RecordAttendance(string employeeUsername, TimeOnly startTime) {
        User? user = unitOfWork.UserRepository.GetByUsername(employeeUsername);
        
        if (user is null) {
            throw new NotFoundException("User could not be found!");
        }

        if (user is not Employee employee) {
            throw new UserException("User is not an employee!");
        }
        
        Attendance attendance = new Attendance {
            Day = DateOnly.FromDateTime(DateTime.Today),
            Start = startTime,
            MarkedBy = employee,
        };
        
        unitOfWork.AttendanceRepository.Add(attendance);
        unitOfWork.SaveChanges();

        return attendance;
    }

    public Attendance EndAttendanceOf(int userId, TimeOnly endTime) {
        Attendance? foundAttendance = unitOfWork.AttendanceRepository
            .Find(a => a.MarkedBy.Id == userId && a.End == null).FirstOrDefault();

        if (foundAttendance is null) {
            throw new NotFoundException("User could not be found!");
        }
        
        foundAttendance.End = endTime;
        unitOfWork.SaveChanges();
        
        return foundAttendance;
    }
}