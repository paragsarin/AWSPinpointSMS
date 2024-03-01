namespace PinpointAPI.Contracts
{
    public class Root
    {
        public string Type { get; set; }
        public string MessageId { get; set; }
        public string TopicArn { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string SignatureVersion { get; set; }
        public string Signature { get; set; }
        public string SigningCertURL { get; set; }
        public string UnsubscribeURL { get; set; }
    }
    public class Messages
    {
        public string originationNumber { get; set; }
        public string destinationNumber { get; set; }
        public string messageKeyword { get; set; }
        public string messageBody { get; set; }
        public string previousPublishedMessageId { get; set; }
        public string inboundMessageId { get; set; }
    }

}
