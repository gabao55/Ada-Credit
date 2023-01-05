using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Bogus;

namespace AdaCreditRepository;

public sealed class Transaction
{
    private static string DataFilesPath;
    private static string SuccessfulFilesPath;
    private static string FailedFilesPath;

    static Transaction()
    {
        string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string dataFilesDirectory = "Transactions";
        string successfulFilesDirectory = "Transactions/Completed";
        string failedFilesDirectory = "Transactions/Failed";

        string dataFilesPath = Path.Combine(directoryPath, dataFilesDirectory);
        string successfulFilesPath = Path.Combine(directoryPath, successfulFilesDirectory);
        string failedFilesPath = Path.Combine(directoryPath, failedFilesDirectory);

        DataFilesPath = dataFilesPath;
        SuccessfulFilesPath = successfulFilesPath;
        FailedFilesPath = failedFilesPath;
    }

    public static void CreateSuccessfullTransaction()
    {
        var faker = new Faker();
        string fakeDate = String.Join("", faker.Date.PastDateOnly().ToString().Split("/").Reverse().ToArray());
        string initialData = "771,0001,483667,771,1,872870,TEF,1\n771,0001,483667,771,1,872870,TEF,2";
        string fileName = Path.Combine(DataFilesPath, $"Santander-{fakeDate}.csv");
        File.WriteAllText(fileName, initialData);
    }

    public static void CreateFailedBalanceTransaction()
    {
        var faker = new Faker();
        string fakeDate = String.Join("", faker.Date.PastDateOnly().ToString().Split("/").Reverse().ToArray());
        List<ClientData> clients = Client.GetAllClients();
        string firstClientAccountNumber = Account.GetAccountDataByClientId(clients.First().Id).Number;
        string lastClientAccountNumber = Account.GetAccountDataByClientId(clients.Last().Id).Number;
        string initialData = $"777,0001,{firstClientAccountNumber},777,1,{lastClientAccountNumber},TEF,30.55\n777,00001,{firstClientAccountNumber},777,1,{lastClientAccountNumber},TEF,20";
        string fileName = Path.Combine(DataFilesPath, $"Safra-{fakeDate}.csv");
        File.WriteAllText(fileName, initialData);
    }

    public static void CreateFailedTEFTransaction()
    {
        var faker = new Faker();
        string fakeDate = String.Join("", faker.Date.PastDateOnly().ToString().Split("/").Reverse().ToArray());
        List<ClientData> clients = Client.GetAllClients();
        AccountData firstClientAccountNumber = Account.GetAccountDataByClientId(clients.First().Id);
        AccountData lastClientAccountNumber = Account.GetAccountDataByClientId(clients.Last().Id);
        string initialData = $"777,0001,{firstClientAccountNumber},775,1,{lastClientAccountNumber},TEF,30.55";
        string fileName = Path.Combine(DataFilesPath, $"Itau-{fakeDate}.csv");
        File.WriteAllText(fileName, initialData);
    }

    public static void CreateFailedAccountNotFoundTransaction()
    {
        var faker = new Faker();
        string fakeDate = String.Join("", faker.Date.PastDateOnly().ToString().Split("/").Reverse().ToArray());
        string initialData = "777,0001,1,777,1,2,TEF,30.55";
        string fileName = Path.Combine(DataFilesPath, $"Nubank-{fakeDate}.csv");
        File.WriteAllText(fileName, initialData);
    }

    public static void CreateData() {
        if (!Directory.Exists(DataFilesPath))
        {
            Directory.CreateDirectory(DataFilesPath);
        }

        if (!Directory.Exists(SuccessfulFilesPath))
        {
            Directory.CreateDirectory(SuccessfulFilesPath);
        }

        if (!Directory.Exists(FailedFilesPath))
        {
            Directory.CreateDirectory(FailedFilesPath);
        }
    }

    public static List<string> GetTransactionFiles() {
        return Directory.GetFiles(DataFilesPath, "*.csv").ToList();
    }

    public static List<TransactionData> GetAllTransactions(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<TransactionData>().ToList();

                return records;
            }
        }
    }

    public static void MoveTransactionFile(string filePath, bool completed)
    {
        string fileName = filePath.Split("/").Last();
        string newFilePath;
        if (completed) {
            newFilePath = Path.Combine(SuccessfulFilesPath, fileName);
        }
        else
        {
            newFilePath = Path.Combine(FailedFilesPath, fileName);
        }
        
        File.Move(filePath, newFilePath);
    }
}

public sealed class TransactionData
{
    [Index(0)]
    public string OriginBankCode { get; set; }
    [Index(1)]
    public string OriginBankBranch { get; set; }
    [Index(2)]
    public string OriginBankAccountNumber { get; set; }
    [Index(3)]
    public string DestinationBankCode { get; set; }
    [Index(4)]
    public string DestinationBankBranch { get; set; }
    [Index(5)]
    public string DestinationBankAccountNumber { get; set; }
    [Index(6)]
    public string TransactionType { get; set; }
    [Index(7)]
    public double TransactionValue { get; set; }
}
