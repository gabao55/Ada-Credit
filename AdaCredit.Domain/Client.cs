using System;

namespace AdaCreditDomain;

public sealed class Client
{
    public static void CreateClient()
    {
        Console.Clear();

        List<AdaCreditRepository.ClientData> clientsList = AdaCreditRepository.Client.GetAllClients();

        int id = clientsList.Last().Id + 1;
        
        Console.Write("Insert client's name: ");        
        string? name = Console.ReadLine();
        while (name == null)
        {
            Console.Write("Invalid name, please insert again: ");
            name = Console.ReadLine();
        }

        Console.Write("Insert client's document (e.g.: ID): ");        
        string? document = Console.ReadLine();
        while (document == null)
        {
            Console.Write("Invalid document, please insert again: ");
            document = Console.ReadLine();
        }
        
        while (document == null || clientsList.Any(client => client.Document == document))
        {
            Console.Write("Document already registered, please insert another document: ");
            document = Console.ReadLine();
        }

        AdaCreditRepository.ClientData newClient = new AdaCreditRepository.ClientData {
            Id = id,
            Name = name,
            Document = document,
        };

        AdaCreditRepository.Client.CreateNewClient(newClient);

        AdaCreditDomain.Account.CreateNewAccount(newClient.Id);
    }

    public static void GetClientData()
    {
        Console.Clear();

        Console.Write("Please insert client's document (e.g.: ID): ");
        string? document = Console.ReadLine();
        while (document == null)
        {
            Console.Write("Invalid document, please insert again: ");
            document = Console.ReadLine();
        }

        List<AdaCreditRepository.ClientData> clientsList = AdaCreditRepository.Client.GetAllClients();

        AdaCreditRepository.ClientData? client = clientsList.FirstOrDefault(client => client.Document == document);

        if (client == null) {
            Console.WriteLine($"No client registered with document {document}");
            Console.WriteLine("\nPress enter to return to client's menu");
            Console.ReadLine();
            return;
        }

        PrintClientData(client);
        AdaCreditDomain.Account.PrintAccountData(client.Id);
        Console.WriteLine("\nPress enter to return to client's menu");
        Console.ReadLine();
    }

    public static void PrintClientData(AdaCreditRepository.ClientData data)
    {
        Console.WriteLine("Client's data:\n");
        Console.WriteLine($"Name: {data.Name}");
        Console.WriteLine($"Document: {data.Document}");
    }

    public static void DeleteClientData()
    {
        Console.Clear();

        Console.Write("Please insert client's document (e.g.: ID): ");
        string? document = Console.ReadLine();
        while (document == null)
        {
            Console.Write("Invalid document, please insert again: ");
            document = Console.ReadLine();
        }

        List<AdaCreditRepository.ClientData> clientsList = AdaCreditRepository.Client.GetAllClients();

        AdaCreditRepository.ClientData? client = clientsList.FirstOrDefault(client => client.Document == document);

        if (client == null) {
            Console.WriteLine($"No client registered with document {document}");
            Console.WriteLine("\nPress enter to return to client's menu");
            Console.ReadLine();
            return;
        }

        AdaCreditRepository.Client.DeleteClientData(client.Id);
        AdaCreditRepository.Account.DeleteAccountData(client.Id);
        
        Console.WriteLine("Client data successfuly deleted.");
        Console.WriteLine("\nPress enter to return to client's menu");
        Console.ReadLine();
    }
}
