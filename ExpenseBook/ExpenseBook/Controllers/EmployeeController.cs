using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpenseBook.Controllers
{
    public class EmployeeController : ApiController
    {
        // GET: Employee - Using it for dropdown
        [HttpGet]
        public HttpResponseMessage Get() //
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

                    // Get Employer
                    Guid EmployeeId = (Guid)app.Attributes["new_employeeid"];
                    employee.EmployerName = employeeCollection.Entities.FirstOrDefault(x => x.Id == EmployeeId).GetAttributeValue<EntityReference>("new_employer").Name.ToString();

                    employees.Add(employee);
                 }

                return Request.CreateResponse(HttpStatusCode.OK, employees);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}