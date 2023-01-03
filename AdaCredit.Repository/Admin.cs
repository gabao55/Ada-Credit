using static BCrypt.Net.BCrypt;

namespace AdaCreditRepository;

public sealed class Admin
{
    private static string DataFilePath;
    private static int salt;
    private static Dictionary<string, int> DataIndexCorrespondence;

    static Admin()
    {
        string directoryPath = Environment.CurrentDirectory;
        string adminDataFileName = "AdminData.txt";

        string dataFilePath = Path.Combine(directoryPath, adminDataFileName);

        DataFilePath = dataFilePath;
        salt = 12;
        DataIndexCorrespondence = new Dictionary<string, int>();
        DataIndexCorrespondence.Add("username", 0);
        DataIndexCorrespondence.Add("password", 1);
    }

    public static bool CheckFirstAccess() {
        if (!File.Exists(DataFilePath))
        {
            return true;
        }
        
        string username = GetUserData("username");
        string password = GetUserData("password");

        if (username == "*user*" || password == "*pass*")
        {
            return true;
        }

        return false;
    }

    public static void CreateData() {
        if (File.Exists(DataFilePath))
            return;
            
        string hashedPassword = HashPassword("*pass*", salt);
        string initialData = $"username: *user*\npassword: {hashedPassword}";
        File.WriteAllText(DataFilePath, initialData);
    }

    public static void ChangeAdminData(string username, string password)
    {
        string hashedPassword = HashPassword(password, salt);
        string newData = $"username: {username}\npassword: {hashedPassword}";

        File.WriteAllText(DataFilePath, newData);
    }

    public static bool ValidateAdminData(string username, string password)
    {
        string dbUsername = GetUserData("username");
        string dbPassword = GetUserData("password");

        if (username != dbUsername || !Verify(password, dbPassword))
        {
            return false;
        }

        return true;
    }

    private static string GetUserData(string data) {
        int index = DataIndexCorrespondence[data];

        string[] userData = File.ReadAllLines(DataFilePath);
        string correspondingData = userData[index].Substring(10);

        return correspondingData;
    }
}
