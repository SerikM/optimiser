using Amazon.DynamoDBv2.DataModel;
using Optimiser.Enums;
using System;

namespace Optimiser.Models
{
    [DynamoDBTable("Commercial")]
    public class Commercial : IData
    {
        [DynamoDBHashKey]
        public int Id { get; set; }
        [DynamoDBProperty("Rating")]
        public Rating CurrentRating { get; set; }
        [DynamoDBProperty(" Commercial Type")]
        public CommercialType CommercialType { get; set; }
        [DynamoDBProperty(" Target Demographic")]
        public DemographicType TargetDemo { get; set; }
        [DynamoDBIgnore]
        public string CommercialTypeName => Enum.GetName(typeof(CommercialType), CommercialType);
        [DynamoDBIgnore]
        public string CommercialTypeNameMobile =>  Enum.GetName(typeof(CommercialType), CommercialType).Substring(0, 6);
        [DynamoDBIgnore]
        public string TargetDemoName => Enum.GetName(typeof(DemographicType), TargetDemo);
    }
}