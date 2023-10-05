using MassTransit;
using RabbitMQ.Esb.MassTransit.Shared.RequestResponseMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Esb.MassTransit.RequestResponse.Consumer.Consumers
{
    internal class RequestMessageConsumer :IConsumer<RequestMessage>
    {
        public async Task Consume(ConsumeContext<RequestMessage> context)
        {
            Console.WriteLine(context.Message.Text);
            await context.RespondAsync<ResponseMessage>(new ResponseMessage() { Text = $"{context.Message.MessageNo}. response to request" });
        }
    }
}
