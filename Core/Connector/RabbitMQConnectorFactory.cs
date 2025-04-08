using RabbitMQ.Client;
using Core.Settings;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Options;

namespace Core.Connector;

public class RabbitMQConnectorFactory
{   
    public static async Task<RabbitMQConnectorBase?> Create(RabbitMQSettings settings)
    {
        try
        {
            var conn = new RabbitMQConnectorBase();

            await conn.Initialize(settings);
            return conn;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}