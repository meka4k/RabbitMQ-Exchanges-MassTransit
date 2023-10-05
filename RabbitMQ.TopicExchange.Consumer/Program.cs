using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://jvthiwve:DP2b7DOnUrXKFroJOwM0wvu_UWDlIO-_@sparrow.rmq.cloudamqp.com/jvthiwve");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "topic-exchange-example", type: ExchangeType.Topic);
Console.WriteLine("Dinlenecek Topic Formatını Yazınız");
string topic = Console.ReadLine();

string queue = channel.QueueDeclare().QueueName;
channel.QueueBind(queue: queue,
    exchange: "topic-exchange-example",
    routingKey:topic);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queue,true,consumer: consumer);
consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();