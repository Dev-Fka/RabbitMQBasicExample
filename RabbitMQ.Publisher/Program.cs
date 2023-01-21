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

channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct); // ismi ,res atınca kaybolmaz ,

Enum.GetNames(typeof(LogNames)).ToList().ForEach(
    x =>
    {
        var routeKey = $"route-{x}";

        var queueName = $"direct-queue {x}";

        channel.QueueDeclare(queueName, true, false, false);

        channel.QueueBind(queueName, "logs-direct", routeKey, null);



    }
    );

Enumerable.Range(1, 50).ToList().ForEach(x =>
{
    LogNames log = (LogNames)new Random().Next(1, 4);

    string msg = $"Mesaj: Log {log}";

    var msgBody = Encoding.UTF8.GetBytes(msg);

    var routeKey = $"route-{log}";

    channel.BasicPublish("logs-direct", routeKey, null, msgBody); // exchange adı verilir.s

    Console.WriteLine($"Mesaj İletildi.:{msg}");

});

Console.ReadLine();