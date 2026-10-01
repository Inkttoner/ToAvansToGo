using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Domain.Entities;

public class Student(int id, int studentNumber, string name, DateTime dateOfBirth, string email, City studyCity, string phoneNumber)
{
    public  int Id { get; private set; } = id;
    public  int StudentNumber { get; set; } = studentNumber;
    public  string Name  { get; set; } = name;
    public  DateTime DateOfBirth  { get; set; } = dateOfBirth;
    public  string Email   { get; set; } = email;
    public  City StudyCity  { get; set; } = studyCity;
    public  string PhoneNumber  { get; set; } = phoneNumber;  
}