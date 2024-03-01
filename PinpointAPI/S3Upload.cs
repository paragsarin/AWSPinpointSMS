using Amazon.S3.Model;
using Amazon.S3;
using Amazon;

class UploadObjectTest
{
    
    
    
    


   public static async Task<string> BucketAvailable(string bucket)
    {
        AmazonS3Client s3Client = new AmazonS3Client();

        string BucketName = "claim." + bucket.Replace("+","");
        try
        {
            await s3Client.GetBucketNotificationAsync(BucketName);
            return BucketName;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Bucket not found");
            PutBucketRequest request = new PutBucketRequest();
            request.BucketName = BucketName;
            await s3Client.PutBucketAsync(request);

            return BucketName;
        }
    }
    static async Task<string> GetFileContentAsync(IAmazonS3 client, string bucketName, string keyName)
    {
        GetObjectRequest request = new GetObjectRequest
        {
            BucketName = bucketName,
            Key = keyName
        };

        try
        {
            using (GetObjectResponse response = await client.GetObjectAsync(request))
            using (Stream responseStream = response.ResponseStream)
            using (StreamReader reader = new StreamReader(responseStream))
            {
                return await reader.ReadToEndAsync();
            }
        }
        catch
        {
            return "";
        }
    }

    public static async Task WritingAnObjectAsync(string bucketName,string content,string keyname,string region)
    {
        RegionEndpoint bucketRegion =  RegionEndpoint.GetBySystemName(region);

         IAmazonS3 client = new AmazonS3Client(bucketRegion);
        // Get the existing content of the file
        string existingContent = await GetFileContentAsync(client, bucketName, keyname);

        // Append the new data
        content = existingContent + Environment.NewLine + content;
   
        try
        {
            // 1. Put object-specify only key name for the new object.
            var putRequest1 = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = keyname,
                ContentBody = content,
                ContentType = "text/json"
            };

            PutObjectResponse response1 = await client.PutObjectAsync(putRequest1);

            
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine(
                    "Error encountered ***. Message:'{0}' when writing an object"
                    , e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(
                "Unknown encountered on server. Message:'{0}' when writing an object"
                , e.Message);
        }
    }
}
