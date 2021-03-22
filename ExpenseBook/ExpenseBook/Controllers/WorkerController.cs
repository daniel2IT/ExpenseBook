using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ExpenseBook.Controllers
{
    public class WorkerController : ApiController
    {
        // GET: Employeer
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                using (CrmServiceClient service = HelperClass.getCRMServie())
                {

                    // Get Employer Section
                    List<Worker> workerList = new List<Worker>();

                    EntityCollection employerCollection = HelperClass.GetEntityCollection(service, "new_employer");
                    foreach (Entity employer in employerCollection.Entities)
                    {
                        Worker employerModel = new Worker();
                        
                        employerModel.EmployerId = employer.Id;
                        employerModel.EmployerName = employer.Attributes["new_name"].ToString();

                        workerList.Add(employerModel);
                    }


                    // Get Employee Section
                    EntityCollection employeeCollection = HelperClass.GetEntityCollection(service, "new_employee");
                    foreach (Entity employee in employeeCollection.Entities)
                    {
                        Worker employeeModel = new Worker();

                        employeeModel.EmployeeName = employee.Attributes["new_name"].ToString();

                        // Get Employee ID
                        employeeModel.EmployeeId = (Guid)employee.Attributes["new_employeeid"];

                        // Get Employer ID
                        employeeModel.EmployerRefId = employeeCollection.Entities.FirstOrDefault(entity => entity.Id == employeeModel.EmployeeId).GetAttributeValue<EntityReference>("new_employer").Id;

                        workerList.Add(employeeModel);
                    }


                    return Request.CreateResponse(HttpStatusCode.OK, workerList);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}