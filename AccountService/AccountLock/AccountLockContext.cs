namespace AccountService.AccountLock;

public enum LockType
{
    SingleAccount,
    MultipleAccount,
}

public class AccountLockContext
{
    public LockType LockType { get; set; }

    public int RequiredLockCount { get; init; }

    public int CurrentLockedCount { get; set; }

    public bool IsReadLock { get; set; }
    
    public Action OnLocked { get; init; } = null!;
}
