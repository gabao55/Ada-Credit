using System;
using static BCrypt.Net.BCrypt;

namespace AdaCreditDomain;

public sealed class Employee
{
    public static void CreateEmployee()
    {
        Console.Clear();

        List<AdaCreditRepository.EmployeeData> EmployeesList = AdaCreditRepository.Employee.GetAllEmployees();

        int id = EmployeesList.Last().Id + 1;
        
        Console.Write("Insert Employee's name: ");        
        string? name = Console.ReadLine();
        while (name == null)
        {
            Console.Write("Invalid name, please insert again: ");
            name = Console.ReadLine();
        }

        Console.Write("Insert Employee's document (e.g.: ID): ");        
        string? document = Console.ReadLine();
        while (document == null)
        {
            Console.Write("Invalid document, please insert again: ");
            document = Console.ReadLine();
        }
        
        while (document == null || EmployeesList.Any(Employee => Employee.Document == document))
        {
            Console.Write("Document already registered, please insert another document: ");
            document = Console.ReadLine();
        }
        
        Console.Write("Insert Employee's username: ");        
        string? username = Console.ReadLine();
        while (username == null)
        {
            Console.Write("Invalid username, please insert again: ");
            username = Console.ReadLine();
        }
        
        while (username == null || EmployeesList.Any(Employee => Employee.Username == username))
        {
            Console.Write("Username already registered, please insert another username: ");
            username = Console.ReadLine();
        }
        
        Console.Write("Insert Employee's password: ");        
        string? password = Console.ReadLine();
        while (password == null)
        {
            Console.Write("Invalid password, please insert again: ");
            password = Console.ReadLine();
        }

        int salt = 12;
        string hashedPassword = HashPassword(password, salt);

        AdaCreditRepository.EmployeeData newEmployee = new AdaCreditRepository.EmployeeData {
            Id = id,
            Name = name,
            Document = document,
            Username = username,
            Password = hashedPassword,
            IsActive = true,
        };

        AdaCreditRepository.Employee.CreateNewEmployee(newEmployee);

        Console.WriteLine("\nEmployee successfuly registered.");
        Console.WriteLine("\nPress enter to return to client's menu");
        Console.ReadLine();
    }

    public static void GetEmployeeData()
    {
        Console.Clear();

        Console.Write("Please insert Employee's document (e.g.: ID): ");
        string? document = Console.ReadLine();
        while (document == null)
        {
            Console.Write("Invalid document, please insert again: ");
            document = Console.ReadLine();
        }

        List<AdaCreditRepository.EmployeeData> EmployeesList = AdaCreditRepository.Employee.GetAllEmployees();

        AdaCreditRepository.EmployeeData? Employee = EmployeesList.FirstOrDefault(Employee => Employee.Document == document);

        if (Employee == null) {
            Console.WriteLine($"No Employee registered with document {document}");
            Console.WriteLine("\nPress enter to return to Employee's menu");
            Console.ReadLine();
            return;
        }

        PrintEmployeeData(Employee);
        AdaCreditDomain.Account.PrintAccountData(Employee.Id);
        Console.WriteLine("\nPress enter to return to Employee's menu");
        Console.ReadLine();
    }

    public static void PrintEmployeeData(AdaCreditRepository.EmployeeData data)
    {
        Console.WriteLine("Employee's data:\n");
        Console.WriteLine($"Name: {data.Name}");
        Console.WriteLine($"Document: {data.Document}");
    }

    public static void DeleteEmployeeData()
    {
        Console.Clear();

        Console.Write("Please insert Employee's document (e.g.: ID): ");
        string? document = Console.ReadLine();
        while (document == null)
        {
            Console.Write("Invalid document, please insert again: ");
            document = Console.ReadLine();
        }

        List<AdaCreditRepository.EmployeeData> EmployeesList = AdaCreditRepository.Employee.GetAllEmployees();

        AdaCreditRepository.EmployeeData? Employee = EmployeesList.FirstOrDefault(Employee => Employee.Document == document);

        if (Employee == null) {
            Console.WriteLine($"No Employee registered with document {document}");
            Console.WriteLine("\nPress enter to return to Employee's menu");
            Console.ReadLine();
            return;
        }

        AdaCreditRepository.Employee.DeleteEmployeeData(Employee.Id);
        AdaCreditRepository.Account.DeleteAccountData(Employee.Id);
        
        Console.WriteLine("Employee data successfuly deleted.");
        Console.WriteLine("\nPress enter to return to Employee's menu");
        Console.ReadLine();
    }

    public static void ChangeEmployeeData()
    {
        Console.Clear();

        Console.Write("Please insert Employee's document (e.g.: ID): ");
        string? document = Console.ReadLine();
        while (document == null)
        {
            Console.Write("Invalid document, please insert again: ");
            document = Console.ReadLine();
        }

        List<AdaCreditRepository.EmployeeData> EmployeesList = AdaCreditRepository.Employee.GetAllEmployees();

        AdaCreditRepository.EmployeeData? Employee = EmployeesList.FirstOrDefault(Employee => Employee.Document == document);

        if (Employee == null) {
            Console.WriteLine($"No Employee registered with document {document}");
            Console.WriteLine("\nPress enter to return to Employee's menu");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("\nEmployee found.");
        
        Console.Write("Insert new Employee's name: ");        
        string? name = Console.ReadLine();
        while (name == null)
        {
            Console.Write("Invalid name, please insert again: ");
            name = Console.ReadLine();
        }

        Console.Write("Insert new Employee's document (e.g.: ID): ");        
        document = Console.ReadLine();
        while (document == null)
        {
            Console.Write("Invalid document, please insert again: ");
            document = Console.ReadLine();
        }
        
        while (document == null || EmployeesList.FirstOrDefault(Employee => Employee.Document == document) != Employee)
        {
            Console.Write("Document already registered, please insert another document: ");
            document = Console.ReadLine();
        }

        AdaCreditRepository.EmployeeData modifiedEmployee = new AdaCreditRepository.EmployeeData {
            Id = Employee.Id,
            Name = name,
            Document = document,
        };

        AdaCreditRepository.Employee.ChangeEmployeeData(Employee.Id, modifiedEmployee);
        
        Console.WriteLine("Employee data successfuly modified.");
        Console.WriteLine("\nPress enter to return to Employee's menu");
        Console.ReadLine();
    }
}