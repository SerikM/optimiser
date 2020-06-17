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


        public async Task<List<T>> GetDatedItems<T>(DateTime from, DateTime to)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")AWSXRayRecorder.Instance.BeginSegment("dynamo db call");

            List<T> page;
            try
            {
                var search = _ddbContext.ScanAsync<T>(null);
                page = await search.GetNextSetAsync();
            }
            catch (Exception)
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") {AWSXRayRecorder.Instance.EndSegment(DateTime.Now); }
                return null;
            }
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") { AWSXRayRecorder.Instance.EndSegment(DateTime.Now); }
            return page;
        }
    }
}