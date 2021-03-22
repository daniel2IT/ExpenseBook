using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseBook.Models
{
    public class Worker
    {
        // Employee section
        public Guid EmployeeId { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        public Guid EmployerRefId { get; set; }

        // Employer section
        public Guid EmployerId { get; set; }
        [Required]
        public string EmployerName { get; set; }

    }
}