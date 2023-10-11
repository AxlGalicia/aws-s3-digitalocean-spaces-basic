using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using aws_s3_digitalocean_spaces_basic.Interfaces;
using System.Net;

namespace aws_s3_digitalocean_spaces_basic
{
    public class Aws3Services : IAws3Services
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _S3Client;
        private readonly AmazonS3Config _S3Config;

        public Aws3Services(string AccessKeyId, string SecretAccessKey, string SessionToken, string region, string bucketName)
        {
            _bucketName = bucketName;
            _S3Config = new AmazonS3Config()
            {
                ServiceURL = "https://prueba-space.nyc3.digitaloceanspaces.com",
                SignatureVersion = "V4"
            };
            //_S3Client = new AmazonS3Client(AccessKeyId, SecretAccessKey, SessionToken, RegionEndpoint.GetBySystemName(region));
            _S3Client = new AmazonS3Client(AccessKeyId, SecretAccessKey, _S3Config);
        }

        public Task<bool> DeleteFileAsync(string fileName, string versionId = "")
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> DownloadFileAsync(string file)
        {
            MemoryStream ms = null;

            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = file
                };

                using (var response = await _S3Client.GetObjectAsync(getObjectRequest))
                {
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        using (ms = new MemoryStream())
                        {
                            await response.ResponseStream.CopyToAsync(ms);
                        }
                    }
                }

                if (ms is null || ms.ToArray().Length < 1)
                    throw new FileNotFoundException(string.Format("The document '{0}' is not found", file));

                return ms.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UploadFileAsync(IFormFile file)
        {
            try
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = file.FileName,
                        BucketName = _bucketName,
                        ContentType = file.ContentType
                    };

                    var fileTransferUtility = new TransferUtility(_S3Client);

                    await fileTransferUtility.UploadAsync(uploadRequest);

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ocurrio un error: ", e.Message);
                return false;
            }
        }
    }
}
