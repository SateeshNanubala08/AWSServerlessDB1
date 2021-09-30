using Amazon.DynamoDBv2.DataModel;

namespace AWSServerlessDB.Modals
{


    [DynamoDBTable("Orders")]
    public class Orders
    {
        [DynamoDBHashKey]
        public int Id { get; set; }

        [DynamoDBProperty]
        public string Name { get; set; }

        [DynamoDBProperty]
        public int Price { get; set; }


    }
}