using System;

namespace EmployeeManagementSystem
{
    // Abstract base class representing a Person
    public abstract class Person
    {
        // Protected variables - encapsulation
        protected string firstName = string.Empty;
        protected string lastName = string.Empty;
        protected DateTime dateOfBirth;

        // getters and setters
        public string FirstName
        {
            get => firstName;
            set => firstName = ValidateName(value, "First name");
        }

        public string LastName
        {
            get => lastName;
            set => lastName = ValidateName(value, "Last name");
        }

        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set => dateOfBirth = ValidateDateOfBirth(value);
        }

        public int Age
        {
            get
            {
                // Calculate age based on date of birth
                DateTime today = DateTime.Today;
                int age = today.Year - dateOfBirth.Year;

                // Adjust if birthday hasn't occurred this year just subting one year from the calculated age
                if (dateOfBirth.Date > today.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
        }

        // Abstract method - must be implemented by derived classes
        public abstract string GetFullName();

        // Virtual method - can be overridden by derived classes
        public virtual string GetBasicInfo()
        {
            return $"Name: {GetFullName()}, Age: {Age}";
        }

        // Helper method for name validation
        private string ValidateName(string name, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"{fieldName} cannot be empty!");
            }
            if (name.Length < 2)
            {
                throw new ArgumentException($"{fieldName} must be at least 2 characters long!");
            }
            return name.Trim();
        }

        // Helper method for date of birth validation
        private DateTime ValidateDateOfBirth(DateTime dob)
        {
            if (dob > DateTime.Today)
            {
                throw new ArgumentException("Date of birth cannot be in the future!");
            }
            if (dob < DateTime.Today.AddYears(-100))
            {
                throw new ArgumentException("Date of birth cannot be more than 100 years ago!");
            }
            return dob;
        }
    }

    // Employee class inheriting from Person - inheritance
    public class Employee : Person
    {
        // Static variable - shared across all instances
        private static int nextId = 1000;

        // Instance variables
        private int employeeId;
        private string department = string.Empty;
        private decimal salary;
        private DateTime hireDate;
        private EmployeeType employeeType;

        // Properties
        public int EmployeeId
        {
            get => employeeId;
            internal set => employeeId = value;  // Changed from private to internal
        }

        public string Department
        {
            get => department;
            set => department = ValidateDepartment(value);
        }

        public decimal Salary
        {
            get => salary;
            set => salary = ValidateSalary(value);
        }

        public DateTime HireDate
        {
            get => hireDate;
            set => hireDate = ValidateHireDate(value);
        }

        public EmployeeType Type
        {
            get => employeeType;
            set => employeeType = value;
        }

        public int YearsOfService
        {
            get
            {
                // Calculate years of service
                DateTime today = DateTime.Today;
                int years = today.Year - hireDate.Year;

                // Adjust if hire date anniversary hasn't occurred this year
                if (hireDate.Date > today.AddYears(-years))
                {
                    years--;
                }
                return years;
            }
        }

        // Default constructor
        public Employee()
        {
            employeeId = GenerateEmployeeId();
            hireDate = DateTime.Today;
            employeeType = EmployeeType.FullTime;
        }

        // Parameterized constructor
        public Employee(string firstName, string lastName, DateTime dob,
                       string department, decimal salary, EmployeeType type)
        {
            employeeId = GenerateEmployeeId();
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dob;
            Department = department;
            Salary = salary;
            hireDate = DateTime.Today;
            employeeType = type;
        }

        // Implementation of abstract method from base class
        public override string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        // Override of virtual method from base class
        public override string GetBasicInfo()
        {
            return base.GetBasicInfo() + $", Department: {Department}, Type: {Type}";
        }

        // Method to display complete employee details
        public string GetDetailedInfo()
        {
            return $"ID: {EmployeeId}\n" +
                   $"Name: {GetFullName()}\n" +
                   $"Date of Birth: {DateOfBirth:yyyy-MM-dd}\n" +
                   $"Age: {Age}\n" +
                   $"Department: {Department}\n" +
                   $"Salary: ${Salary:N2}\n" +
                   $"Hire Date: {HireDate:yyyy-MM-dd}\n" +
                   $"Years of Service: {YearsOfService}\n" +
                   $"Employee Type: {Type}";
        }

        // Static method to generate unique employee ID
        private static int GenerateEmployeeId()
        {
            return nextId++;
        }

        // Helper method for department validation
        private string ValidateDepartment(string dept)
        {
            string[] validDepartments = { "HR", "IT", "Finance", "Marketing", "Operations", "Sales" };

            foreach (string validDept in validDepartments)
            {
                if (string.Equals(dept, validDept, StringComparison.OrdinalIgnoreCase))
                {
                    return validDept; // Return the properly capitalized version
                }
            }

            throw new ArgumentException($"Invalid department! Must be one of: {string.Join(", ", validDepartments)}");
        }

        // Helper method for salary validation
        private decimal ValidateSalary(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Salary cannot be negative!");
            }
            if (amount > 1000000)
            {
                throw new ArgumentException("Salary cannot exceed $1,000,000!");
            }
            return amount;
        }

        // Helper method for hire date validation
        private DateTime ValidateHireDate(DateTime hireDate)
        {
            if (hireDate > DateTime.Today)
            {
                throw new ArgumentException("Hire date cannot be in the future!");
            }
            if (hireDate < new DateTime(2000, 1, 1))
            {
                throw new ArgumentException("Hire date cannot be before year 2000!");
            }
            return hireDate;
        }
    }

    // Enumeration for employee types
    public enum EmployeeType
    {
        FullTime,
        PartTime,
        Contract,
        Intern
    }
}