using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

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

    public static void MoveTransactionFile(string filePath, bool completed, List<string> failedTransactionFiles)
    {
        string fileName = filePath.Split("/").Last();
        string newFilePath;
        if (completed) {
            newFilePath = Path.Combine(SuccessfulFilesPath, fileName);
        }
        else
        {
            newFilePath = Path.Combine(FailedFilesPath, fileName);
            failedTransactionFiles.Add(newFilePath);
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
    public int TransactionDirection { get; set; }
    [Index(8)]
    public double TransactionValue { get; set; }
}
