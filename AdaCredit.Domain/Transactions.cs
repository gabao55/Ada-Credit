using System;

namespace AdaCreditDomain;

public sealed class Transactions
{
    public static void Seed()
    {
        AdaCreditRepository.Transaction.CreateSuccessfullTransaction();
        AdaCreditRepository.Transaction.CreateFailedAccountNotFoundTransaction();
        AdaCreditRepository.Transaction.CreateFailedBalanceTransaction();
        AdaCreditRepository.Transaction.CreateFailedTEFTransaction();
    }
    
    public static void ProcessAllTransactions()
    {
        Console.Clear();

        List<string> transactionsFilesList = AdaCreditRepository.Transaction.GetTransactionFiles();
        List<string> failedTransactionFiles = new List<string>();
        if(transactionsFilesList.Count() == 0) {
            Console.WriteLine("No new transactions to be processed");
        
            Console.WriteLine("\nPress enter to return to transactions' menu");
            Console.ReadLine();

            return;
        }
        
        foreach (string file in transactionsFilesList)
        {
            bool fileCheck = ProcessFileTransactions(file);
            AdaCreditRepository.Transaction.MoveTransactionFile(file, fileCheck);
            if (!fileCheck)
                failedTransactionFiles.Add(file);
        }

        if (failedTransactionFiles.Count() > 0)
        {
            Console.WriteLine("An error has occurred processing the transaction files below:");
            foreach (string failedFilePath in failedTransactionFiles)
            {
                Console.WriteLine(failedFilePath);
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
            {
                AdaCreditRepository.TransactionErrors.CreateNewError(transacation, filePath, "TEF");
                return false;
            }

            AdaCreditRepository.AccountData? originAccount = null;
            AdaCreditRepository.AccountData? destinationAccount = null;
            
            if (transacation.OriginBankCode == "777")
            {
                originAccount = AdaCreditRepository.Account.GetAccountData(transacation.OriginBankAccountNumber);
                if (originAccount == null)
                {
                    AdaCreditRepository.TransactionErrors.CreateNewError(transacation, filePath, "Origin account not found");
                    return false;
                }
            }
            
            if (transacation.DestinationBankCode == "777")
            {
                destinationAccount = AdaCreditRepository.Account.GetAccountData(transacation.DestinationBankAccountNumber);
                if (destinationAccount == null)
                {
                    AdaCreditRepository.TransactionErrors.CreateNewError(transacation, filePath, "Destination account not found");
                    return false;
                }
            }

            double tax = GetTransactionTax(transacation.TransactionType, transacation.TransactionValue);
            if (originAccount != null)
            {
                if (transacation.TransactionValue + tax > originAccount.Balance)
                {
                    AdaCreditRepository.TransactionErrors.CreateNewError(transacation, filePath, "Origin account has not enough money");
                    return false;
                }
                AdaCreditRepository.Account.UpdateBalance(originAccount.Id, originAccount.Balance - (transacation.TransactionValue + tax));
                if (destinationAccount != null)
                    AdaCreditRepository.Account.UpdateBalance(destinationAccount.Id, destinationAccount.Balance + transacation.TransactionValue);
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