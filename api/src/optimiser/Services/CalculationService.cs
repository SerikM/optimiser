using Optimiser.Models;
using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;

namespace Optimiser.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IDBDataService<IData> _dataService;

        public CalculationService(IDBDataService<IData> dataService)
        {
            _dataService = dataService;
        }

        public object GetData()
        {
            return null;
        }


    }
}
