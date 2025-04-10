namespace AccountService.AccountLock;

public class AccountLockService
{
    private readonly Dictionary<long, AccountLock> _lockTable = new();

    public void LockAsync(long id, Action onLocked) 
    {
        var accountLock = GetLock(id);

        accountLock.LockAsync(new AccountLockContext 
        {
            LockType = LockType.SingleAccount,
            RequiredLockCount = 1,
            CurrentLockedCount = 0,
            IsReadLock = false,
            OnLocked = onLocked
        });
    }

    public void Unlock(long id) 
    {
        var accountLock = GetLock(id);
        accountLock.Unlock();
    }

    public void LockAsyncForTrade(
        long buyerId, 
        long sellerId,
        Action onLocked)
    {
        var locks = GetMultipleLocks(buyerId, sellerId);

        var first = locks.buyer.Id < locks.seller.Id ?
            locks.buyer : locks.seller;

        var second = locks.buyer.Id < locks.seller.Id ?
            locks.seller : locks.buyer;

        first.LockAsync(new AccountLockContext
        {
            LockType = LockType.MultipleAccount,
            RequiredLockCount = 2,
            CurrentLockedCount = 0,
            IsReadLock = false,
            OnLocked = onLocked
        });

        second.LockAsync(new AccountLockContext
        {
            LockType = LockType.MultipleAccount,
            RequiredLockCount = 2,
            CurrentLockedCount = 0,
            IsReadLock = false,
            OnLocked = onLocked
        });
    }

    public void UnlockForTrade(long buyerId, long sellerId)
    {
        var locks = GetMultipleLocks(buyerId, sellerId);

        var first = locks.buyer.Id < locks.seller.Id ?
            locks.buyer : locks.seller;

        var second = locks.buyer.Id < locks.seller.Id ?
            locks.seller : locks.buyer;

        first.Unlock();
        second.Unlock();
    }

    private AccountLock GetLock(long id)
    {
        lock (_lockTable)
        {
            return GetLockImpl(id);
        }
    }


    private (AccountLock buyer, AccountLock seller) GetMultipleLocks(long buyerId, long sellerId)
    {
        lock (_lockTable)
        {
            var buyer = GetLockImpl(buyerId);
            var seller = GetLockImpl(sellerId);

            return (buyer, seller);
        }
    }

    private AccountLock GetLockImpl(long id)
    {
        if (_lockTable.TryGetValue(id, out var accountLock))
        {
            return accountLock;
        }

        accountLock = new AccountLock(id);
        _lockTable[id] = accountLock;

        return accountLock;
    }
}
