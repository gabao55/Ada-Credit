using ConsoleTables;

namespace AdaCreditDomain;

public sealed class Report
{
    public static void ActiveClients()
    {
        Console.Clear();

        List<AdaCreditRepository.ClientData> allClients = AdaCreditRepository.Client.GetAllClients();
        List<AdaCreditRepository.ClientData> activeClients = allClients.FindAll(c => c.IsActive == true);

        if (activeClients.Count() == 0)
        {
            Console.WriteLine("No active clients registered");
            Console.WriteLine("\nPress enter to return to reports' menu");
            Console.ReadLine();
            return;
        }

        var table = new ConsoleTable("Name", "Document", "Last login", "Account number", "Branch", "Balance");

        foreach (AdaCreditRepository.ClientData client in activeClients)
        {
            AdaCreditRepository.AccountData account = AdaCreditRepository.Account.GetAccountDataByClientId(client.Id);

            table.AddRow(
                client.Name,
                client.Document,
                client.LastLogin,
                account.Number,
                account.Branch,
                $"R$ {account.Balance}"
            );
        }

        table.Write();

        Console.WriteLine("\nPress enter to return to reports' menu");
        Console.ReadLine();
    }
    
    public static void InactiveClients()
    {
        Console.Clear();

        List<AdaCreditRepository.ClientData> allClients = AdaCreditRepository.Client.GetAllClients();
        List<AdaCreditRepository.ClientData> activeClients = allClients.FindAll(c => c.IsActive == false);

        if (activeClients.Count() == 0)
        {
            Console.WriteLine("No inactive clients registered");
            Console.WriteLine("\nPress enter to return to reports' menu");
            Console.ReadLine();
            return;
        }

        var table = new ConsoleTable("Name", "Document", "Last login");

        foreach (AdaCreditRepository.ClientData client in activeClients)
        {
            table.AddRow(
                client.Name,
                client.Document,
                client.LastLogin
            );
        }

        table.Write();

        Console.WriteLine("\nPress enter to return to reports' menu");
        Console.ReadLine();
    }
    
    public static void ActiveEmployees()
    {
        Console.Clear();

        List<AdaCreditRepository.EmployeeData> allEmployees = AdaCreditRepository.Employee.GetAllEmployees();
        List<AdaCreditRepository.EmployeeData> activeEmployees = allEmployees.FindAll(e => e.IsActive == true);

        if (activeEmployees.Count() == 0)
        {
            Console.WriteLine("No active employees registered");
            Console.WriteLine("\nPress enter to return to reports' menu");
            Console.ReadLine();
            return;
        }

        var table = new ConsoleTable("Name", "Document", "Last login");

        foreach (AdaCreditRepository.EmployeeData employee in activeEmployees)
        {
            table.AddRow(
                employee.Name,
                employee.Document,
                employee.LastLogin
            );
        }

        table.Write();

        Console.WriteLine("\nPress enter to return to reports' menu");
        Console.ReadLine();
    }
    
    public static void TransactionErrors()
    {
        Console.Clear();

        List<AdaCreditRepository.TransactionErrorData> allErrors = AdaCreditRepository.TransactionErrors.GetAllErrors();

        if (allErrors.Count() == 0)
        {
            Console.WriteLine("No active clients registered");
            Console.WriteLine("\nPress enter to return to reports' menu");
            Console.ReadLine();
            return;
        }

        var table = new ConsoleTable(
            "File name",
            "Error date",
            "Origin bank code",
            "Origin bank branch",
            "Origin account number",
            "Destination bank code",
            "Destination bank branch",
            "Destination account number",
            "Transaction type",
            "Transaction direction",
            "Transaction value",
            "Error details"
        );

        foreach (AdaCreditRepository.TransactionErrorData error in allErrors)
        {
            table.AddRow(
                error.FileName,
                error.Date,
                error.OriginBankCode,
                error.OriginBankBranch,
                error.OriginBankAccountNumber,
                error.DestinationBankCode,
                error.DestinationBankBranch,
                error.DestinationBankAccountNumber,
                error.TransactionType,
                error.TransactionDirection == 0 ? "Debit" : "Credit",
                $"R$ {error.TransactionValue}",
                error.Error
            );
        }

        table.Write();

        Console.WriteLine("\nPress enter to return to reports' menu");
        Console.ReadLine();
    }
}