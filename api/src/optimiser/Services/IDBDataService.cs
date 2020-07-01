using System.Collections.Generic;
using System.Threading.Tasks;

namespace Optimiser.Services
{
    public interface IDbDataService<T>
    {
        Task<List<T>> GetItems<T>();
        bool SeedItems();
    }
}
