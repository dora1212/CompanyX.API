using CompanyX.API.BusinessLogic.Interfaces;

namespace CompanyX.API.BusinessLogic.Services
{
    public class ConcurrencyService : IConcurrencyService
    {
        private readonly SemaphoreSlim _semaphore;

        public ConcurrencyService(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        public SemaphoreSlim GetSemaphore()
        {
            return _semaphore;
        }
    }
}

