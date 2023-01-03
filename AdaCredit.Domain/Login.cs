using AdaCreditRepository;

namespace AdaCreditDomain;

public sealed class Login
{
    public static void Start() {
        CheckAppResources();
        bool isFirstAcces = Admin.CheckFirstAccess();
        
        if (isFirstAcces)
        {
            FirstAccess();
        }
        else
        {
            Console.Clear();
            bool isUserValid = FurtherAcces();

            while (!isUserValid)
            {
                Console.WriteLine("Username or password invalid. Try again:");
                isUserValid = FurtherAcces();
            }
        }
    }

    private static void CheckAppResources()
    {
        Admin.CreateData();
        AdaCreditRepository.Client.CreateData();
        AdaCreditRepository.Account.CreateData();
        AdaCreditRepository.Employee.CreateData();
        AdaCreditRepository.Transaction.CreateData();
        AdaCreditRepository.TransactionErrors.CreateData();
    }

    private static void FirstAccess()
    {
        Console.Clear();

        bool isUserValid = FurtherAcces();
        while (!isUserValid)
        {
            Console.WriteLine("Username or password invalid. Try again:");
            isUserValid = FurtherAcces();
        }

        Console.WriteLine("Welcome to Ada Credit. Since this is your first access, you must define your username and password below:");

        Console.Write("username: ");        
        string? username = Console.ReadLine();
        while (username == null)
        {
            Console.Write("Invalid username, please insert a new value: ");
            username = Console.ReadLine();
        }

        Console.Write("password: ");        
        string? password = Console.ReadLine();
        while (password == null)
        {
            Console.Write("Invalid password, please insert a new value: ");
            password = Console.ReadLine();
        }

        Admin.ChangeAdminData(username, password);
    }

    private static bool FurtherAcces()
    {
        Console.WriteLine("Welcome to Ada Credit. In order to login the application, please insert your username and password below:");

        Console.Write("username: ");        
        string? username = Console.ReadLine();
        while (username == null)
        {
            Console.Write("Invalid username, please insert it again: ");
            username = Console.ReadLine();
        }

        Console.Write("password: ");        
        string? password = Console.ReadLine();
        while (password == null)
        {
            Console.Write("Invalid password, please insert it again: ");
            password = Console.ReadLine();
        }

        return Admin.ValidateAdminData(username, password);
    }
}
