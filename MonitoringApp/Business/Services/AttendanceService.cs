using Domain;
using Persistence.UnitOfWork;

namespace Business.Services;

public class AttendanceService(IUnitOfWork unitOfWork) {
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public IEnumerable<Attendance> GetAttendancesOfToday() {
        return _unitOfWork.AttendanceRepository
            .Find(a => a.Day == DateOnly.FromDateTime(DateTime.Today))
            .Select(a => new Attendance {
                Id = a.Id,
                Day = a.Day,
                Start = a.Start,
                End = a.End,
                MarkedBy = (Employee)_unitOfWork.UserRepository.Get(a.MarkedById),
                }
            ).ToList();
    }
    
    public void RecordAttendance(Attendance attendance) {
        _unitOfWork.AttendanceRepository.Add(attendance);
        _unitOfWork.SaveChanges();
    }
}