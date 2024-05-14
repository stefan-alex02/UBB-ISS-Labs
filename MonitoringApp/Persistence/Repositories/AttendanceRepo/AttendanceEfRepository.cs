using System.Linq.Expressions;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.AttendanceRepo;

public class AttendanceEfRepository(IDatabaseContext context) : IAttendanceRepository {
    public void Add(Attendance entity) {
        context.Attendances.Add(entity);
    }

    public IEnumerable<Attendance> Find(Expression<Func<Attendance, bool>> predicate) {
        return context.Attendances.Where(predicate)
            .Include(a => a.MarkedBy)
            .ToList();
    }

    public Attendance? Get(int id) {
        return context.Attendances
            .Include(a => a.MarkedBy)
            .FirstOrDefault(a => a.Id == id);
    }

    public IEnumerable<Attendance> GetAll() {
        return context.Attendances
            .Include(a => a.MarkedBy)
            .ToList();
    }
}