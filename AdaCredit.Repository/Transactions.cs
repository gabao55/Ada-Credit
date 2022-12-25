using CsvHelper;
using System.Globalization;

namespace AdaCreditRepository;

public sealed class TransactionData
{
    public string OriginBankCode { get; set; }
    public string OriginBankBranch { get; set; }
    public string OriginBankAccountNumber { get; set; }
    public string DestinationBankCode { get; set; }
    public string DestinationBankBranch { get; set; }
    public string DestinationBankAccountNumber { get; set; }
    public string TransactionType { get; set; }
    public int TransactionDirection { get; set; }
    public double TransactionValue { get; set; }
}
