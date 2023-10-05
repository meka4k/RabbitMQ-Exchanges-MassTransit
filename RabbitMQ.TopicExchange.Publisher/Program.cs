using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://jvthiwve:DP2b7DOnUrXKFroJOwM0wvu_UWDlIO-_@sparrow.rmq.cloudamqp.com/jvthiwve");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "topic-exchange-example", type: ExchangeType.Topic);

for (int i = 0; i < 10; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    Console.Write("Mesajı Göndereceğiniz Topic Formatını Belirleyiniz :");
    string topic =Console.ReadLine();
    channel.BasicPublish(exchange: "topic-exchange-example", routingKey: topic, body: message);
}
Console.Read();