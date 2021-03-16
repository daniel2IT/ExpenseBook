using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseBook.Models
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        public Guid EmployerId { get; set; }
    }
}