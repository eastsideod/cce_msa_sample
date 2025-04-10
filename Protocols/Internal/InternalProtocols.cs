using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using static Protocols.Internal.InternalProtocolBase;

namespace Protocols.Internal
{
    public class InternalProtocolBase
    {
        public enum MessageTypes
        {
            LoginReq = 1,
            LoginRes = 2,
            LogoutReq = 3,
            LogoutRes = 4,
        }

        public enum ErrorCode
        {
            SUCCESS = 0,
        }

        public int MessageType { get; set; }
    }

    public class InternalProtocols : InternalProtocolBase
    {
    }

    public class LoginReq : InternalProtocolBase
    {
        public string Id { get; set; }
    }


    public class LoginRes : InternalProtocolBase
    {
        public ErrorCode ErrorCode { get; set; }
    }

    public class LogoutReq : InternalProtocolBase
    {
    }

    public class LogoutRes : InternalProtocolBase
    {
    }


    public class DepositReq : InternalProtocolBase
    {
    }

    public class DepositRes : InternalProtocolBase
    {
    }
}
