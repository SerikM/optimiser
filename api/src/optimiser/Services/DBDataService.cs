using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.XRay.Recorder.Core;
using Optimiser.Enums;
using Optimiser.Models;

namespace Optimiser.Services
{
    public class DbDataService<T> : IDbDataService<IData>
    {
        private readonly IDynamoDBContext _ddbContext;

        public DbDataService(IAmazonDynamoDB dynamoDbClient)
        {
            AWSConfigsDynamoDB.Context.TypeMappings[typeof(T)] = new Amazon.Util.TypeMapping(typeof(T), typeof(T).Name);
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

        public bool SeedItems()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") AWSXRayRecorder.Instance.BeginSegment("dynamo db call");
            try
            {
                var brBatch = _ddbContext.CreateBatchWrite<Break>();
                brBatch.AddPutItems(new List<Break> {
                    new Break() { Id = 1, Ratings = new List<Rating>{
                    new Rating() { Score = 80, DemoType = DemographicType.Women },
                    new Rating() { Score = 100, DemoType = DemographicType.Men },
                    new Rating() { Score = 250, DemoType = DemographicType.Total }}},
                    new Break() { Id = 2, DisallowedCommTypes =
                    new List<CommercialType> { CommercialType.Finance }, Ratings = new List<Rating>{
                    new Rating() { Score = 50, DemoType = DemographicType.Women },
                    new Rating() { Score = 120, DemoType = DemographicType.Men },
                    new Rating() { Score = 200, DemoType = DemographicType.Total }}},
                    new Break() { Id = 3, Ratings = new List<Rating>{
                    new Rating() { Score = 350, DemoType = DemographicType.Women },
                    new Rating() { Score = 150, DemoType = DemographicType.Men },
                    new Rating() { Score = 500, DemoType = DemographicType.Total }}}
                });


                var config = new DynamoDBOperationConfig();
                var commBatch = _ddbContext.CreateBatchWrite<Commercial>(config);
                commBatch.AddPutItems(new List<Commercial> {
                new Commercial() { Id = 1, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Women },
                new Commercial() { Id = 2, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Men },
                new Commercial() { Id = 3, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Total },
                new Commercial() { Id = 4, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Men },
                new Commercial() { Id = 5, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Men },
                new Commercial() { Id = 6, CommercialType = CommercialType.Finance, TargetDemo = DemographicType.Women },
                new Commercial() { Id = 7, CommercialType = CommercialType.Finance, TargetDemo = DemographicType.Men },
                new Commercial() { Id = 8, CommercialType = CommercialType.Automotive, TargetDemo = DemographicType.Total },
                new Commercial() { Id = 9, CommercialType = CommercialType.Travel, TargetDemo = DemographicType.Women }
                });

                var superBatch = new MultiTableBatchWrite(brBatch, commBatch);
                var result = superBatch.ExecuteAsync().Status;
                return result != TaskStatus.Faulted && result != TaskStatus.Canceled;
            }
            catch (Exception)
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") { AWSXRayRecorder.Instance.EndSegment(DateTime.Now); }
                return false;
            }
        }
    }
}