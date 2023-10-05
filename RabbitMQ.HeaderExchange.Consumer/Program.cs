using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://jvthiwve:DP2b7DOnUrXKFroJOwM0wvu_UWDlIO-_@sparrow.rmq.cloudamqp.com/jvthiwve");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "header-exchange-example", type: ExchangeType.Headers);

Console.WriteLine("Lütfen header valuesini girininiz.");
string value = Console.ReadLine();

string queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(queue: queueName, exchange: "header-exchange-example", routingKey: string.Empty, new Dictionary<string, object>
{
    ["no"] = value
});

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: queueName, true, consumer: consumer);
consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();