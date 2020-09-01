using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace LetsTalkToCBYK
{
    class Program
    {
        private const string HOSTNAME = "localhost";
        private const int PORT = 5672;

        private const string QUEUE_NAME = "CBYK";
        private const string ROUTING_KEY = "CBYK";

        private static string _microServiceName;

        public static string MicroServiceName
        {
            get { return _microServiceName; }
            set
            {
                if (!string.IsNullOrEmpty(value.Trim()))
                    _microServiceName = value;
                else
                    _microServiceName = $"CBYK-{GetTimeStamp().ToString()}";
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Setting up Microservice... \n");

            Console.Write("What's the microservice name ? (default = 'CBYK-timeStamp'): ");
            var microServiceName = Console.ReadLine();

            MicroServiceName = microServiceName;

            Setup();

            Console.ReadKey();
        }

        private static void Setup()
        {
            Console.WriteLine($"Setting hostname {HOSTNAME} and port {PORT}... \n");

            var factory = new ConnectionFactory()
            {
                HostName = HOSTNAME,
                Port = PORT
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            Console.WriteLine($"Working on {factory.HostName} port {factory.Port}... \n");

            Console.WriteLine("Declaring queue... \n");
            channel.QueueDeclare(queue: QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);

            SendSender(channel);
            SetupReceiver(channel);
        }

        private static void SetupReceiver(IModel channel)
        {
            Console.WriteLine("Setting up receiver... \n");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += OnMessageReceived;

            channel.BasicConsume(queue: QUEUE_NAME, autoAck: true, consumer: consumer);
        }

        private static void OnMessageReceived(object model, BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            var message = JsonConvert.DeserializeObject<Message>(json);

            var formattedDate = ConvertUnixTimeStampToDateTime(message.TimeStamp).ToString("dd/MM/yyyy HH:mm");

            Console.WriteLine($"Message received from {message.Sender} with content: {message.Content} sent: {formattedDate} \n");
        }

        private static string ToJson(object content)
        {
            return JsonConvert.SerializeObject(content);
        }

        private static Message GetMessage()
        {
            return new Message
            {
                Content = "Hello World",
                Id = Guid.NewGuid(),
                Sender = MicroServiceName,
                TimeStamp = GetTimeStamp()
            };
        }

        private static double GetTimeStamp()
        {
            return (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        }

        private static void SendSender(IModel channel)
        {
            Console.WriteLine("Setting up sender...");

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(5);

            var message = GetMessage();

            var body = Encoding.UTF8.GetBytes(ToJson(message));

            var timer = new System.Threading.Timer((e) =>
            {
                if (channel != null)
                {
                    channel.BasicPublish(exchange: "", routingKey: ROUTING_KEY, basicProperties: null, body: body);

                    Console.WriteLine($"{MicroServiceName} sent a Hello World... \n");
                }
            }, null, startTimeSpan, periodTimeSpan);
        }

        private static DateTime ConvertUnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
