using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Protocols.Internal;
using static Protocols.Internal.InternalProtocolBase;

namespace Core.Services.Rpc;

public class RpcHandlerRegistryService
{
    private readonly Dictionary<MessageTypes, Func<InternalProtocolBase, Task<InternalProtocolBase>>> _handlers = new();

    private Func<InternalProtocolBase, Task<InternalProtocolBase>>? _defaultHandler = null;

    public bool TryAddHandler(
        MessageTypes messageType, 
        Func<InternalProtocolBase, Task<InternalProtocolBase>> 
            handler)
    {
        return _handlers.TryAdd(messageType, handler);
    }

    public void SetDefaultHandler(
        Func<InternalProtocolBase, Task<InternalProtocolBase>> 
            defaultHandler) 
    {
        _defaultHandler = defaultHandler;
    }

    public Func<InternalProtocolBase, Task<InternalProtocolBase>> 
        Get(MessageTypes messageType)
    {
        if (_handlers.TryGetValue(messageType, 
                                  out var handler) == false)
        {
            return _defaultHandler;
        }

        return handler;
    }
}
