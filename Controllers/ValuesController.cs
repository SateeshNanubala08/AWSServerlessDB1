using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.S3;
using AWSServerlessDB.Modals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AWSServerlessDB.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {

        private static IAmazonDynamoDB dynamoDBClient;
        private static IDynamoDBContext dynamoDbContext;
        IAmazonS3 S3Client { get; set; }

        //public readonly IAmazonS3 amazonS3;
       // private readonly AmazonS3Client _S3Bucket;
        private readonly ILogger<ValuesController> _logger;

        //private readonly ServiceConfiguration _settings;
        private const string TableName = "commentsTable";

        //public ValuesController(IAmazonS3 amazonS3, IOptions<ServiceConfiguration> settings, IAmazonS3 s3Client, ILogger<ValuesController> logger, IAmazonDynamoDB _dynamoDBClient, IDynamoDBContext _dynamoDbContext)
        public ValuesController(IOptions<ServiceConfiguration> settings,  ILogger<ValuesController> logger, IAmazonDynamoDB _dynamoDBClient, IDynamoDBContext _dynamoDbContext)
        {
            _logger = logger;

            //this.amazonS3 = amazonS3;

            //this.S3Client = s3Client;
           // this._settings = settings.Value;
           // this._S3Bucket = new AmazonS3Client(this._settings.AWSS3.AccessKey, this._settings.AWSS3.SecretKey, RegionEndpoint.APSouth1);

            dynamoDBClient = _dynamoDBClient;
            dynamoDbContext = _dynamoDbContext;

        }



        // GET api/values
        [HttpGet]
        public async Task<List<string>> Get()
        {
            try
            {
                List<string> list = new List<string>();
                var request = new ListTablesRequest
                {
                    Limit = 100
                };

                var response = await dynamoDBClient.ListTablesAsync(request);
                list = response.TableNames;

                return list;
                 
            }
            catch (Exception)
            {

                throw;
            }
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}



        [HttpPost("SaveToOrdersTable")]
        public async Task<Orders> SaveToDynamoDbOrders(Orders obj)
        {
            var response = await SaveToDynamoDb(obj);
            return response;
        }

        public async Task<T> SaveToDynamoDb<T>(T obj) where T : class
        {
            DynamoDBContext context = new DynamoDBContext(dynamoDBClient);
            await context.SaveAsync(obj);
            var response = await context.LoadAsync<T>(obj);
            return response;
        }

        [HttpGet("Orders")]
        public async Task<List<Orders>> GetOrders()
        {
            var response = await GetDynamoDBFullData<Orders>();
            return response;
        }

        public async Task<List<T>> GetDynamoDBFullData<T>() where T : class
        {
            DynamoDBContext contect = new DynamoDBContext(dynamoDBClient);
            var conditions = new List<ScanCondition>();

            var result = await contect.ScanAsync<T>(conditions).GetRemainingAsync();
            return result;
        }

        [HttpGet("GetOrdersById /{id}")]
        public async Task<Orders> GetOrdersById(int id)
        {
            var response = await GetDynamoDbDataById<Orders>(id);
            return response;
        }

        public async Task<T> GetDynamoDbDataById<T>(int id) where T : class
        {
            DynamoDBContext contect = new DynamoDBContext(dynamoDBClient);
            //var conditions = new List<ScanCondition>();

            var result = await contect.LoadAsync<T>(id);
            return result;
        }

        [HttpGet("Delete /{id}")]
        public async Task DeleteOrder(int id)
        {
            await Delete<Orders>(id);
        }

        public async Task Delete<T>(int id) where T : class
        {
            DynamoDBContext contect = new DynamoDBContext(dynamoDBClient);

            await contect.DeleteAsync<T>(id);
        }

    }
}
