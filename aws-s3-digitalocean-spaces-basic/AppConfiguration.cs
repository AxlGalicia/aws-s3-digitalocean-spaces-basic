using aws_s3_digitalocean_spaces_basic.Interfaces;

namespace aws_s3_digitalocean_spaces_basic
{
    public class AppConfiguration : IAppConfiguration
    {
        public string AccessKey { get; set; }
        public string SecretAccessKey { get; set; }
        public string? SessionToken { get; set; }
        public string BucketName { get; set; }
        public string? Region { get; set; }

        public AppConfiguration()
        {
            BucketName = "prueba-space.nyc3.digitaloceanspaces.com";
            Region = "";
            AccessKey = "DO00ELCLY3LRFMRYNEG9";
            SecretAccessKey = "CGZEdqMVTl4J8nNdSwNDE0YQIX7vyVt6usMmnk/uLJY";
            SessionToken = "";
        }
    }
}
