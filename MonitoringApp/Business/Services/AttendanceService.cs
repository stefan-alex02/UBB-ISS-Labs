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
    
    public Attendance RecordAttendance(int userId, TimeOnly startTime) {
        User? user = unitOfWork.UserRepository.Get(userId);
        
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

    public Attendance EndAttendanceOf(int userId, DateTime endTime) {
        Attendance? foundAttendance = unitOfWork.AttendanceRepository
            .Find(a => a.MarkedBy.Id == userId).FirstOrDefault();

        if (foundAttendance is null) {
            throw new NotFoundException("User could not be found!");
        }
        
        foundAttendance.End = TimeOnly.FromDateTime(endTime);
        unitOfWork.SaveChanges();
        
        return foundAttendance;
    }
}