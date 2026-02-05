
using System;
using System.Collections.Generic;
using System.IO;

namespace EmployeeManagementSystem
{
    // Main Program Class
    class Program
    {
        // Main method - entry point of the application
        static void Main(string[] args)
        {
            Console.WriteLine("=====================================");
            Console.WriteLine("   EMPLOYEE MANAGEMENT SYSTEM");
            Console.WriteLine("=====================================\n");
            
            // Create employee manager instance
            EmployeeManager employeeManager = new EmployeeManager();
            
            // Load existing employees from file
            employeeManager.LoadEmployees();
            
            // Main program loop
            bool exitProgram = false;
            
            while (!exitProgram)
            {
                DisplayMainMenu();
                
                Console.Write("\nEnter your choice (1-6): ");
                string choice = Console.ReadLine();
                
                // Process user choice using switch statement
                switch (choice)
                {
                    case "1":
                        employeeManager.AddEmployee();
                        break;
                    case "2":
                        employeeManager.EditEmployee();
                        break;
                    case "3":
                        employeeManager.DisplayAllEmployees();
                        break;
                    case "4":
                        employeeManager.DisplayEmployeeDetails();
                        break;
                    case "5":
                        employeeManager.DeleteEmployee();
                        break;
                    case "6":
                        exitProgram = true;
                        Console.WriteLine("\nSaving data and exiting program...");
                        employeeManager.SaveEmployees();
                        Console.WriteLine("Thank you for using Employee Management System!");
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice! Please enter a number between 1 and 6.");
                        break;
                }
                
                if (!exitProgram)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
        
        // Function to display the main menu
        static void DisplayMainMenu()
        {
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("---------");
            Console.WriteLine("1. Add New Employee");
            Console.WriteLine("2. Edit Employee Details");
            Console.WriteLine("3. Display All Employees");
            Console.WriteLine("4. View Employee Details");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Exit Program");
        }
    }
}
