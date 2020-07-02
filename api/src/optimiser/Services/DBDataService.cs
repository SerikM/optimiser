using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.XRay.Recorder.Core;
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

        /// <summary>
        /// connects to DynamoDb and retrieves data of the the type T provided by the client
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>a list of typed objects from the database</returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SeedItems()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") AWSXRayRecorder.Instance.BeginSegment("dynamo db call");
            try
            {
                var brBatch = _ddbContext.CreateBatchWrite<Break>();
                brBatch.AddPutItems(DefaultData.GetDefaultBreaks());


                var config = new DynamoDBOperationConfig();
                var commBatch = _ddbContext.CreateBatchWrite<Commercial>(config);
                commBatch.AddPutItems(DefaultData.GetDefaultCommercials());

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