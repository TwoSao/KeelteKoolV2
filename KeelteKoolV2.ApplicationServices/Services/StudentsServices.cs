using KeelteKoolV2.Core.Domain;
using KeelteKoolV2.Core.DTO;
using KeelteKoolV2.Core.ServiceInterface;
using KeelteKoolV2.Data;
using Microsoft.EntityFrameworkCore;

namespace KeelteKoolV2.ApplicationServices.Services;

public class StudentsServices : IStudentsServices
{
    private readonly KeelteKoolV2Context _context;

    public StudentsServices(KeelteKoolV2Context context) => _context = context;

    public async Task<Student?> Create(StudentDTO dto)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.FirstName) ||
            string.IsNullOrWhiteSpace(dto.LastName))
            return null;

        var now = DateTime.UtcNow;
        var student = new Student
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            UserID = dto.UserID,
            CreatedAt = now,
            ModifiedAt = now
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<Student?> Update(StudentDTO dto)
    {
        if (dto is null || dto.Id == Guid.Empty || string.IsNullOrWhiteSpace(dto.FirstName) ||
            string.IsNullOrWhiteSpace(dto.LastName))
            return null;

        var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == dto.Id);
        if (student is null)
            return null;

        student.FirstName = dto.FirstName.Trim();
        student.LastName = dto.LastName.Trim();
        student.UserID = dto.UserID;
        student.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return student;
    }

    public Task<Student?> DetailAsync(Guid id) => id == Guid.Empty
        ? Task.FromResult<Student?>(null)
        : _context.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Student?> Delete(Guid id)
    {
        if (id == Guid.Empty)
            return null;

        var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == id);
        if (student is null)
            return null;

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public Task<List<Student>> GetAll() => _context.Students
        .AsNoTracking()
        .OrderBy(x => x.LastName)
        .ThenBy(x => x.FirstName)
        .ToListAsync();
}
