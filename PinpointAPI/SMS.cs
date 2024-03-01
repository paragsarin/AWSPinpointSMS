using Amazon;
using Amazon.Pinpoint;
using Amazon.Pinpoint.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
//using SendMessage;
using System.Dynamic;

namespace PinPoint
{
    class SendSMS
    {

        private static string destinationNumber = "";

        private static string message = "";


        public static async Task SendMessage(string number,string _message,IConfiguration configuration)
        {

            string messageType = configuration.GetValue<string>("PinPoint:messageType");
            string appId = configuration.GetValue<string>("PinPoint:appId");
            string region = configuration.GetValue<string>("PinPoint:region");
            string registeredKeyword = configuration.GetValue<string>("PinPoint:registeredKeyword");
            string originationNumber = configuration.GetValue<string>("PinPoint:originationNumber");
            string senderId = configuration.GetValue<string>("PinPoint:senderId");
            if (number.Trim().Length!=0 )
            {
                destinationNumber = number;
            }
            if (_message.Trim().Length != 0)
            {
                message = _message;
            }
            // Create the Amazon PinPoint client
            using (AmazonPinpointClient client = new AmazonPinpointClient(RegionEndpoint.GetBySystemName(region)))
            {
                SendMessagesRequest sendRequest = new SendMessagesRequest
                {
                    ApplicationId = appId,
                    MessageRequest = new MessageRequest
                    {
                        Addresses = new Dictionary<string, AddressConfiguration>
                    {
                        {
                            destinationNumber,
                            new AddressConfiguration
                            {
                                ChannelType = "SMS"
                            }
                        }
                    },
                        MessageConfiguration = new DirectMessageConfiguration
                        {
                            SMSMessage = new SMSMessage
                            {
                                Body = message,
                                MessageType = messageType,
                                OriginationNumber = originationNumber,
                                SenderId = senderId,
                                Keyword = registeredKeyword
                            }
                        }
                    }
                };
                try
                {
                    Console.WriteLine("Sending message...");

                    SendMessagesResponse response = await client.SendMessagesAsync(sendRequest);
                    string bname = await UploadObjectTest.BucketAvailable(destinationNumber);
                    message = "("+originationNumber +") - "+DateTime.Now.ToString() + " : " + message + Environment.NewLine;
                    await UploadObjectTest.WritingAnObjectAsync(bname, message, destinationNumber.Replace("+", "") + ".txt",region);
                    Console.WriteLine("Message sent!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The message wasn't sent. Error message: " + ex.Message);
                }
            }
        }
    }
}
