using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Account;

internal class Account
{
    private readonly Dictionary<int, Balance> _balanceMap = new();

    public Account(long id, Dictionary<int, Balance> balanceMap)
    {
        Id = id;
        _balanceMap = balanceMap;
    }

    public long Id { get; init; }

    public void Deposit(Balance balance)
    {
        if (_balanceMap.TryAdd(balance.CurrencyCode, balance))
        {
            return;
        }

        var currentBalance = _balanceMap[balance.CurrencyCode];
        Debug.Assert(currentBalance.CurrencyCode == balance.CurrencyCode);
        Debug.Assert(currentBalance.Available >= 0);
        Debug.Assert(currentBalance.Locked >= 0);

        _balanceMap[balance.CurrencyCode] = new Balance
        {
            CurrencyCode = balance.CurrencyCode,
            Available = currentBalance.Available + balance.Available,
            Locked = currentBalance.Locked,
        };
    }

    public bool Withdraw(Balance balance)
    {
        if (_balanceMap.TryGetValue(balance.CurrencyCode, out var currentBalance) == false)
        {
            return false;
        }

        Debug.Assert(currentBalance.CurrencyCode == balance.CurrencyCode);
        Debug.Assert(currentBalance.Available >= 0);
        Debug.Assert(currentBalance.Locked >= 0);

        if (currentBalance.Available - balance.Available < 0)
        {
            return false;
        }

        _balanceMap[balance.CurrencyCode] = new Balance
        {
            CurrencyCode = balance.CurrencyCode,
            Available = currentBalance.Available - balance.Available,
            Locked = currentBalance.Locked,
        };

        return true;
    }
}
