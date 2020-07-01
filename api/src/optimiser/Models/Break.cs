using Amazon.DynamoDBv2.DataModel;
using Optimiser.Enums;
using System.Collections.Generic;

namespace Optimiser.Models
{
    [DynamoDBTable("Break")]
    public class Break : IData
    {
        [DynamoDBHashKey]
        public int Id { get; set; }
        [DynamoDBProperty("Rating")]
        public List<Rating> Ratings { get; set; }
        [DynamoDBProperty("Disallowed Commercial Types")]
        public List<CommercialType> DisallowedCommTypes { get; set; }
        [DynamoDBProperty("Commercial")]
        public List<Commercial> Commercials { get; set; }
    }
}