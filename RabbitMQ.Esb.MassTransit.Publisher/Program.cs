using MassTransit;
using RabbitMQ.Esb.MassTransit.Shared.Messages;

string rabbitMQUri=("amqps://jvthiwve:DP2b7DOnUrXKFroJOwM0wvu_UWDlIO-_@sparrow.rmq.cloudamqp.com/jvthiwve");

string queueName = "example-queue";

IBusControl bus=Bus.Factory.CreateUsingRabbitMq(factor =>
{
    factor.Host(rabbitMQUri);
});

ISendEndpoint sendEndpoint = await bus.GetSendEndpoint(new ($"{rabbitMQUri}/{queueName}"));

Console.Write("Mesajı giriniz: ");
string message = Console.ReadLine();

await sendEndpoint.Send<IMessage>(new ExampleMessage
{
    Text = message
});
Console.Read();