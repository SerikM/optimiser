using Amazon.DynamoDBv2.DataModel;
using System;

namespace Optimiser.Models
{
    public class DataStructure : IData 
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DemographicType Type { get; set; }
    }
}