using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace EmployeeManagementSystem
{
    // Class to manage employee operations
    public class EmployeeManager
    {
        // Collection to store employees
        private List<Employee> employees;

        // Constant for file path
        private const string DataFilePath = "employees.dat";

        // Constructor
        public EmployeeManager()
        {
            employees = new List<Employee>();
            Console.WriteLine("Employee Manager initialized.\n");
        }

        // Method to add a new employee
        public void AddEmployee()
        {
            Console.WriteLine("\n=== ADD NEW EMPLOYEE ===\n");

            try
            {
                Employee newEmployee = new Employee();

                // Get employee details from user with validation
                Console.Write("Enter First Name: ");
                newEmployee.FirstName = GetValidName("First Name");

                Console.Write("Enter Last Name: ");
                newEmployee.LastName = GetValidName("Last Name");

                // Get and validate date of birth (REQUIRES valid date)
                newEmployee.DateOfBirth = GetValidDateOfBirth();

                // Get and validate department
                newEmployee.Department = GetValidDepartment();

                // Get and validate salary
                newEmployee.Salary = GetValidSalary();

                // Get employee type
                newEmployee.Type = GetValidEmployeeType();

                // Add employee to collection
                employees.Add(newEmployee);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✅ Employee added successfully!");
                Console.WriteLine($"   Employee ID: {newEmployee.EmployeeId}");
                Console.WriteLine($"   Name: {newEmployee.GetFullName()}");
                Console.WriteLine($"   Age: {newEmployee.Age} years");
                Console.WriteLine($"   Department: {newEmployee.Department}");
                Console.WriteLine($"   Salary: ${newEmployee.Salary:N2}");
                Console.WriteLine($"   Type: {newEmployee.Type}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Error adding employee: {ex.Message}");
                Console.ResetColor();
            }
        }

        // Helper method for date validation - IMPROVED
        private DateTime GetValidDateOfBirth()
        {
            while (true)
            {
                Console.Write("Enter Date of Birth (YYYY-MM-DD): ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(" [Example: 1990-05-15]");
                Console.ResetColor();

                string? input = Console.ReadLine();

                // Try parsing with exact format
                if (DateTime.TryParseExact(input, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime dob))
                {
                    // Check if date is in the future
                    if (dob > DateTime.Today)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Date of birth cannot be in the future!");
                        Console.ResetColor();
                        continue;
                    }

                    // Calculate exact age
                    int age = CalculateAge(dob);

                    // Validate age range (16-100 years)
                    if (age < 16)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"❌ Employee must be at least 16 years old! (Would be {age} years)");
                        Console.ResetColor();
                        continue;
                    }
                    else if (age > 100)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"❌ Employee cannot be older than 100 years! (Would be {age} years)");
                        Console.ResetColor();
                        continue;
                    }

                    // Additional check: date shouldn't be too old
                    if (dob < new DateTime(1900, 1, 1))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ Date of birth cannot be before 1900!");
                        Console.ResetColor();
                        continue;
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✓ Valid date! Age: {age} years");
                    Console.ResetColor();
                    return dob;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("❌ Invalid date format! Please use exactly: YYYY-MM-DD");
                    Console.WriteLine("   Example: 1990-05-15 (May 15, 1990)");
                    Console.ResetColor();
                }
            }
        }

        // Helper method to calculate exact age
        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            // Adjust if birthday hasn't occurred this year
            if (birthDate.Date > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        // Helper method for name validation
        private string GetValidName(string fieldName)
        {
            while (true)
            {
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{fieldName} cannot be empty. Enter {fieldName}: ");
                    Console.ResetColor();
                    continue;
                }

                if (input.Length < 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{fieldName} must be at least 2 characters. Enter {fieldName}: ");
                    Console.ResetColor();
                    continue;
                }

                // Check if name contains only letters, hyphens, apostrophes and spaces
                if (!Regex.IsMatch(input, @"^[a-zA-Z\s\-']+$"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{fieldName} can only contain letters, spaces, hyphens (-) and apostrophes ('). Enter {fieldName}: ");
                    Console.ResetColor();
                    continue;
                }

                return input.Trim();
            }
        }

        // Helper method for department validation
        private string GetValidDepartment()
        {
            string[] validDepartments = { "HR", "IT", "Finance", "Marketing", "Operations", "Sales" };

            while (true)
            {
                Console.Write($"Enter Department ({string.Join(", ", validDepartments)}): ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Department cannot be empty!");
                    Console.ResetColor();
                    continue;
                }

                foreach (string dept in validDepartments)
                {
                    if (string.Equals(input, dept, StringComparison.OrdinalIgnoreCase))
                    {
                        return dept; // Return properly capitalized version
                    }
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid department! Must be one of: {string.Join(", ", validDepartments)}");
                Console.ResetColor();
            }
        }

        // Helper method for salary validation
        private decimal GetValidSalary()
        {
            while (true)
            {
                Console.Write("Enter Salary: $");
                string? input = Console.ReadLine();

                if (decimal.TryParse(input, out decimal salary))
                {
                    if (salary < 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Salary cannot be negative!");
                        Console.ResetColor();
                        continue;
                    }

                    if (salary > 1000000)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Salary cannot exceed $1,000,000!");
                        Console.ResetColor();
                        continue;
                    }

                    // Warning for low salary
                    if (salary < 300)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"⚠ Salary ${salary:N2} is below typical minimum wage. Is this correct? (Y/N): ");
                        Console.ResetColor();

                        if (Console.ReadLine()?.ToUpper() != "Y")
                        {
                            continue;
                        }
                    }

                    return salary;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid salary amount! Please enter a valid number (e.g., 50000 or 75000.50)");
                    Console.ResetColor();
                }
            }
        }

        // Helper method for employee type validation
        private EmployeeType GetValidEmployeeType()
        {
            while (true)
            {
                Console.WriteLine("Select Employee Type:");
                Console.WriteLine("1. Full Time");
                Console.WriteLine("2. Part Time");
                Console.WriteLine("3. Contract");
                Console.WriteLine("4. Intern");
                Console.Write("Enter choice (1-4): ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": return EmployeeType.FullTime;
                    case "2": return EmployeeType.PartTime;
                    case "3": return EmployeeType.Contract;
                    case "4": return EmployeeType.Intern;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice! Please enter 1, 2, 3, or 4.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        // Method to edit employee details - IMPROVED
        public void EditEmployee()
        {
            Console.WriteLine("\n=== EDIT EMPLOYEE DETAILS ===\n");

            if (employees.Count == 0)
            {
                Console.WriteLine("No employees to edit.");
                return;
            }

            Console.Write("Enter Employee ID to edit: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Employee? employeeToEdit = FindEmployeeById(id);

                if (employeeToEdit != null)
                {
                    Console.WriteLine($"\nCurrent details for Employee ID {id}:");
                    Console.WriteLine(employeeToEdit.GetDetailedInfo());

                    Console.WriteLine("\nEnter new details (press Enter to keep current value):");

                    try
                    {
                        // Edit First Name with validation
                        Console.Write($"First Name ({employeeToEdit.FirstName}): ");
                        string? input = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            // Validate name
                            if (input.Length < 2)
                            {
                                Console.WriteLine("First name must be at least 2 characters. Keeping current value.");
                            }
                            else if (!Regex.IsMatch(input, @"^[a-zA-Z\s\-']+$"))
                            {
                                Console.WriteLine("Invalid characters. Keeping current value.");
                            }
                            else
                            {
                                employeeToEdit.FirstName = input.Trim();
                            }
                        }

                        // Edit Last Name with validation
                        Console.Write($"Last Name ({employeeToEdit.LastName}): ");
                        input = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            if (input.Length < 2)
                            {
                                Console.WriteLine("Last name must be at least 2 characters. Keeping current value.");
                            }
                            else if (!Regex.IsMatch(input, @"^[a-zA-Z\s\-']+$"))
                            {
                                Console.WriteLine("Invalid characters. Keeping current value.");
                            }
                            else
                            {
                                employeeToEdit.LastName = input.Trim();
                            }
                        }

                        // Edit Department with validation
                        Console.Write($"Department ({employeeToEdit.Department}): ");
                        input = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            try
                            {
                                employeeToEdit.Department = input;
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine($"Invalid department: {ex.Message}. Keeping current value.");
                            }
                        }

                        // Edit Salary with validation
                        Console.Write($"Salary (${employeeToEdit.Salary:N2}): ");
                        input = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            if (decimal.TryParse(input, out decimal newSalary))
                            {
                                try
                                {
                                    employeeToEdit.Salary = newSalary;
                                }
                                catch (ArgumentException ex)
                                {
                                    Console.WriteLine($"Invalid salary: {ex.Message}. Keeping current value.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid salary format. Keeping current value.");
                            }
                        }

                        // Edit Employee Type
                        Console.WriteLine($"Current Type: {employeeToEdit.Type}");
                        Console.Write("Change employee type? (Y/N): ");
                        if (Console.ReadLine()?.ToUpper() == "Y")
                        {
                            employeeToEdit.Type = GetValidEmployeeType();
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n✅ Employee details updated successfully!");
                        Console.ResetColor();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"\n❌ Error updating employee: {ex.Message}");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.WriteLine($"\nEmployee with ID {id} not found.");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid Employee ID format.");
            }
        }

        // Method to display all employees
        public void DisplayAllEmployees()
        {
            Console.WriteLine("\n=== ALL EMPLOYEES ===\n");

            if (employees.Count == 0)
            {
                Console.WriteLine("No employees in the system.");
                return;
            }

            Console.WriteLine($"Total Employees: {employees.Count}\n");
            Console.WriteLine("ID\tName\t\t\tDepartment\t\tSalary\t\tType");
            Console.WriteLine("-------------------------------------------------------------------------");

            // Loop through all employees
            foreach (Employee emp in employees)
            {
                Console.WriteLine($"{emp.EmployeeId}\t{emp.GetFullName().PadRight(20)}\t" +
                                $"{emp.Department.PadRight(15)}\t" +
                                $"${emp.Salary:N2}\t" +
                                $"{emp.Type}");
            }
        }

        // Method to display specific employee details
        public void DisplayEmployeeDetails()
        {
            Console.WriteLine("\n=== VIEW EMPLOYEE DETAILS ===\n");

            if (employees.Count == 0)
            {
                Console.WriteLine("No employees in the system.");
                return;
            }

            Console.Write("Enter Employee ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Employee? employee = FindEmployeeById(id);

                if (employee != null)
                {
                    Console.WriteLine($"\n=== EMPLOYEE DETAILS ===\n");
                    Console.WriteLine(employee.GetDetailedInfo());
                }
                else
                {
                    Console.WriteLine($"\nEmployee with ID {id} not found.");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid Employee ID format.");
            }
        }

        // Method to delete an employee
        public void DeleteEmployee()
        {
            Console.WriteLine("\n=== DELETE EMPLOYEE ===\n");

            if (employees.Count == 0)
            {
                Console.WriteLine("No employees to delete.");
                return;
            }

            Console.Write("Enter Employee ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Employee? employeeToDelete = FindEmployeeById(id);

                if (employeeToDelete != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n⚠ Are you sure you want to delete:");
                    Console.WriteLine($"   Employee ID: {employeeToDelete.EmployeeId}");
                    Console.WriteLine($"   Name: {employeeToDelete.GetFullName()}");
                    Console.WriteLine($"   Department: {employeeToDelete.Department}");
                    Console.WriteLine($"   Salary: ${employeeToDelete.Salary:N2}");
                    Console.Write("\nConfirm deletion (Y/N): ");
                    Console.ResetColor();

                    string? confirmation = Console.ReadLine();

                    if (confirmation?.ToUpper() == "Y")
                    {
                        employees.Remove(employeeToDelete);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n✅ Employee {employeeToDelete.GetFullName()} deleted successfully!");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\nDeletion cancelled.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.WriteLine($"\nEmployee with ID {id} not found.");
                }
            }
            else
            {
                Console.WriteLine("\nInvalid Employee ID format.");
            }
        }

        // Method to save employees to file
        public void SaveEmployees()
        {
            try
            {
                using (FileStream fs = new FileStream(DataFilePath, FileMode.Create))
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    // Write number of employees
                    writer.Write(employees.Count);

                    // Write each employee as binary data
                    foreach (Employee emp in employees)
                    {
                        EmployeeData data = EmployeeData.FromEmployee(emp);

                        writer.Write(data.EmployeeId);
                        writer.Write(data.FirstName);
                        writer.Write(data.LastName);
                        writer.Write(data.DateOfBirthBinary);
                        writer.Write(data.Salary);
                        writer.Write(data.Department);
                        writer.Write(data.HireDateBinary);
                        writer.Write(data.EmployeeType);
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n✅ Employee data saved to {DataFilePath}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Error saving employee data: {ex.Message}");
                Console.ResetColor();
            }
        }

        // Method to load employees from file
        public void LoadEmployees()
        {
            try
            {
                if (File.Exists(DataFilePath))
                {
                    using (FileStream fs = new FileStream(DataFilePath, FileMode.Open))
                    using (BinaryReader reader = new BinaryReader(fs))
                    {
                        // Read number of employees
                        int count = reader.ReadInt32();

                        // Clear current list
                        employees.Clear();

                        // Read each employee
                        for (int i = 0; i < count; i++)
                        {
                            EmployeeData data = new EmployeeData
                            {
                                EmployeeId = reader.ReadInt32(),
                                FirstName = reader.ReadString(),
                                LastName = reader.ReadString(),
                                DateOfBirthBinary = reader.ReadInt64(),
                                Salary = reader.ReadDecimal(),
                                Department = reader.ReadString(),
                                HireDateBinary = reader.ReadInt64(),
                                EmployeeType = reader.ReadInt32()
                            };

                            employees.Add(data.ToEmployee());
                        }
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n✅ Loaded {employees.Count} employees from {DataFilePath}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nℹ No existing employee data file found. Starting with empty database.");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n❌ Error loading employee data: {ex.Message}");
                Console.ResetColor();
            }
        }

        // Helper method to find employee by ID
        private Employee? FindEmployeeById(int id)
        {
            // Use loop to search for employee
            foreach (Employee emp in employees)
            {
                if (emp.EmployeeId == id)
                {
                    return emp;
                }
            }
            return null;
        }

        // Method to get statistics about employees
        public void DisplayStatistics()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("\nNo employees for statistics.");
                return;
            }

            decimal totalSalary = 0;
            decimal minSalary = decimal.MaxValue;
            decimal maxSalary = decimal.MinValue;
            Dictionary<string, int> departmentStats = new Dictionary<string, int>();
            Dictionary<EmployeeType, int> typeStats = new Dictionary<EmployeeType, int>();

            // Initialize dictionaries
            string[] departments = { "HR", "IT", "Finance", "Marketing", "Operations", "Sales" };
            foreach (var dept in departments)
            {
                departmentStats[dept] = 0;
            }

            foreach (var type in Enum.GetValues<EmployeeType>())
            {
                typeStats[type] = 0;
            }

            // Calculate statistics
            foreach (Employee emp in employees)
            {
                totalSalary += emp.Salary;

                // Track min/max salary
                if (emp.Salary < minSalary) minSalary = emp.Salary;
                if (emp.Salary > maxSalary) maxSalary = emp.Salary;

                // Department count
                if (departmentStats.ContainsKey(emp.Department))
                {
                    departmentStats[emp.Department]++;
                }

                // Employee type count
                typeStats[emp.Type]++;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=== EMPLOYEE STATISTICS ===");
            Console.ResetColor();

            Console.WriteLine($"Total Employees: {employees.Count}");
            Console.WriteLine($"Average Salary: ${(totalSalary / employees.Count):N2}");
            Console.WriteLine($"Minimum Salary: ${minSalary:N2}");
            Console.WriteLine($"Maximum Salary: ${maxSalary:N2}");
            Console.WriteLine($"Total Salary Expense: ${totalSalary:N2}");

            Console.WriteLine("\nDepartment Distribution:");
            foreach (var dept in departments)
            {
                if (departmentStats[dept] > 0)
                {
                    double percentage = (departmentStats[dept] / (double)employees.Count) * 100;
                    Console.WriteLine($"  {dept}: {departmentStats[dept]} employees ({percentage:F1}%)");
                }
            }

            Console.WriteLine("\nEmployee Type Distribution:");
            foreach (var type in typeStats)
            {
                if (type.Value > 0)
                {
                    double percentage = (type.Value / (double)employees.Count) * 100;
                    Console.WriteLine($"  {type.Key}: {type.Value} employees ({percentage:F1}%)");
                }
            }

            // Age statistics
            if (employees.Count > 0)
            {
                int totalAge = 0;
                int minAge = int.MaxValue;
                int maxAge = int.MinValue;

                foreach (Employee emp in employees)
                {
                    totalAge += emp.Age;
                    if (emp.Age < minAge) minAge = emp.Age;
                    if (emp.Age > maxAge) maxAge = emp.Age;
                }

                Console.WriteLine($"\nAge Statistics:");
                Console.WriteLine($"  Average Age: {totalAge / employees.Count} years");
                Console.WriteLine($"  Youngest Employee: {minAge} years");
                Console.WriteLine($"  Oldest Employee: {maxAge} years");
            }
        }
    }
}