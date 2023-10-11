namespace aws_s3_digitalocean_spaces_basic.Interfaces
{
    public interface IAws3Services
    {

        Task<byte[]> DownloadFileAsync(string file);

        Task<bool> UploadFileAsync(IFormFile file);

        Task<bool> DeleteFileAsync(string fileName, string versionId = "");
    }
}
