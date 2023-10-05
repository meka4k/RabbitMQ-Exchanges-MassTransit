using MassTransit;
using RabbitMQ.Esb.MassTransit.RequestResponse.Consumer.Consumers;
Console.WriteLine("Consumer");
string rabbitMQUri = ("amqps://jvthiwve:DP2b7DOnUrXKFroJOwM0wvu_UWDlIO-_@sparrow.rmq.cloudamqp.com/jvthiwve");
string requestQueue = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factor =>
{
    factor.Host(rabbitMQUri);
    factor.ReceiveEndpoint(requestQueue, endpoint =>
    {
        endpoint.Consumer<RequestMessageConsumer>();
    });
});

await bus.StartAsync();

Console.Read();

