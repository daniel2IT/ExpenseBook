using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpenseBook.Models
{
    public class Employer
    {
        [Required]
        public string EmployerName { get; set; }
    }
}