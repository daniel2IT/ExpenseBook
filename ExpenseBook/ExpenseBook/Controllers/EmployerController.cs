using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ExpenseBook.Controllers
{
    public class EmployerController : ApiController
    {
        // GET: Employeer - Using it for dropdown
        [System.Web.Http.HttpGet]
        public List<Employer> Get() 
        {
            try
            {
                List<Employer> employees = new List<Employer>();
                var service = HelperClass.getCRMServie();

                EntityCollection employeeCollection = HelperClass.Query(service, "new_employer", "", "");

                foreach (Entity app in employeeCollection.Entities)
                {
                    Employer employee = new Employer();

                    employee.EmployerName = app.Attributes["new_name"].ToString();

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