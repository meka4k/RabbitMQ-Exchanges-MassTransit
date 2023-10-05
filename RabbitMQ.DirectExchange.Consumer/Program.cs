using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://jvthiwve:DP2b7DOnUrXKFroJOwM0wvu_UWDlIO-_@sparrow.rmq.cloudamqp.com/jvthiwve");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//1.adım
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

//2.adım
string queueName = channel.QueueDeclare().QueueName;

//3.adım
channel.QueueBind(queue: queueName, exchange: "direct-exchange-example", routingKey: "direct-queue-example");

EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();


//1. adım: publisherdaki exchange ile birebir aynı isim ve type'a sahip bir exchange tanımlanmalıdır.

//2. adım: publisher tarafından routing keyde bulunan değerdeki kuyruğa gönderilen mesajları, kendi oluşturduğumuz
//kuyruğa yönlendirerek tüketmemiz gerekmektedir. bir kuyruk oluşturulmalıdır.

//3. adım: consume işlemleri ve mesajı okuma 