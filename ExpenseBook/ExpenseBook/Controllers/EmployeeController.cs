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
        // GET: Employee 
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                List<Employee> employeesList = new List<Employee>();

                var service = HelperClass.getCRMServie();

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
/*using ExpenseBook.Models;
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
                List<Employee> employeesList = new List<Employee>();
                using (CrmServiceClient service = HelperClass.getCRMServie())
                {

                        EntityCollection employeeCollection = HelperClass.GetEntityCollection(service, "new_employee");

                        foreach (Entity app in employeeCollection.Entities)
                        {
                            Employee employeeEntity = new Employee();

                            // Get Employee
                            employeeEntity.EmployeeId = app.Id;
                            employeeEntity.EmployeeName = app.Attributes["new_name"].ToString();

                            // Get Employer
                            Guid EmployeeId = (Guid)app.Attributes["new_employeeid"];
                            employeeEntity.EmployerName = employeeCollection.Entities.FirstOrDefault(x => x.Id == EmployeeId).GetAttributeValue<EntityReference>("new_employer").Name.ToString();

                            employeesList.Add(employeeEntity);
                        }
                }
                return Request.CreateResponse(HttpStatusCode.OK, employeesList);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}*/