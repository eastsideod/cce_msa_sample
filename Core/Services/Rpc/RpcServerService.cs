using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Connector;
using Core.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Core.Services.Rpc;

public class RpcServerService
{
    private RpcServerSettings _settings;
    private RabbitMQConnectorBase _connector;
    private RpcHandlerRegistryService _handlerRegistry;

    public RpcServerService(IOptions<RpcServerSettings> settings, RpcHandlerRegistryService handlerRegistry)
    {
        _settings = settings.Value;
    }

    public async void Start()
    {
        _connector = await RabbitMQConnectorFactory.Create(_settings.BackendMQSettings);
    }
}
