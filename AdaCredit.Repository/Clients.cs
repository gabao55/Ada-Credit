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
}