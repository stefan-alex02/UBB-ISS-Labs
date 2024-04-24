using System.Linq.Expressions;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories.AttendanceRepo;

public class AttendanceEfRepository(IDatabaseContext context) : IAttendanceRepository {
    private readonly IDatabaseContext _context = context;

    public void Add(Attendance entity) {
        _context.Attendances.Add(entity);
    }

    public IEnumerable<Attendance> Find(Expression<Func<Attendance, bool>> predicate) {
        return _context.Attendances.Where(predicate)
            .Include(a => a.MarkedBy)
            .ToList();
    }

    public Attendance? Get(int id) {
        throw new NotImplementedException();
    }

    public IEnumerable<Attendance> GetAll() {
        return _context.Attendances.ToList();
    }

    public void Remove(Attendance entity) {
        throw new NotImplementedException();
    }
}