using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Application.InterFaces;

public interface IEmployeeRepository
{
    
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task AddEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(Employee employee);
}