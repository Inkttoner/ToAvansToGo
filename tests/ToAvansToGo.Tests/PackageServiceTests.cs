using NSubstitute;
using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Application.Services;
using ToAvansToGo.Domain.Entities;
using ToAvansToGo.Domain.Enums;

namespace ToAvansToGo.Tests;

public class PackageServiceTests
{
    private readonly IPackageRepository _packageRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPackageService _packageService;

    public PackageServiceTests()
    {
        _packageRepository = Substitute.For<IPackageRepository>();
        _employeeRepository = Substitute.For<IEmployeeRepository>();
        _packageService = new PackageService(_packageRepository, _employeeRepository);
    }

    private Employee CreateEmployee(int id, string name, City city)
    {
        var canteen = new Canteen(id, city, "LA", false);
        return new Employee(id, name, 1234, canteen);
    }

    private Product CreateProduct(int id, string name, bool hasAlcohol)
    {
        return new Product(id, name, hasAlcohol, "Picture Link");
    }

    private Package CreatePackage(int id, MealType mealType)
    {
        return new Package(
            id,
            "Test package",
            mealType,
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
    public async Task T05_US_03_CreatingPackageShouldReturnPackageWithLocationOfEmployee()
    {
        //Arrange
        var employee = CreateEmployee(1, "LA", City.Breda);

        var package1 = CreatePackage(1, MealType.Bread);

        _employeeRepository.GetEmployeeByIdAsync(employee.Id).Returns(employee);

        //Act
        await _packageService.CreatePackageAsync(package1, employee.Id);

        //Assert
        Assert.Equal(package1.PickUpLocation, employee.Location);
    }

    [Fact]
    public async Task T06_US_03_ChangingPackageWithReservationShouldReturnException()
    {
        //Arrange
        var employee = CreateEmployee(1, "LA", City.Breda);
        var package = CreatePackage(1, MealType.Bread);
        var student = CreateStudent("John Doe", 1);

        _employeeRepository.GetEmployeeByIdAsync(employee.Id).Returns(employee);

        //Act
        await _packageService.CreatePackageAsync(package, employee.Id);
        package.ReservedBy = student;
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _packageService.UpdatePackageAsync(package)
        );

        //Assert
        Assert.Equal("Package is reserved and can not be edited", exception.Message);
        await _packageRepository.DidNotReceive().UpdatePackageAsync(Arg.Any<Package>());
    }

    [Fact]
    public async Task T07_US_03_DeletingPackageWithReservationShouldReturnException()
    {
        //Arrange
        var employee = CreateEmployee(1, "LA", City.Breda);
        var package = CreatePackage(1, MealType.Bread);
        var student = CreateStudent("John Doe", 1);

        _employeeRepository.GetEmployeeByIdAsync(employee.Id).Returns(employee);

        //Act
        await _packageService.CreatePackageAsync(package, employee.Id);
        package.ReservedBy = student;
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _packageService.DeletePackageAsync(package)
        );

        //Assert
        Assert.Equal("Package is reserved and can not be deleted", exception.Message);
        await _packageRepository.DidNotReceive().DeletePackageAsync(Arg.Any<Package>());
    }

    [Fact]
    public async Task T08_Us_03_EmployeeShouldSeeListForLocationSortedByDate()
    {
        //Arrange
        var employee1 = CreateEmployee(1, "LA", City.Breda);
        var employee2 = CreateEmployee(2, "LA", City.Tilburg);
        var package1 = CreatePackage(1, MealType.Bread);
        var package2 = CreatePackage(2, MealType.Bread);
        var package3 = CreatePackage(3, MealType.Drinks);

        var packages = new List<Package>() { package1, package2, package3 };

        package1.PickUpTime = package1.PickUpTime.AddHours(12);
        _packageRepository.GetPackages().Returns(packages);
        _employeeRepository.GetEmployeeByIdAsync(employee1.Id).Returns(employee1);
        _employeeRepository.GetEmployeeByIdAsync(employee2.Id).Returns(employee2);

        //Act
        await _packageService.CreatePackageAsync(package1, employee1.Id);
        await _packageService.CreatePackageAsync(package2, employee2.Id);
        await _packageService.CreatePackageAsync(package3, employee1.Id);

        packages = await _packageService.GetPackagesForCanteenAsync(true, employee1.Location.Id);
        //Assert
        Assert.Equal(2, packages.Count);
        Assert.Equal(package1, packages[1]);
        Assert.Equal(package3, packages[0]);
    }

