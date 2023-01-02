using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Globalization;

namespace AdaCreditRepository;

public sealed class Account
{
    private static string DataFilePath;

    static Account()
    {
        string directoryPath = Environment.CurrentDirectory;
        string dataFileName = "AccountsData.txt";

        string dataFilePath = Path.Combine(directoryPath, dataFileName);

        DataFilePath = dataFilePath;
    }

    public static List<AccountData> GetAllAccounts()
    {
        using (var reader = new StreamReader(DataFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<AccountData>().ToList();

            return records;
        }
    }

    public static void CreateNewAccount(AccountData data) {
        List<AccountData> records = GetAllAccounts();
        records.Add(data);
        
        SaveData(records);
    }

    public static void UpdateBalance(int accountId, double newBalance)
    {
        List<AccountData> records = GetAllAccounts();
        AccountData account = records.First(a => a.Id == accountId);
        account.Balance = Math.Round(newBalance, 2);
        

        SaveData(records);
    }

    public static AccountData? GetAccountData(string accountNumber)
    {
        List<AccountData> records = GetAllAccounts();
        AccountData? account = records.FirstOrDefault(a => a.Number == accountNumber);

        return account;
    }

    public static void SaveData(List<AccountData> records) {
        using (var writer = new StreamWriter(DataFilePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
    }
}

public sealed class AccountData
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string Branch { get; set; }
    public double Balance { get; set; }
    public int ClientId { get; set; }
}