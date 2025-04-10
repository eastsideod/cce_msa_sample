using System.Diagnostics;

namespace AccountService.AccountLock;

public class AccountLock
{
    private readonly Queue<AccountLockContext> _jobs = new();
    private AccountLockContext? _currentJob = null;

    public AccountLock(long id)
    {
        Id = id;
    }

    public long Id { get; init; }

    public void LockAsync(AccountLockContext context) 
    {
        AccountLockContext? nextLockContext = context;

        lock (_jobs)
        {
            if (_jobs.Count == 0 && _currentJob == null)
            {
                nextLockContext.CurrentLockedCount++;
                _currentJob = nextLockContext;

                if (nextLockContext.CurrentLockedCount != nextLockContext.RequiredLockCount)
                {
                    nextLockContext = null;
                }
            }
            else
            {
                nextLockContext = null;
                _jobs.Enqueue(context);
            }
        }

        if (nextLockContext != null)
        {
            nextLockContext.OnLocked();
        }
    }

    public void Unlock() 
    {
        AccountLockContext? nextLockContext = null;

        lock (_jobs)
        {
            _currentJob = null;

            if (_jobs.TryDequeue(out nextLockContext))
            {
                nextLockContext.CurrentLockedCount++;
                _currentJob = nextLockContext;

                if (nextLockContext.CurrentLockedCount != nextLockContext.RequiredLockCount)
                {
                    nextLockContext = null;
                }
            }
        }

        if (nextLockContext != null)
        {
            nextLockContext.OnLocked();
        }
    }

}
