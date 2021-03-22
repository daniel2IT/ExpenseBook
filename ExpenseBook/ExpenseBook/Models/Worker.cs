using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseBook.Models
{
    public class Worker
    {
        // Employee section
        [Required]
        public Guid EmployeeId { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public Guid EmployerRefId { get; set; }

        // Employer section
        [Required]
        public Guid EmployerId { get; set; }
        [Required]
        public string EmployerName { get; set; }
    }
}