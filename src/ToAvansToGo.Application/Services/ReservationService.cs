using ToAvansToGo.Application.InterFaces;
using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Application.Services;

public class ReservationService: IReservationService
{
    IPackageRepository _packageRepository;
    IStudentRepository _studentRepository;
    public ReservationService(IPackageRepository packageRepository, IStudentRepository studentRepository)
    {
        _packageRepository = packageRepository;
        _studentRepository = studentRepository;
    }
     public async Task ReservePackage(int packageId, int studentID)
     {
         var student = await _studentRepository.GetStudentByIdAsync(studentID);
         var package =  await _packageRepository.GetPackageByIdAsync(packageId);

         if (package == null)
             throw new Exception("Package not found");
         if (student == null)
             throw new Exception("Student not found");
         
         if (package.ReservedBy != null)
             throw new Exception("Package is already reserved");
         if(await HasAlreadyReservedOnDateAsync(student, package.PickUpTime))
             throw new Exception("Student has already reserved a package");
         if (package.Is18Plus && !IsStudent18Plus(student, package.PickUpTime))
             throw new Exception("Student is not 18+ and package is");
         
         package.ReservedBy = student;
         await _packageRepository.UpdatePackageAsync(package);
    }

     private bool IsStudent18Plus(Student student, DateTime pickupDate)
     {
         var age = DateTime.Now.Year - student.DateOfBirth.Year;
         if (student.DateOfBirth.Date > pickupDate.Date.AddYears(-age))
         {
             age--;
         }
         return age >= 18;
     }

     private async Task<bool> HasAlreadyReservedOnDateAsync(
         Student student,
         DateTime pickupDate)
     {
         var packages =
             await _packageRepository.GetPackagesReservedByStudentAsync(student.Id);

         return packages.Any(p => p.PickUpTime.Date == pickupDate.Date);
     }
}
    
