using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Account;

public struct Balance
{
    public int CurrencyCode { get; set; }
    public Int128 Available { get; set; }
    public Int128 Locked { get; set; }
}
