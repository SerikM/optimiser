using System;
using System.Collections.Generic;

namespace Optimiser.Models
{
    public class Rating : IData
    {
        public DemographicType DemoType { get; set; }
        public string DemoName  => Enum.GetName(typeof(DemographicType), DemoType);
        public int Score { get; set; }
    }
}