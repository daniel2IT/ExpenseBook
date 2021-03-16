using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpenseBook.Models
{
    public class Employer
    {
        public Guid EmployerId { get; set; }
        [Required]
        public string EmployerName { get; set; }
    }
}