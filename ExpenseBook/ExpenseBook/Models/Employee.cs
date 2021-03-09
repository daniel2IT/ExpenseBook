using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpenseBook.Models
{
    public class Employee
    {
        [Required]
        public string EmployeeName { get; set; }
    }
}