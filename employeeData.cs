using System;
using System.Runtime.InteropServices;

namespace EmployeeManagementSystem
{
    // Structure to represent employee data for file operations
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public struct EmployeeData
    {
        // Structure fields
        public int EmployeeId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string FirstName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string LastName;

        public long DateOfBirthBinary; // Store as binary for file I/O
        public decimal Salary;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string Department;

        public long HireDateBinary; // Store as binary for file I/O
        public int EmployeeType;

        // Method to convert from Employee class to EmployeeData struct
        public static EmployeeData FromEmployee(Employee emp)
        {
            EmployeeData data = new EmployeeData
            {
                EmployeeId = emp.EmployeeId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                DateOfBirthBinary = emp.DateOfBirth.ToBinary(),
                Salary = emp.Salary,
                Department = emp.Department,
                HireDateBinary = emp.HireDate.ToBinary(),
            };
            return data;
        }

        // Method to convert from EmployeeData struct to Employee class
        public Employee ToEmployee()
        {
            return new Employee
            {
                EmployeeId = this.EmployeeId,
                FirstName = this.FirstName,
                LastName = this.LastName,
                DateOfBirth = DateTime.FromBinary(this.DateOfBirthBinary),
                Salary = this.Salary,
                Department = this.Department,
                HireDate = DateTime.FromBinary(this.HireDateBinary),
                Type = (EmployeeType)this.EmployeeType
            };
        }
    }
}
