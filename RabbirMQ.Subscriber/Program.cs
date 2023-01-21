// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");

var factory = new ConnectionFactory
{
    Port = 5672,
    Uri = new Uri("amqp://guest:guest@localhost:5672/")
};

using var conn = factory.CreateConnection(); //Bağlantı oluşturduk

var channel = conn.CreateModel(); // Kanal oluşturduk.


channel.BasicQos(0, 1, false); //her alıcıya 1 mesaj atar 

//channel.QueueDeclare("hello-rabbitmq", true, false, false); // kuyruk resde silinmez,başka kanallardan erişilir,otomatik silinmesin dinleyen olmazsa.
// dinleyici böyle bir kuyruk yoksa oluşturur.

var consumer = new EventingBasicConsumer(channel);

var queueName = "direct-queue critical";

var a = channel.BasicConsume(queueName, false, consumer); //mesajı verince siler

Console.WriteLine("Kanal dinleniyor!");

consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var msg = Encoding.UTF8.GetString(e.Body.ToArray());

    Thread.Sleep(1000);

    Console.WriteLine("kuyruktran gelen mesaj : " + msg);

    File.AppendAllTextAsync("log-critical.txt", msg + "\n");

    channel.BasicAck(e.DeliveryTag, false); // gönderiiciye mesajı aldım der , 1 taneyi siler.
};

Console.ReadLine();