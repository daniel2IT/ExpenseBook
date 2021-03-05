using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpenseBook.Models
{
    public class Book
    {
        [Key]
        public int No { get; set; }
        [Required]
        public string EmployeerName { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public string Project { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Spent { get; set; }

        [Required]
        public decimal VAT { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public string Comment { get; set; }

    }
}