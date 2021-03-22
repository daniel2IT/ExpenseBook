using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseBook.Models
{
    public class Expense
    {
        [Key]
        public int No { get; set; }
        [Required]
        public string EmployerName { get; set; }
        public Guid EmployerId { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        public Guid EmployeeId { get; set; }
        [Required]
        public string Project { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public String Date { get; set; }
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