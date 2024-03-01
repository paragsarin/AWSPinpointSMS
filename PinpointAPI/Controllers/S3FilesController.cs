using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;

namespace PinpointAPI.Controllers
{
    [ApiController]
    [Route("api/s3files")]
    public class S3FilesController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;
      


        public S3FilesController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

       
        [HttpGet("listfiles")]
        public async Task<IActionResult> GetFiles(string bucketname)
        {
            try
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = bucketname,
                };

                ListObjectsV2Response response = await _s3Client.ListObjectsV2Async(request);

                return Ok(response.S3Objects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("buckets")]
        public async Task<IActionResult> GetS3Buckets()
        {
            try
            {
                List<S3Object> buckets = await ListS3Buckets();
                return Ok(buckets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<List<S3Object>> ListS3Buckets()
        {
            List<string> buckets = new List<string>();
            
            ListBucketsResponse response = await _s3Client.ListBucketsAsync();
            List<S3Object> objects = new List<S3Object>();
            foreach (S3Bucket bucket in response.Buckets)
            {
                if (bucket.BucketName.StartsWith("claim."))
                {
                    IActionResult result = await GetFiles(bucket.BucketName);
                    if (result is OkObjectResult okObjectResult)
                    {
                        // Extract data from the OkObjectResult and convert it to List<string>
                        List<S3Object> listOfStrings = (List<S3Object>)okObjectResult.Value;
                        foreach (var file in listOfStrings)
                        {
                            objects.Add(file);
                        }
                    }
                }
            }

            return objects;
        }

        [HttpGet("read")]
        public async Task<IActionResult> ReadFile(string fileName,string bucketname)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketname,
                    Key =  fileName
                };

                using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string content = await reader.ReadToEndAsync();
                    return Ok(content);
                }
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return NotFound("File not found.");

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
