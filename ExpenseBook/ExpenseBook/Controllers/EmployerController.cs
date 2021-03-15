using ExpenseBook.Models;
using ExpenseBook.Repository;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpenseBook.Controllers
{
    public class EmployerController : ApiController
    {
        // GET: Employeer
        [HttpGet]
        public HttpResponseMessage Get() 
        {
            try
            {
                List<Employer> employer = new List<Employer>();
                var service = HelperClass.getCRMServie();

                EntityCollection employeeCollection = HelperClass.Query(service, "new_employer", "", "");

                foreach (Entity app in employeeCollection.Entities)
                {
                    Employer employee = new Employer();

                    employee.EmployerName = app.Attributes["new_name"].ToString();

                    employer.Add(employee);
                }

                return Request.CreateResponse(HttpStatusCode.OK, employer);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}