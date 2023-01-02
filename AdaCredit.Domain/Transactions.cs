using System;

namespace AdaCreditDomain;

public sealed class Transactions
{
    public static void ProcessAllTransactions()
    {
        Console.Clear();

        List<string> transactionsFilesList = AdaCreditRepository.Transaction.GetTransactionFiles();
        List<string> failedTransactionFiles = new List<string>();
        foreach (string file in transactionsFilesList)
        {
            bool fileCheck = ProcessFileTransactions(file);
            AdaCreditRepository.Transaction.MoveTransactionFile(file, fileCheck, failedTransactionFiles);
        }

        if (failedTransactionFiles.Count() > 0)
        {
            Console.WriteLine("An error has occurred processing the transaction files below:");
            foreach (string fileName in failedTransactionFiles)
            {
                Console.WriteLine(fileName);
            }
        }
        else
        {
            Console.WriteLine("All files have been processed successfully");
        }
        
        Console.WriteLine("\nPress enter to return to transactions' menu");
        Console.ReadLine();
    }

    public static bool ProcessFileTransactions(string filePath)
    {
        List<AdaCreditRepository.TransactionData> transactions = AdaCreditRepository.Transaction.GetAllTransactions(filePath);
        foreach (AdaCreditRepository.TransactionData transacation in transactions)
        {
            if (transacation.TransactionType == "TEF" && (transacation.DestinationBankCode != transacation.OriginBankCode))
                return false;
            AdaCreditRepository.AccountData? originAccount = null;
            AdaCreditRepository.AccountData? destinationAccount = null;
            
            if (transacation.OriginBankCode == "777")
            {
                originAccount = AdaCreditRepository.Account.GetAccountData(transacation.OriginBankAccountNumber);
                if (originAccount == null)
                    return false;
            }
            
            if (transacation.DestinationBankCode == "777")
            {
                destinationAccount = AdaCreditRepository.Account.GetAccountData(transacation.DestinationBankAccountNumber);
                if (destinationAccount == null)
                    return false;
            }

            double tax = GetTransactionTax(transacation.TransactionType, transacation.TransactionValue);
            if (transacation.TransactionDirection == 0 && originAccount != null)
            {
                if (transacation.TransactionValue + tax > originAccount.Balance)
                    return false;
                AdaCreditRepository.Account.UpdateBalance(originAccount.Id, originAccount.Balance - (transacation.TransactionValue + tax));
                if (destinationAccount != null)
                    AdaCreditRepository.Account.UpdateBalance(destinationAccount.Id, destinationAccount.Balance + transacation.TransactionValue);
            }
            if (transacation.TransactionDirection == 1 && destinationAccount != null)
            {
                if (transacation.TransactionValue + tax > destinationAccount.Balance)
                    return false;
                AdaCreditRepository.Account.UpdateBalance(destinationAccount.Id, destinationAccount.Balance - (transacation.TransactionValue + tax));
                if (originAccount != null)
                    AdaCreditRepository.Account.UpdateBalance(originAccount.Id, originAccount.Balance + transacation.TransactionValue);
            }
        }

        return true;
    }

    private static double GetTransactionTax(string transacationType, double transacationValue)
    {
        switch (transacationType)
        {
            case "TEF":
                return 0;
            case "DOC":
                return transacationValue*0.01 > 5.0 ? 6.0 : 1.0 + transacationValue*0.01;
            default:
                return 5.0;
        }
    }
}