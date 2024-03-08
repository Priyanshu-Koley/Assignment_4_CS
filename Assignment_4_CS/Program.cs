using System;
using System.Linq;
using System.Collections.Generic;

public class Program
{
    IList<Employee> employeeList;
    IList<Salary> salaryList;

    public Program()
    {
        // Initialize employee list
        employeeList = new List<Employee>() {
            new Employee(){ EmployeeID = 1, EmployeeFirstName = "Rajiv", EmployeeLastName = "Desai", Age = 49},
            new Employee(){ EmployeeID = 2, EmployeeFirstName = "Karan", EmployeeLastName = "Patel", Age = 32},
            new Employee(){ EmployeeID = 3, EmployeeFirstName = "Sujit", EmployeeLastName = "Dixit", Age = 28},
            new Employee(){ EmployeeID = 4, EmployeeFirstName = "Mahendra", EmployeeLastName = "Suri", Age = 26},
            new Employee(){ EmployeeID = 5, EmployeeFirstName = "Divya", EmployeeLastName = "Das", Age = 20},
            new Employee(){ EmployeeID = 6, EmployeeFirstName = "Ridhi", EmployeeLastName = "Shah", Age = 60},
            new Employee(){ EmployeeID = 7, EmployeeFirstName = "Dimple", EmployeeLastName = "Bhatt", Age = 53}
        };

        // Initialize salary list
        salaryList = new List<Salary>() {
            new Salary(){ EmployeeID = 1, Amount = 1000, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 1, Amount = 500, Type = SalaryType.Performance},
            new Salary(){ EmployeeID = 1, Amount = 100, Type = SalaryType.Bonus},
            new Salary(){ EmployeeID = 2, Amount = 3000, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 2, Amount = 1000, Type = SalaryType.Bonus},
            new Salary(){ EmployeeID = 3, Amount = 1500, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 4, Amount = 2100, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 5, Amount = 2800, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 5, Amount = 600, Type = SalaryType.Performance},
            new Salary(){ EmployeeID = 5, Amount = 500, Type = SalaryType.Bonus},
            new Salary(){ EmployeeID = 6, Amount = 3000, Type = SalaryType.Monthly},
            new Salary(){ EmployeeID = 6, Amount = 400, Type = SalaryType.Performance},
            new Salary(){ EmployeeID = 7, Amount = 4700, Type = SalaryType.Monthly}
        };
    }

    public static void Main()
    {
        Program program = new Program();

        program.Task1();

        program.Task2();

        program.Task3();
    }

    public void Task1()
    {
        // Task 1: Print total Salary of all employees with their corresponding names in ascending order of salary

        // LINQ query to join employeeList and salaryList, group by employee name, and calculate total salary
        var totalSalaries = from employee in employeeList
                            join salary in salaryList on employee.EmployeeID equals salary.EmployeeID
                            group salary.Amount by new { employee.EmployeeFirstName, employee.EmployeeLastName } into g
                            select new
                            {
                                Name = g.Key.EmployeeFirstName + " " + g.Key.EmployeeLastName,
                                TotalSalary = g.Sum()
                            };

        // Order total salaries in ascending order
        var sortedSalaries = totalSalaries.OrderBy(s => s.TotalSalary);

        // Print the result
        Console.WriteLine("Task 1: Total Salary of all employees with their corresponding names in ascending order of salary:");
        foreach (var item in sortedSalaries)
        {
            Console.WriteLine($"{item.Name}: {item.TotalSalary}");
        }
        Console.WriteLine();
    }

    public void Task2()
    {
        // Task 2: Print Employee details of the second oldest employee including his/her total monthly salary

        // Get the second oldest employee
        var sortedEmployeesByAge = employeeList.OrderBy(e => e.Age);
        var secondOldestEmployee = sortedEmployeesByAge.Skip(1).First(); // Skip 1 to get the second oldest

        // Calculate total monthly salary of the second oldest employee
        var monthlySalary = salaryList.Where(s => s.EmployeeID == secondOldestEmployee.EmployeeID && s.Type == SalaryType.Monthly).Sum(s => s.Amount);
        var performanceSalary = salaryList.Where(s => s.EmployeeID == secondOldestEmployee.EmployeeID && s.Type == SalaryType.Performance).Sum(s => s.Amount);
        var bonusSalary = salaryList.Where(s => s.EmployeeID == secondOldestEmployee.EmployeeID && s.Type == SalaryType.Bonus).Sum(s => s.Amount);
        var totalMonthlySalary = monthlySalary + performanceSalary + bonusSalary;

        // Print the result
        Console.WriteLine("Task 2: Employee details of the second oldest employee including total monthly salary:");
        Console.WriteLine($"Employee ID: {secondOldestEmployee.EmployeeID}");
        Console.WriteLine($"Name: {secondOldestEmployee.EmployeeFirstName} {secondOldestEmployee.EmployeeLastName}");
        Console.WriteLine($"Age: {secondOldestEmployee.Age}");
        Console.WriteLine($"Total Monthly Salary: {totalMonthlySalary}");
        Console.WriteLine();
    }

    public void Task3()
    {
        // Task 3: Print means of Monthly, Performance, Bonus salary of employees whose age is greater than 30

        // LINQ query to filter employees by age, group by employee, and calculate mean salaries
        var averageSalaries = employeeList.Where(e => e.Age > 30)
                                           .GroupJoin(salaryList,
                                                      emp => emp.EmployeeID,
                                                      sal => sal.EmployeeID,
                                                      (emp, sal) => new
                                                      {
                                                          Employee = emp,
                                                          MonthlySalary = sal.Where(s => s.Type == SalaryType.Monthly).Select(s => s.Amount).DefaultIfEmpty(0).Average(),
                                                          PerformanceSalary = sal.Where(s => s.Type == SalaryType.Performance).Select(s => s.Amount).DefaultIfEmpty(0).Average(),
                                                          BonusSalary = sal.Where(s => s.Type == SalaryType.Bonus).Select(s => s.Amount).DefaultIfEmpty(0).Average()
                                                      });

        // Print the result
        Console.WriteLine("Task 3: Means of Monthly, Performance, Bonus salary of employees whose age is greater than 30:");
        foreach (var item in averageSalaries)
        {
            Console.WriteLine($"Employee: {item.Employee.EmployeeFirstName} {item.Employee.EmployeeLastName}");
            Console.WriteLine($"Monthly Salary: {item.MonthlySalary}");
            Console.WriteLine($"Performance Salary: {item.PerformanceSalary}");
            Console.WriteLine($"Bonus Salary: {item.BonusSalary}");
            Console.WriteLine();
        }
    }
}

public enum SalaryType
{
    Monthly,
    Performance,
    Bonus
}

public class Employee
{
    public int EmployeeID { get; set; }
    public string EmployeeFirstName { get; set; }
    public string EmployeeLastName { get; set; }
    public int Age { get; set; }
}

public class Salary
{
    public int EmployeeID { get; set; }
    public int Amount { get; set; }
    public SalaryType Type { get; set; }
}
