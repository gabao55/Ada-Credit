namespace AdaCreditDomain;

public sealed class Account
{
    public int Number { get; private set; }
    // public string Branch { get; private set; }

    public static Account CreateNewAccount()
    {
        return new Account
        {
            Number = 12345,
            // Branch = "0001",
        };
    }
}
