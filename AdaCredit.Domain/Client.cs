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
}
