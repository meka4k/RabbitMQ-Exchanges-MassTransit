using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new("amqps://jvthiwve:DP2b7DOnUrXKFroJOwM0wvu_UWDlIO-_@sparrow.rmq.cloudamqp.com/jvthiwve");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P (Point to Point) Yaklaşımı
//string queueName = "p2p-queue-example";

//channel.QueueDeclare(queue: queueName,autoDelete:false,exclusive:false,durable:false);

//byte[] message = Encoding.UTF8.GetBytes("Merhaba");
//channel.BasicPublish(exchange:string.Empty,routingKey:queueName,body:message);

#endregion

#region Publish/Subscribe Yaklaşımı
//string exchangeName = "example-pub-sub-exchange";

//channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout);
//for (int i = 0; i < 50; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba "+ i);
//    channel.BasicPublish(exchange: exchangeName, body: message,routingKey:string.Empty);
//}
#endregion

#region Work Queue Yaklaşımı

//string queueName = "example-work-queue";
//channel.QueueDeclare(queueName, false, false, false);

//for (int i = 0; i < 50; i++)
//{
//    await Task.Delay(200);
//    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
//    channel.BasicPublish(exchange: string.Empty, body: message, routingKey: queueName);
//}

#endregion

#region Request/Response Yaklaşımı

string requestQueueName = "example-request-response-queue";
channel.QueueDeclare(queue: requestQueueName, false, false, false);

string replyQueueName = channel.QueueDeclare().QueueName;
string correlationId=Guid.NewGuid().ToString();

//Request Mesajını oluşturma ve gönderme
IBasicProperties basicProperties = channel.CreateBasicProperties();
basicProperties.ReplyTo = replyQueueName;
basicProperties.CorrelationId = correlationId;
for (int i = 0; i < 10; i++)
{
    await Task.Delay(200);
    byte[] message = Encoding.UTF8.GetBytes("Merhaba " + i);
    channel.BasicPublish(exchange:string.Empty, body: message, routingKey: requestQueueName,basicProperties:basicProperties);

}

//response kuyruğu dinleme
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: replyQueueName, true, consumer);

consumer.Received += (sender, e) =>
{
    if (basicProperties.CorrelationId == correlationId)
    {

        Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
    }
};




#endregion



Console.Read();