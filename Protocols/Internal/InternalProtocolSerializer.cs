using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Protocols.Internal.InternalProtocolBase;

namespace Protocols.Internal;

public class InternalProtocolSerializer
{
    public static InternalProtocolBase? Deserialize(
        int messageType,
        ReadOnlyMemory<byte> body)
    {
        var doc = JsonDocument.Parse(body);

        switch ((MessageTypes)messageType)
        {
            case MessageTypes.LoginReq:
                return doc.Deserialize<LoginReq>();

            case MessageTypes.LoginRes:
                return doc.Deserialize<LoginRes>();

            case MessageTypes.LogoutReq:
                return doc.Deserialize<LogoutReq>();

            case MessageTypes.LogoutRes:
                return doc.Deserialize<LogoutRes>();

        }

        return null;
    }

    public static byte[] Serialize(InternalProtocolBase msg)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(msg));
    }
}
