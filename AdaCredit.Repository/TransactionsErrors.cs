using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace AdaCreditRepository;

public sealed class TransactionErrors
{
    private static string DataFilePath;

    static TransactionErrors()
    {
        string directoryPath = Environment.CurrentDirectory;
        string dataFileName = "TransactionsErrors.txt";

        string dataFilePath = Path.Combine(directoryPath, dataFileName);

        DataFilePath = dataFilePath;
    }
    
    public static List<TransactionErrorData> GetAllErrors()
    {
        using (var reader = new StreamReader(DataFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<TransactionErrorData>().ToList();

            return records;
        }
    }

    public static void CreateNewError(TransactionData data, string filePath, string errorType) {
        List<TransactionErrorData> records = GetAllErrors();
        string error = DefineErrorMessage(data, errorType);

        records.Add(
            new TransactionErrorData {
                Id = records.Count() == 0 ? 1 : records.Last().Id + 1,
                FileName = filePath.Split("/").Last(),
                Date = DateTime.Now,
                OriginBankCode = data.OriginBankCode,
                OriginBankBranch = data.OriginBankBranch,
                OriginBankAccountNumber = data.OriginBankAccountNumber,
                DestinationBankCode = data.DestinationBankCode,
                DestinationBankBranch = data.DestinationBankBranch,
                DestinationBankAccountNumber = data.DestinationBankAccountNumber,
                TransactionType = data.TransactionType,
                TransactionDirection = data.TransactionDirection,
                TransactionValue = data.TransactionValue,
                Error = error,
            }
        );
        
        SaveData(records);
    }

    private static string DefineErrorMessage(TransactionData data, string errorType)
    {
        switch (errorType)
        {
            case "TEF":
                return $"Invalid TEF transaction between different banks (bank {data.OriginBankCode} and bank {data.DestinationBankCode})";
            case "Origin account not found":
                return $"Account with number {data.OriginBankAccountNumber} not found";
            case "Destination account not found":
                return $"Account with number {data.DestinationBankAccountNumber} not found";
            case "Origin account has not enough money":
                return $"Account with number {data.OriginBankAccountNumber} has not enough money to complete transaction with value R$ {data.TransactionValue}";
            case "Destination account has not enough money":
                return $"Account with number {data.DestinationBankAccountNumber} has not enough money to complete transaction with value ${data.TransactionValue}";
            default:
                return "Unidentified error";
        }
    }

    public static void SaveData(List<TransactionErrorData> records) {
        using (var writer = new StreamWriter(DataFilePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
    }
}

public sealed class TransactionErrorData
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public DateTime Date { get; set; }
    public string OriginBankCode { get; set; }
    public string OriginBankBranch { get; set; }
    public string OriginBankAccountNumber { get; set; }
    public string DestinationBankCode { get; set; }
    public string DestinationBankBranch { get; set; }
    public string DestinationBankAccountNumber { get; set; }
    public string TransactionType { get; set; }
    public int TransactionDirection { get; set; }
    public double TransactionValue { get; set; }
    public string Error { get; set; }
}
