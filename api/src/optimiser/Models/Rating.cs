using System.Collections.Generic;

namespace Optimiser.Models
{
    public class Rating: IData
    {
        public DemographicType DemoType { get; set; }
        public int Score { get; set; }
    }
}