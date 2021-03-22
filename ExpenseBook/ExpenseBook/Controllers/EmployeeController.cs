using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
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
        // GET: Employee 
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                /*   using (CrmServiceClient service = HelperClass.getCRMServie())
                   {
                   }*/

                    CrmServiceClient service = HelperClass.getCRMServie();

                    
                    List<Employee> employeesList = new List<Employee>();
                    EntityCollection employeeCollection = HelperClass.GetEntityCollection(service, "new_employee");

                    foreach (Entity employee in employeeCollection.Entities)
                    {
                        Employee employeeModel = new Employee();

                        employeeModel.EmployeeName = employee.Attributes["new_name"].ToString();

                        // Get Employee ID
                        employeeModel.EmployeeId = (Guid)employee.Attributes["new_employeeid"];

                        // Get Employer ID
                        employeeModel.EmployerId = employeeCollection.Entities.FirstOrDefault(entity => entity.Id == employeeModel.EmployeeId).GetAttributeValue<EntityReference>("new_employer").Id;

                        employeesList.Add(employeeModel);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, employeesList);
               
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
