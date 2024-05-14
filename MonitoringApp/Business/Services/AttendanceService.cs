using Domain;
using Persistence.UnitOfWork;

namespace Business.Services;

public class AttendanceService(IUnitOfWork unitOfWork) {
    public IEnumerable<Attendance> GetAttendancesOfToday() {
        return unitOfWork.AttendanceRepository
            .Find(a => a.Day == DateOnly.FromDateTime(DateTime.Today));
    }
    
    public void RecordAttendance(Attendance attendance) {
        unitOfWork.AttendanceRepository.Add(attendance);
        unitOfWork.SaveChanges();
    }
}