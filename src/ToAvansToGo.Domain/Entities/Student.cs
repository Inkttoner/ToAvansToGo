using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Domain.Entities;

public class Student
{
    public int Id { get; private set; }
    public int StudentNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
    public City StudyCity { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;

    private Student() { }

    public Student(int id, int studentNumber, string name, DateTime dateOfBirth,
        string email, City studyCity, string phoneNumber)
    {
        Id = id;
        StudentNumber = studentNumber;
        Name = name;
        DateOfBirth = dateOfBirth;
        Email = email;
        StudyCity = studyCity;
        PhoneNumber = phoneNumber;
    }
}