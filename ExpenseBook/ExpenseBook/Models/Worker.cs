using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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