using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://jvthiwve:DP2b7DOnUrXKFroJOwM0wvu_UWDlIO-_@sparrow.rmq.cloudamqp.com/jvthiwve");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P (Point to Point) Yaklaşımı
//string queueName = "p2p-queue-example";

//channel.QueueDeclare(queue: queueName, autoDelete: false, exclusive: false, durable: false);

//EventingBasicConsumer consumer = new (channel);
//channel.BasicConsume(queue: queueName, autoAck:false, consumer:consumer);

//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};

#endregion

#region Publish/Subscribe Yaklaşımı
//string exchangeName = "example-pub-sub-exchange";

//channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);

//string queueName=channel.QueueDeclare().QueueName;
//channel.QueueBind(exchange: exchangeName, queue: queueName,routingKey:string.Empty);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: queueName, false, consumer) ;

//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};



#endregion

#region Work Queue Yaklaşımı
//string queueName = "example-work-queue";
//channel.QueueDeclare(queueName, false, false, false);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(queue: queueName, true, consumer);

//channel.BasicQos(prefetchCount: 1,
//    prefetchSize: 0
//   , global: false);

//consumer.Received += (sender, e) =>
//{
//    Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};


#endregion

#region Request/Response yaklaşımı

string requestQueueName = "example-request-response-queue";
channel.QueueDeclare(queue: requestQueueName, false, false, false);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue:requestQueueName, consumer:consumer,autoAck:true);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);

    byte[] messageResponse=Encoding.UTF8.GetBytes($"İşlem Tamamlandı: {message}");
    IBasicProperties properties = channel.CreateBasicProperties();
    properties.CorrelationId=e.BasicProperties.CorrelationId;
    channel.BasicPublish(exchange:string.Empty
        ,routingKey:e.BasicProperties.ReplyTo,basicProperties:properties,body:messageResponse);
};
#endregion

Console.Read();