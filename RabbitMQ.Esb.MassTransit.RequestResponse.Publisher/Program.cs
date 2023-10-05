using MassTransit;
using RabbitMQ.Esb.MassTransit.Shared.RequestResponseMessages;
Console.WriteLine("Publisher");
string rabbitMQUri = ("amqps://jvthiwve:DP2b7DOnUrXKFroJOwM0wvu_UWDlIO-_@sparrow.rmq.cloudamqp.com/jvthiwve");
string requestQueue = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factor =>
{
    factor.Host(rabbitMQUri);
});

await bus.StartAsync();
var request = bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUri}/{requestQueue}"));

int i = 1;
while (true)
{
    var response = await request.GetResponse<ResponseMessage>(new() { MessageNo = i, Text = $"{i++}. request" });
    Console.WriteLine($"Response Received: {response.Message.Text}");
}

Console.Read();


