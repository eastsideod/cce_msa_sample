using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using RabbitMQ.Client.Events;
using Protocols.Internal;
using Core.Connector;
using static Protocols.Internal.InternalProtocolBase;

namespace Core.Services.Rpc;

public class RpcRabbitMQConnector : RabbitMQConnectorBase
{
    private readonly RpcHandlerRegistryService _handlerRegistry;

    public RpcRabbitMQConnector(RpcHandlerRegistryService handlerRegistry)
    {
        _handlerRegistry = handlerRegistry;
    }

    public override ValueTask PublishMessage(InternalProtocolBase message)
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        return _channel.BasicPublishAsync(
            exchange: "",
            routingKey: _queueName,
            mandatory: true,
            body: body);
    }

    protected override async Task OnConsumed(
        object o, BasicDeliverEventArgs ea)
    {
        string response = string.Empty;

        var props = ea.BasicProperties;
        var replyProps = new BasicProperties();
        replyProps.CorrelationId = props.CorrelationId;

        try
        {
            var messageId = Int32.Parse(props.MessageId);
            var message = InternalProtocolSerializer.Deserialize(
                Int32.Parse(props.MessageId), ea.Body);

            if (message == null)
            {
                return;
            }

            var handler = _handlerRegistry.Get((MessageTypes)messageId);
            if (handler == null)
            {
                return;
            }

            var resp = await handler(message);
            var responseBytes = InternalProtocolSerializer.Serialize(resp);

            _channel.BasicPublishAsync(
                exchange: "",
                routingKey: props.ReplyTo,
                mandatory: false,
                basicProperties: replyProps,
                body: responseBytes);

            _channel.BasicAckAsync(
                deliveryTag: ea.DeliveryTag,
                multiple: false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] Error: {ex.Message}");
            
            _channel.BasicNackAsync(
                deliveryTag: ea.DeliveryTag,
                multiple: false,
                requeue: false);
        }
    }
}
