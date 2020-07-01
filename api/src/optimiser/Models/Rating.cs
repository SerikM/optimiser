using Amazon.DynamoDBv2.DataModel;
using Optimiser.Enums;
using System;

namespace Optimiser.Models
{
    [DynamoDBTable("Rating")]
    public class Rating : IData
    {
        [DynamoDBHashKey]
        public int Id { get; set; }
        [DynamoDBProperty("Demographic Type")]
        public DemographicType DemoType { get; set; }
        [DynamoDBIgnore]
        public string DemoName  => Enum.GetName(typeof(DemographicType), DemoType);
        [DynamoDBProperty("Score")]
        public int Score { get; set; }
    }
}