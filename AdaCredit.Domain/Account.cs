namespace AdaCreditDomain;

public sealed class Account
{
    private static string Branch = "0001";
    private static Random random = new Random();

    public static void CreateNewAccount(int clientId)
    {
        List<AdaCreditRepository.AccountData> accountsList = AdaCreditRepository.Account.GetAllAccounts();
        
        string newNumber;

        do
        {
            newNumber = GenerateRandomAccountNumber();
        } while (accountsList.Any(account => account.Number == newNumber));

        int id = accountsList.Last().Id + 1;
        double balance = 0;

        AdaCreditRepository.AccountData newAccount = new AdaCreditRepository.AccountData {
            Id = id,
            Number = newNumber,
            Branch = Branch,
            Balance = balance,
            ClientId = clientId,
        };

        AdaCreditRepository.Account.CreateNewAccount(newAccount);
    }

    public static void PrintClientData()
    {

    }

    private static string GenerateRandomAccountNumber()
    {
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
