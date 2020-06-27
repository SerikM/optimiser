using Optimiser.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Optimiser.Services
{
    public interface IProcessingService
    {
        public List<Break> GetDefaultData();
        public List<Break> GetData(List<Break> breaks = null);

    }
}
