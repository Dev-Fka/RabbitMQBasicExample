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

channel.QueueDeclare("hello-rabbitmq", true, false, false); // kuyruk resde silinmez,başka kanallardan erişilir,otomatik silinmesin dinleyen olmazsa.

Enumerable.Range(1, 50).ToList().ForEach(
    x =>
    {
        string msg = $"Mesaj {x}";

        var msgBody = Encoding.UTF8.GetBytes(msg);

        channel.BasicPublish(string.Empty, "hello-rabbitmq", null, msgBody);

        Console.WriteLine($"Mesaj İletildi.:{msg}");

    }
    );


Console.ReadLine();