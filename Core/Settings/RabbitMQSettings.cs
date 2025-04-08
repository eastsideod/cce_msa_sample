namespace Core.Settings
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string QueueName { get; set; } = "api_requests";

        public bool QueueDurable { get; set; } = false;

        public bool QueueExclusive { get; set; } = false;

        public bool QueueAutoDelete { get; set; } = false;

        public int QueuePrefetchSize { get; set; } = 0;

        public int QueuePrefetchCount { get; set; } = 1;
    }
}