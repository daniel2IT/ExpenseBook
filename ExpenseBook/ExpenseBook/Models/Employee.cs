using System.ComponentModel.DataAnnotations;

namespace ExpenseBook.Models
{
    public class Employee
    {
        [Required]
        public string EmployeeName { get; set; }
        public string EmployerName { get; set; }
    }
}