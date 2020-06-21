using Amazon.DynamoDBv2.DataModel;
using System;

namespace Optimiser.Models
{
    public class Commercial : IData 
    {
        public int Id { get; set; }
        public Rating CurrentRating { get; set; }
        public CommercialType CommercialType { get; set; }
        public DemographicType TargetDemo { get; set; }
    }
}