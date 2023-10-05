using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://jvthiwve:DP2b7DOnUrXKFroJOwM0wvu_UWDlIO-_@sparrow.rmq.cloudamqp.com/jvthiwve");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "header-exchange-example", type: ExchangeType.Headers);

for (int i = 0; i < 100; i++)
{
    byte[] message = Encoding.UTF8.GetBytes($"Merhaba {i}");
    Console.WriteLine("Lütfen header valuesini giriniz");
    string value = Console.ReadLine();

    IBasicProperties basicProperties = channel.CreateBasicProperties();
    basicProperties.Headers = new Dictionary<string, object> {
        ["no"] = value
    };

    channel.BasicPublish(exchange: "header-exchange-example", routingKey: string.Empty, body: message, basicProperties: basicProperties);

}