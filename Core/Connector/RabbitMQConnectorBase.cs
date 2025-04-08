using RabbitMQ.Client;
using Core.Settings;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using RabbitMQ.Client.Events;
using System.Threading.Channels;
using Protocols.Internal;
using Core.Services.Rpc;

namespace Core.Connector;

public class RabbitMQConnectorBase : IDisposable
{
    protected string _queueName = string.Empty;
    protected IConnection _connection = null!;
    protected IChannel _channel = null!;
    protected AsyncEventingBasicConsumer? _consumer;
    
    public RabbitMQConnectorBase()
    {
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }

    public async Task Initialize(RabbitMQSettings settings)
    {
        var factory = new ConnectionFactory
        {
            HostName = settings.HostName,
            Port = settings.Port,
            UserName = settings.UserName,
            Password = settings.Password
        };

        _queueName = settings.QueueName;

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.QueueDeclareAsync(
            queue: settings.QueueName,
            durable: settings.QueueDurable,
            exclusive: settings.QueueExclusive,
            autoDelete: settings.QueueAutoDelete,
            arguments: null);

        await _channel.BasicQosAsync(
            (uint)settings.QueuePrefetchSize, 
            (ushort)settings.QueuePrefetchCount,
            false);

    }

    public void BeginConsume(AsyncEventHandler<BasicDeliverEventArgs> cb)
    {
        _consumer = new AsyncEventingBasicConsumer(_channel);

        _consumer.ReceivedAsync += this.OnConsumed;

        _channel.BasicConsumeAsync(
            queue: _queueName,
            autoAck: false,
            consumer: _consumer);
    }

    public abstract ValueTask PublishMessage<T>(T message);

    protected abstract Task OnConsumed(object o, BasicDeliverEventArgs ea);
}