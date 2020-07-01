﻿using System.Collections.Generic;
using Optimiser.Models;

namespace Optimiser.Services
{
    public interface IProcessingService
    {
        public List<Break> GetDefaultData();
        public List<Break> GetData(List<Break> breaks = null);
    }
}
