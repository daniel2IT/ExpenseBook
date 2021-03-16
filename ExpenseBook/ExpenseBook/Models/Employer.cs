using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseBook.Models
{
    public class Employer
    {
        public Guid EmployerId { get; set; }
        [Required]
        public string EmployerName { get; set; }
    }
}