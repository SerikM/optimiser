using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Optimiser.Models;

namespace Optimiser.Services
{
    public interface IDBDataService<T>
    {
        Task<List<T>> GetItems<T>();
        List<Break> GetBreaksWithDefaultRatings();
        List<Commercial> GetCommercials();
    }
}
