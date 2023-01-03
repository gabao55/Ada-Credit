using CsvHelper;
using System.Globalization;

namespace AdaCreditRepository;

public sealed class Client
{
    private static string DataFilePath;

    static Client()
    {
        string directoryPath = Environment.CurrentDirectory;
        string dataFileName = "ClientsData.txt";

        string dataFilePath = Path.Combine(directoryPath, dataFileName);

        DataFilePath = dataFilePath;
    }

    public static void CreateData() {
        if (File.Exists(DataFilePath))
            return;
            
        string initialData = "Id,Name,Document,LastLogin,IsActive";
        File.WriteAllText(DataFilePath, initialData);
    }

    public static List<ClientData> GetAllClients()
    {
        using (var reader = new StreamReader(DataFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<ClientData>().ToList();

            return records;
        }
    }

    public static void CreateNewClient(ClientData data) {
        List<ClientData> records = GetAllClients();
        records.Add(data);
        
        SaveData(records);
    }

    public static void DeactivateClientData(int id) {
        List<ClientData> records = GetAllClients();
        
        ClientData client = records.First(c => c.Id == id);
        client.IsActive = false;
        
        SaveData(records);
    }

    public static void ChangeClientData(int id, ClientData data) {
        List<ClientData> records = GetAllClients();

        ClientData client = records.First(client => client.Id == id);

        client.Name = data.Name;
        client.Document = data.Document;

        SaveData(records);
    }

    public static void SaveData(List<ClientData> records) {
        using (var writer = new StreamWriter(DataFilePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
    }
}

public sealed class ClientData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public DateTime LastLogin { get; set; }
    public bool IsActive { get; set; }
}