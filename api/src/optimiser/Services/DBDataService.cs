using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Optimiser.Models;
using Microsoft.Extensions.Options;
using Amazon.XRay.Recorder.Core;

namespace Optimiser.Services
{
    public class DBDataService<T> : IDBDataService<IData>
    {
        private readonly IOptions<AwsSettingsModel> _appSettings;
        private readonly IDynamoDBContext _ddbContext;

        public DBDataService(IAmazonDynamoDB dynamoDbClient, IOptions<AwsSettingsModel> appSettings)
        {
            _appSettings = appSettings;
            var tblName = _appSettings?.Value?.MainTableName;
            if (!string.IsNullOrEmpty(tblName))
            {
                AWSConfigsDynamoDB.Context.TypeMappings[typeof(T)] = new Amazon.Util.TypeMapping(typeof(T), tblName);
            }
            var conf = new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
            _ddbContext = new DynamoDBContext(dynamoDbClient, conf);
        }


        public async Task<List<T>> GetItems<T>()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") AWSXRayRecorder.Instance.BeginSegment("dynamo db call");

            List<T> page;
            try
            {
                var search = _ddbContext.ScanAsync<T>(null);
                page = await search.GetNextSetAsync();
            }
            catch (Exception)
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") { AWSXRayRecorder.Instance.EndSegment(DateTime.Now); }
                return null;
            }
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") { AWSXRayRecorder.Instance.EndSegment(DateTime.Now); }
            return page;
        }


        public List<Break> GetBreaks()
        {
            return new List<Break> {
                    new Break() { Id = 1, Ratings = new List<Rating>{
                                                      new Rating() { Score = 80, DemoType = DemographicType.Women },
                                                      new Rating() { Score = 100, DemoType = DemographicType.Men },
                                                      new Rating() { Score = 250, DemoType = DemographicType.Total }}},

                    new Break() { Id = 2, Ratings = new List<Rating>{
                                                      new Rating() { Score = 50, DemoType = DemographicType.Women },
                                                      new Rating() { Score = 120, DemoType = DemographicType.Men },
                                                      new Rating() { Score = 200, DemoType = DemographicType.Total }}},

                     new Break() { Id = 3, Ratings = new List<Rating>{
                                                      new Rating() { Score = 350, DemoType = DemographicType.Women },
                                                      new Rating() { Score = 150, DemoType = DemographicType.Men },
                                                      new Rating() { Score = 500, DemoType = DemographicType.Total }}}
                };
        }


        public List<Commercial> GetCommercials()
        {
            return new List<Commercial> {
                                          new Commercial() { Id = 1, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Women },
                                          new Commercial() { Id = 2, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Men },
                                          new Commercial() { Id = 3, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Total },

                                          new Commercial() { Id = 4, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Men },
                                          new Commercial() { Id = 5, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Men },
                                          new Commercial() { Id = 6, CommercialType = CommercialType.Finance, TargetDemo = DemographicType.Women },

                                          new Commercial() { Id = 7, CommercialType = CommercialType.Finance, TargetDemo = DemographicType.Men },
                                          new Commercial() { Id = 8, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Total },
                                          new Commercial() { Id = 9, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Women }
                                        };
        }
    }
}