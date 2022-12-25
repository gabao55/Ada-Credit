using CsvHelper;
using System.Globalization;

namespace AdaCreditRepository;

public sealed class Employee
{
    private static string DataFilePath;

    static Employee()
    {
        string directoryPath = Environment.CurrentDirectory;
        string dataFileName = "EmployeesData.txt";

        string dataFilePath = Path.Combine(directoryPath, dataFileName);

        DataFilePath = dataFilePath;
    }

    public static List<EmployeeData> GetAllEmployees()
    {
        using (var reader = new StreamReader(DataFilePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<EmployeeData>().ToList();

            return records;
        }
    }

    public static void CreateNewEmployee(EmployeeData data) {
        List<EmployeeData> records = GetAllEmployees();
        records.Add(data);
        
        SaveData(records);
    }

    public static void DeleteEmployeeData(int id) {
        List<EmployeeData> records = GetAllEmployees();

        records.RemoveAll(Employee => Employee.Id == id);
        
        SaveData(records);
    }

    public static void ChangeEmployeeData(int id, EmployeeData data) {
        List<EmployeeData> records = GetAllEmployees();

        EmployeeData Employee = records.First(Employee => Employee.Id == id);

        Employee.Name = data.Name;
        Employee.Document = data.Document;

        SaveData(records);
    }

    public static void SaveData(List<EmployeeData> records) {
        using (var writer = new StreamWriter(DataFilePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
    }
}

public sealed class EmployeeData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
}