using Amazon.Runtime.Internal.Util;
using aws_s3_digitalocean_spaces_basic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ILogger = Microsoft.Extensions.Logging.ILogger;

using System.Net;

namespace aws_s3_digitalocean_spaces_basic.Controllers
{
    [ApiController]
    [Route("api/bucket")]
    public class DigitalOceanSpacesController : ControllerBase
    {
        private readonly IAppConfiguration _appConfiguration;
        private readonly ILogger<DigitalOceanSpacesController> _logger;
        private IAws3Services _aws3Services;

        public DigitalOceanSpacesController(IAppConfiguration appConfiguration,
                                            ILogger<DigitalOceanSpacesController> logger)
        {
            _appConfiguration = appConfiguration;
            _logger = logger;
        }

        [HttpGet("{documentName}")]
        public IActionResult GetDocumentFromS3(string documentName)
        {
            _logger.LogInformation("Http GET from DigitalOceanSpacesController [GetDocumentFromS3]");

            try
            {
                if (string.IsNullOrEmpty(documentName))
                    return BadRequest("The 'documentName' parameter is required");
                   // return ReturnMessage("The 'documentName' parameter is required", (int)HttpStatusCode.BadRequest);

                _aws3Services = new Aws3Services(_appConfiguration.AccessKey,
                                                _appConfiguration.SecretAccessKey,
                                                _appConfiguration.SessionToken,
                                                _appConfiguration.Region,
                                                _appConfiguration.BucketName);

                var document = _aws3Services.DownloadFileAsync(documentName).Result;

                return File(document, "application/octet-stream", documentName);
            }
            catch (Exception ex)
            {
                 Console.WriteLine("Ocurrio un error: ",ex);
                return NotFound();
                //return ValidateException(ex);
            }
        }

        [HttpPost]
        public IActionResult UploadDocumentToS3(IFormFile file)
        {
            _logger.LogInformation("Http POST from DigitalOceanSpacesController [UploadDocumentToS3]");

            try
            {
                if (file is null || file.Length <= 0)
                    return BadRequest("file is required to upload");
                    //return ReturnMessage("file is required to upload", (int)HttpStatusCode.BadRequest);

                _aws3Services = new Aws3Services(_appConfiguration.AccessKey, _appConfiguration.SecretAccessKey, _appConfiguration.SessionToken, _appConfiguration.Region, _appConfiguration.BucketName);

                var result = _aws3Services.UploadFileAsync(file);

                return Created(string.Empty, (int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrio un error: ",ex.Message);
                return StatusCode(500);
               // return ReturnMessage(ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("{documentName}")]
        public IActionResult DeleteDocumentFromS3(string documentName)
        {
            _logger.LogInformation("Http DELETE from DigitalOceanSpacesController [DeleteDocumentFromS3]");

            try
            {
                if (string.IsNullOrEmpty(documentName))
                    return BadRequest("The 'documentName' parameter is required");
                    //return ReturnMessage("The 'documentName' parameter is required", (int)HttpStatusCode.BadRequest);

                _aws3Services = new Aws3Services(_appConfiguration.AccessKey, _appConfiguration.SecretAccessKey, _appConfiguration.SessionToken, _appConfiguration.Region, _appConfiguration.BucketName);

                var result = _aws3Services.DeleteFileAsync(documentName).Result;
                if (!result)
                {
                    return NotFound();
                }

                return Ok(string.Format("The document '{0}' is deleted successfully", documentName));
                //return ReturnMessage(string.Format("The document '{0}' is deleted successfully", documentName));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrio un error:", ex);
                return null;
            }
        }

    }
}
