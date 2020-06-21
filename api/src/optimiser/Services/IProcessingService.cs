using System.Threading.Tasks;

namespace Optimiser.Services
{
    public interface IProcessingService
    {
        public Task<object> GetData();
    }
}