    [Fact]
    public async Task T09_US_03_CreatingPackageMoreThan2DaysInAdvanceShouldThrowException()
    {
        //Arrange
        var employee1 = CreateEmployee(1, "LA", City.Breda);
        var package1 = CreatePackage(1, MealType.Bread);

        package1.PickUpTime = package1.PickUpTime.AddDays(2);

        _employeeRepository.GetEmployeeByIdAsync(employee1.Id).Returns(employee1);

        //Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _packageService.CreatePackageAsync(package1, employee1.Id)
        );

        //Assert
        Assert.Equal("A package can not be created more than 2 days in advance", exception.Message);
    }

    [Fact]
    public async Task T10_US_04_PackageCreatedWith18PlusProductsShouldReturnPackageWithIs18PlusTrue()
    {
        //Arrange
        var package1 = CreatePackage(1, MealType.Drinks);
        var beer = CreateProduct(1, "beer", true);
        var products = new List<Product>() { beer };
        package1.Products = products;
        var employee = CreateEmployee(1, "LA", City.Breda);

        _employeeRepository.GetEmployeeByIdAsync(employee.Id).Returns(employee);

        //Act
        await _packageService.CreatePackageAsync(package1, employee.Id);

        //Assert
        Assert.True(package1.Is18Plus);
    }

    [Fact]
    public async Task T11_US_04_PackageUpdatedWith18PlusProductsShouldUpdatePackageWithIs18PlusTrue()
    {
        //Arrange
        var package1 = CreatePackage(1, MealType.Drinks);
        var beer = CreateProduct(1, "beer", true);
        var products = new List<Product>() { beer };
        var employee = CreateEmployee(1, "LA", City.Breda);

        _employeeRepository.GetEmployeeByIdAsync(employee.Id).Returns(employee);

        //Act
        await _packageService.CreatePackageAsync(package1, employee.Id);
        Assert.False(package1.Is18Plus);
        package1.Products = products;
        await _packageService.UpdatePackageAsync(package1);

        //Assert
        Assert.True(package1.Is18Plus);
    }

    [Fact]
    public async Task T12_US_06_PackagesOfMealTypeShouldShowAHistoryOfProducts()
    {
        //Arrange
        var package1 = CreatePackage(1, MealType.Drinks);
        var package2 = CreatePackage(2, MealType.Drinks);
        var package3 = CreatePackage(3, MealType.Drinks);
        var package4 = CreatePackage(4, MealType.Bread);

        package1.PickUpTime = DateTime.Today.AddDays(-3);
        package2.PickUpTime = DateTime.Today.AddDays(-2);
        package3.PickUpTime = DateTime.Today.AddDays(-2);
        package4.PickUpTime = DateTime.Today.AddDays(-2);

        var beer = CreateProduct(1, "beer", true);
        var lemonade = CreateProduct(2, "lemonade", false);
        var redBull = CreateProduct(3, "red bull", false);
        var appleJuice = CreateProduct(4, "apple juice", false);
        var orangeJuice = CreateProduct(5, "orange juice", false);
        var croissant = CreateProduct(6, "croissant", false);
        var breadStick = CreateProduct(7, "bread stick", false);

        package1.Products = new List<Product>() { beer, lemonade, redBull };
        package2.Products = new List<Product>() { appleJuice, orangeJuice, lemonade };
        package3.Products = new List<Product>() { appleJuice, orangeJuice, lemonade };
        package4.Products = new List<Product>() { croissant, breadStick };

        var packages = new List<Package>() { package1, package2, package3, package4 };

        _packageRepository.GetPackages().Returns(packages);

        //Act
        var products = await _packageService.GetProductHistoryAsync(MealType.Drinks);

        //Assert
        Assert.Equal(5, products.Count);
        Assert.Equal(lemonade, products[0]);
        Assert.Equal(appleJuice, products[1]);
        Assert.Equal(orangeJuice, products[2]);
        Assert.Equal(beer, products[3]);
        Assert.Equal(redBull, products[4]);
    }
}
