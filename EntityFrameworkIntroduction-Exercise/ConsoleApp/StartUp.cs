using System.Globalization;
using System.Text;
using System.Xml;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni;



public class StartUp
{
    private static IQueryable<EmployeeProject> epToDelete;

    public static void Main(string[] args)
    {
        SoftUniContext dbContext = new SoftUniContext();

        string result = DeleteProjectById(dbContext);
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

    //problem 7
    public static string GetEmployeesInPeriod(SoftUniContext context)
    {
        StringBuilder sb = new StringBuilder();
        var employeesWithProjects = context.Employees
            .Where(e => e.EmployeesProjects
            .Any(ep => ep.Project.StartDate.Year >= 2001 &&
                       ep.Project.StartDate.Year <= 2003))
            .Take(10)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                ManagerFirstName = e.Manager!.FirstName,
                ManagerLastName = e.Manager!.LastName,
                Projects = e.EmployeesProjects
                    .Select(e => new
                    {
                        ProjectName = e.Project.Name,
                        StartDate = e.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        EndDate = e.Project.EndDate.HasValue ? e.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt",CultureInfo.InvariantCulture) : "not finished",
                    })
                    .ToArray()
            })
            .ToArray();

        foreach (var e in employeesWithProjects)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");
            foreach (var p in e.Projects) 
            {
                sb.AppendLine($"--{p.ProjectName} - {p.StartDate} - {p.EndDate}");
            }
        }
        return sb.ToString().TrimEnd();
    }

    //problem 14
    public static string DeleteProjectById(SoftUniContext context)
    {
        // Check if the project exists
        Project projectToDelete = context.Projects.Find(1)!;

        if (projectToDelete != null)
        {
            // Delete all rows from employeeProject that refer to the project with the selected Id
            IQueryable<EmployeeProject> epToDelete = context.EmployeeProjects
                .Where(ep => ep.ProjectId == 2);
            context.EmployeeProjects.RemoveRange(epToDelete);

            // Remove the project
            context.Projects.Remove(projectToDelete);
            context.SaveChanges();

        }
        // Return the names of the top 10 projects
        string[] projectNames = context.Projects
            .Take(10)
            .Select(p => p.Name)
            .ToArray();

        return string.Join(Environment.NewLine, projectNames);
    }
}