using Amazon.SQS;
using Amazon.SQS.Model;
using MassTransit;
using Newtonsoft.Json;
using PinPoint;
using PinpointAPI.Contracts;
using System.Security.AccessControl;
using Message = Amazon.SQS.Model.Message;

public class QueueBackgroundTask : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromSeconds(2);
    private readonly ILogger<QueueBackgroundTask> _logger;
    private readonly IConfiguration _config;
    private readonly IAmazonSQS _Sqs;
   

    public QueueBackgroundTask(ILogger<QueueBackgroundTask> logger, IConfiguration config, IAmazonSQS Sqs)
    {
        _logger = logger;
        _Sqs = Sqs;
        _config= config;
    }
    private const int MaxMessages = 1;
    private const int WaitTime = 2;

    private static async Task<ReceiveMessageResponse> GetMessage(
      IAmazonSQS sqsClient, string qUrl, int waitTime = 0)
    {
        return await sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
        {
            QueueUrl = qUrl,
            MaxNumberOfMessages = MaxMessages,
            WaitTimeSeconds = waitTime
 
        });
    }


   
    // Method to process a message
   
    private static async Task<bool> ProcessMessage(Message message,string region)
    {
  

        Root dynamicObject = JsonConvert.DeserializeObject<Root>(message.Body);





        Messages messages = JsonConvert.DeserializeObject<Messages>(dynamicObject.Message);
        var body = messages.messageBody;
        var originationNumber = messages.originationNumber;
        body = "(" + originationNumber + ") - " + DateTime.Now.ToString() + " : " + messages.messageBody + Environment.NewLine;



       
        string bname = await UploadObjectTest.BucketAvailable(originationNumber);
        await UploadObjectTest.WritingAnObjectAsync(bname, body, originationNumber.Replace("+", "") + ".txt", region);
        return true;
    }
    private  async Task DeleteMessage(
          IAmazonSQS sqsClient, Message message, string qUrl)
    {
        _logger.LogInformation($"\nDeleting message {message.MessageId} from queue...");
        await sqsClient.DeleteMessageAsync(qUrl, message.ReceiptHandle);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);

        


        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
            var queue = _config.GetValue(typeof(string),"PinPoint:SQSQueue");
            var msg = await GetMessage(_Sqs, (string)queue, WaitTime);
            if (msg.Messages.Count != 0)
            {
                if (await ProcessMessage(msg.Messages[0],_config.GetValue<string>("PinPoint:region")))
                    _logger.LogInformation("Executing PeriodicBackgroundTask" + msg.Messages[0].Body);
                await DeleteMessage(_Sqs, msg.Messages[0], (string)queue);
            }

            await Task.Delay(1000);


        }

    }
}