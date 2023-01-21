// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;



Console.WriteLine("Hello, World!");

var factory = new ConnectionFactory
{
    Port = 5672,

    Uri = new Uri("amqp://guest:guest@localhost:5672/")
};

using var conn = factory.CreateConnection(); //Bağlantı oluşturduk

var channel = conn.CreateModel(); // Kanal oluşturduk.

channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic); // ismi ,res atınca kaybolmaz ,


Enumerable.Range(1, 50).ToList().ForEach(x =>
{

    var random = new Random();

    LogNames log1 = (LogNames)random.Next(1, 4);
    LogNames log2 = (LogNames)random.Next(1, 4);
    LogNames log3 = (LogNames)random.Next(1, 4);

    string msg = $"Mesaj: Log {log1} - {log2} -{log3}";

    var msgBody = Encoding.UTF8.GetBytes(msg);

    var routeKey = $"{log1}.{log2}.{log3}";

    channel.BasicPublish("logs-topic", routeKey, null, msgBody); // exchange adı verilir.s

    Console.WriteLine($"Mesaj İletildi.:{msg}");

});

Console.ReadLine();