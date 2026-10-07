using NSubstitute;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Application.Services;
using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Tests;

public class ReservationServiceTests
{
    private readonly IStudentRepository _studentRepository;
    private readonly IPackageRepository _packageRepository;
    private readonly ReservationService _reservationService;

    public ReservationServiceTests()
    {
        _packageRepository = Substitute.For<IPackageRepository>();
        _studentRepository = Substitute.For<IStudentRepository>();
        _reservationService = new ReservationService(_packageRepository, _studentRepository);
    }

    private Package CreatePackage(City city, int id)
    {
        return new Package(
            id,
            "Test package",
            MealType.Bread,
            new List<Product>(),
            DateTime.Today.AddDays(1),
            DateTime.Today.AddDays(1).AddHours(2),
            5.00,
            null
        );
    }

    private Student CreateStudent(String name, int id)
    {
        return new Student(
            id,
            12345,
            name,
            DateTime.Today.AddYears(-20),
            "email@email.com",
            City.Breda,
            "0612334534"
        );
    }

    [Fact]
    public async Task T01_US_05_ReservingPackageForSameDayShouldThrowException()
    {
        // Arrange
        var package1 = CreatePackage(City.Breda, 1);
        var package2 = CreatePackage(City.Breda, 2);

        package1.PickUpTime = DateTime.Today.AddDays(1).AddHours(12);
        package2.PickUpTime = DateTime.Today.AddDays(1).AddHours(14);

        var student = CreateStudent("John Doe", 1);

        var packages = new List<Package> { package1, package2 };

        _packageRepository.GetPackageByIdAsync(package1.Id).Returns(package1);

        _packageRepository.GetPackageByIdAsync(package2.Id).Returns(package2);

        _studentRepository.GetStudentByIdAsync(student.Id).Returns(student);

        _packageRepository.GetPackages().Returns(packages);

        // _reservationService = new ReservationService(_packageRepository, _studentRepository);

        // Act
        await _reservationService.ReservePackage(package1.Id, student.Id);

        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _reservationService.ReservePackage(package2.Id, student.Id)
        );

        // Assert
        Assert.Equal("Student has already reserved a package", exception.Message);
    }

    [Fact]
    public async Task T02_US_04_Reserving18PlusPackageWhenNot18OnDateShouldThrowException()
    {
        //Arrange
        var package1 = CreatePackage(City.Breda, 1);
        package1.Is18Plus = true;
        var student = CreateStudent("John Doe", 1);
        student.DateOfBirth = DateTime.Today.AddYears(-17);
        var packages = new List<Package>() { package1 };

        _packageRepository.GetPackageByIdAsync(package1.Id).Returns(package1);
        _studentRepository.GetStudentByIdAsync(student.Id).Returns(student);
        _packageRepository.GetPackages().Returns(packages);

        //Act
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _reservationService.ReservePackage(package1.Id, student.Id)
        );

        //Assert
        Assert.Equal("Student is not 18+ and package is", exception.Message);
    }

    [Fact]
    public async Task T03_US_07_ReservingAPackageThatIsReservedShouldThrowException()
    {
        //Arrange
        var package1 = CreatePackage(City.Breda, 1);
        var student1 = CreateStudent("John Doe", 1);
        var student2 = CreateStudent("Coen de Kruijf", 2);

        var packages = new List<Package>() { package1 };

        _packageRepository.GetPackageByIdAsync(package1.Id).Returns(package1);
        _studentRepository.GetStudentByIdAsync(student1.Id).Returns(student1);
        _studentRepository.GetStudentByIdAsync(student2.Id).Returns(student2);
        _packageRepository.GetPackages().Returns(packages);

        //Act
        await _reservationService.ReservePackage(package1.Id, student1.Id);
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _reservationService.ReservePackage(package1.Id, student2.Id)
        );

        //Assert
        Assert.Equal("Package is already reserved", exception.Message);
    }

    [Fact]
    public async Task T04_US_05_ReservingAPackageShouldChangePackageReservedByIntoStudent()
    {
        //Arrange
        var package1 = CreatePackage(City.Breda, 1);
        var student1 = CreateStudent("John Doe", 1);

        var packages = new List<Package>() { package1 };
        _packageRepository.GetPackageByIdAsync(package1.Id).Returns(package1);
        _studentRepository.GetStudentByIdAsync(student1.Id).Returns(student1);
        _packageRepository.GetPackages().Returns(packages);

        //Act
        await _reservationService.ReservePackage(package1.Id, student1.Id);

        //Assert
        Assert.Equal(package1.ReservedBy, student1);
    }
}
