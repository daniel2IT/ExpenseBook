using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ExpenseBook.Controllers
{
    public class EmployeeController : ApiController
    {
        // GET: Employee - Using it for dropdown
        [HttpGet]
        public List<Employee> Get()
        {
            try
            {
                // Employee  – dropdown‘as parodantis visus CRM‘e esančius darbuotojų, priklausančius pasirinktam Employer;
                // Emoloyer dropdown Selected posle chego siuda posupaet Employer Name ... 
                // I pagal jego , pagal Criteria, my ishem wsiex prinodlezhashix dla etogo Emloyerera
                List<Employee> employees = new List<Employee>();
                var service = HelperClass.getCRMServie();

                EntityCollection employeeCollection =  HelperClass.Query(service, "new_employee", "", "");

                foreach (Entity app in employeeCollection.Entities) 
                {
                    Employee employee = new Employee();

                    // Get Employee
                    employee.EmployeeName = app.Attributes["new_name"].ToString();

                    employees.Add(employee);
                }

                return employees;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}