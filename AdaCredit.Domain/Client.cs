namespace AdaCreditDomain;

public sealed class Client
{
    public string Name { get; private set; }
    public long Document { get; private set; }
    public Account Account { get; private set; }

    public Client(string name, long document)
    {
        Name = name;
        Document = document;
        Account = Account.CreateNewAccount();
    }
}
