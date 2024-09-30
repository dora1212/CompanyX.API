
namespace CompanyX.API.BusinessLogic.Interfaces
{
    public interface IConcurrencyService
    {
        SemaphoreSlim GetSemaphore();
    }
}
