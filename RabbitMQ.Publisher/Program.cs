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

channel.ExchangeDeclare("headerExchange", durable: true, type: ExchangeType.Topic); // ismi ,res atınca kaybolmaz ,

Dictionary<string, object> headers = new()
{
    { "format", "pdf" },
    { "shape", "a4" }
};

var properties = channel.CreateBasicProperties();
properties.Headers = headers;

channel.BasicPublish("headerExchange", string.Empty, properties, Encoding.UTF8.GetBytes("Mesajım"));

Console.WriteLine("Mesaj kuyruğa iletildi.");
Console.ReadLine();