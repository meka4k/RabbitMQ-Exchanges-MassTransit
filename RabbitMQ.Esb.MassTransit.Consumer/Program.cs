using MassTransit;
using RabbitMQ.Esb.MassTransit.Consumer.Consumers;

string rabbitMQUri = ("amqps://jvthiwve:DP2b7DOnUrXKFroJOwM0wvu_UWDlIO-_@sparrow.rmq.cloudamqp.com/jvthiwve");

string queueName = "example-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factor =>
{
    factor.Host(rabbitMQUri);

    factor.ReceiveEndpoint(queueName, endpoint =>
    {
        endpoint.Consumer<ExampleMessageConsumer>();
    });
});
await bus.StartAsync();
Console.Read();
