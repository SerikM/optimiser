using System.Collections.Generic;
using System.Threading.Tasks;
using Optimiser.Models;

namespace Optimiser.Services
{
    public interface IDbDataService<T>
    {
        Task<List<T>> GetItems<T>();
        List<Break> GetBreaksWithDefaultRatings();
        IEnumerable<Commercial> GetCommercials();
    }
}
