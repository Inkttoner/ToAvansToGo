using ToAvansToGo.Domain.Entities;

namespace ToAvansToGo.Application.InterFaces;

public interface IReservationService
{ Task ReservePackage(int packageId, int studentId);
}