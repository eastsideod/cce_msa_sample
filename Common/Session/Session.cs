using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Session;

public class UserSession
{
    public long Id { get; set; }
    public string DeviceId { get; set; } = string.Empty;

    public DateTime LastReceivedAt { get; set; }
}
