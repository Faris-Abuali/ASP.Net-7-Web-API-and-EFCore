using System;

namespace EFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var _context = new AppDbContext();

            var employee = new Employee();

            _context.Employees.Add(employee);
            // Until now it will still in memory. You have to save changes
            _context.SaveChanges();
        }
    }
}