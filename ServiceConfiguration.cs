namespace AWSServerlessDB
{
    public class ServiceConfiguration
    {
        public AWSS3Configuration AWSS3 { get; set; }
    }
    public class AWSS3Configuration
    {
        public string BucketName { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }

    }
}