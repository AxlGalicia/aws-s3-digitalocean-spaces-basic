namespace aws_s3_digitalocean_spaces_basic.Interfaces
{
    public interface IAppConfiguration
    {
        string AccessKey { get; set; }
        string SecretAccessKey { get; set; }
        string? SessionToken { get; set; }
        string BucketName { get; set; }
        string? Region { get; set; }

    }
}
