using System.Text;
using System.Xml;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni;



public class StartUp
{
    static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();

        string result = AddNewAddressToEmployee(dbContext);
        Console.WriteLine(result);
    }


    //problem 3
    public static string GetEmployeesFullInformation(SoftUniContext context)
    {
        StringBuilder sb  = new StringBuilder();
        var employees = context
            .Employees
            .OrderBy(e => e.EmployeeId)
            .Select(e => new
            {
                e.FirstName, 
                e.LastName, 
                e.MiddleName, 
                e.JobTitle, 
                e.Salary
            })
            .ToArray();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle}" + String.Format("{0:F2}", e.Salary));
        }
        return sb.ToString().TrimEnd();
    }

    //problem 4
    public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();
        var employees = context
            .Employees
            .OrderBy(e => e.FirstName)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            })
            .Where(e => e.Salary > 50000)
            .ToArray();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} - ${e.Salary:f2}");
        }
        return sb.ToString().TrimEnd();
    }

    //problem 5
    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
    {
            StringBuilder sb = new StringBuilder();
            var employees = context
                .Employees
                .Where(e => e.Department.Name.Equals("Research and Development"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .ToArray();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} - ${e.Salary:f2}");
            }
            return sb.ToString().TrimEnd();
    }

    //problem 6
    public static string AddNewAddressToEmployee(SoftUniContext context)
    {
        Address newAddress = new Address()
       
        {
            AddressText = "Vitoshka 15",
            TownId = 4,
        };

        Employee employee = context.Employees
            .FirstOrDefault(e => e.LastName == "Nakov");
        employee!.Address = newAddress;

        context.SaveChanges();
        //the address is automatically added through the employee address add

        string[] employeeAddresses = context.Employees
            .OrderByDescending(e => e.AddressId)
            .Take(10) //this is how we take the first 10 rows
            .Select(e => e.Address.AddressText)
            .ToArray();

      return String.Join(Environment.NewLine, employeeAddresses);
    }
}