using System.Text;
using System.Xml;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni;



public class StartUp
{
    static void Main(String[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();

        String result = GetEmployeesFullInformation(dbContext);
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
            sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} " + String.Format("{0:F2}", e.Salary));
        }
        return sb.ToString().TrimEnd();
    }
}
