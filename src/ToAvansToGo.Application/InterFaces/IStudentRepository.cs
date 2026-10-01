using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Application.InterFaces;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetStudentsAsync();
    Task<Student> GetStudentByIdAsync(int id);
    Task UpdateStudentAsync(Student student);
    Task AddStudentAsync(Student student);
    Task DeleteStudentAsync(Student student);
}