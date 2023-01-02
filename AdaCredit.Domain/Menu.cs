using ConsoleTools;

namespace AdaCreditDomain;

public sealed class Menu
{
    public static void Show(string[] args) {        
        var menu = new ConsoleMenu(args, level: 0)
            .Add("Clients", () => ShowClientsMenu(args))
            .Add("Employees", () => ShowEmployeesMenu(args))
            .Add("Transactions", () => ShowTransactionsMenu(args))
            .Add("Reports", () => ShowReportsMenu(args))
            .Add("Exit", () => Environment.Exit(0))
            .Configure(config =>
            {
            config.Selector = "--> ";
            config.EnableFilter = true;
            config.Title = "Main menu. Select which service you want to use";
            config.EnableWriteTitle = false;
            config.EnableBreadcrumb = true;
            });

        menu.Show();
    }

    private static void ShowClientsMenu(string[] args)
    {

        var clientsMenu = new ConsoleMenu(args, level: 1)
            .Add("Register new client", () => AdaCreditDomain.Client.CreateClient())
            .Add("Query existing client data", () => AdaCreditDomain.Client.GetClientData())
            .Add("Change existing client data", () => AdaCreditDomain.Client.ChangeClientData())
            .Add("Deactivate existing client", () => AdaCreditDomain.Client.DeactivateClientData())
            .Add("Get back to main menu", ConsoleMenu.Close)
            .Configure(config =>
            {
            config.Selector = "--> ";
            config.EnableFilter = true;
            config.Title = "Clients menu";
            config.EnableBreadcrumb = true;
            config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
            });

        clientsMenu.Show();
    }

    private static void ShowEmployeesMenu(string[] args)
    {
        var clientsMenu = new ConsoleMenu(args, level: 1)
            .Add("Register new employee", () => AdaCreditDomain.Employee.CreateEmployee())
            .Add("Change existing employee password", () => AdaCreditDomain.Employee.ChangeEmployeePassword())
            .Add("Deactivate employee registration", () => AdaCreditDomain.Employee.DeactivateEmployeeRegistration())
            .Add("Get back to main menu", ConsoleMenu.Close)
            .Configure(config =>
            {
            config.Selector = "--> ";
            config.EnableFilter = true;
            config.Title = "Employees menu";
            config.EnableBreadcrumb = true;
            config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
            });

        clientsMenu.Show();
    }

    private static void ShowTransactionsMenu(string[] args)
    {
        var clientsMenu = new ConsoleMenu(args, level: 1)
            .Add("Process Transactions (Bank Reconciliation)", () => AdaCreditDomain.Transactions.ProcessAllTransactions())
            .Add("Get back to main menu", ConsoleMenu.Close)
            .Configure(config =>
            {
            config.Selector = "--> ";
            config.EnableFilter = true;
            config.Title = "Transactions menu";
            config.EnableBreadcrumb = true;
            config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
            });

        clientsMenu.Show();
    }

    private static void ShowReportsMenu(string[] args)
    {
        var clientsMenu = new ConsoleMenu(args, level: 1)
            .Add("View all active clients with their respective balances", () => SomeAction("Sub_One"))
            .Add("View all existing clients", () => SomeAction("Sub_Two"))
            .Add("View all active employees and their last login date and time", () => SomeAction("Sub_Three"))
            .Add("View transactions with error (transaction and error details)", () => SomeAction("Sub_Four"))
            .Add("Get back to main menu", ConsoleMenu.Close)
            .Configure(config =>
            {
            config.Selector = "--> ";
            config.EnableFilter = true;
            config.Title = "Reports menu";
            config.EnableBreadcrumb = true;
            config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
            });

        clientsMenu.Show();
    }

    private static void AddNewClient()
    {
        
    }

    private static void SomeAction(string str) {

    }
}

