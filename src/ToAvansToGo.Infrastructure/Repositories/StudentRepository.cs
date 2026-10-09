using Microsoft.EntityFrameworkCore;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Infrastructure.Data;

namespace ToAvansToGo.Infrastructure.Repositories;

public class StudentRepository(ApplicationDbContext context) : IStudentRepository
{
    public async Task<IEnumerable<Student>> GetStudentsAsync()
    {
        return await context.Students.AsNoTracking().ToListAsync();
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
        return await context.Students.FindAsync(id)
               ?? throw new KeyNotFoundException($"Student with id {id} was not found.");
    }

    public async Task AddStudentAsync(Student student)
    {
        context.Students.Add(student);
        await context.SaveChangesAsync();
    }

    public async Task UpdateStudentAsync(Student student)
    {
        context.Entry(student).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteStudentAsync(Student student)
    {
        context.Students.Remove(student);
        await context.SaveChangesAsync();
    }
}
