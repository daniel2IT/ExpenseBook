using ExpenseBook.Repository;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpenseBook.Controllers
{
    public class WorkerController : ApiController
    {
        private readonly IExpenseRepository _repository;
        public WorkerController(IExpenseRepository repository)
        {
            _repository = repository;
        }

        // GET: Worker
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                using (CrmServiceClient service = HelperClass.getCRMService())
                {
                    // Get Collection Data
                    EntityCollection employeeCollection = HelperClass.GetEntityCollection(service, "new_employee");
                    EntityCollection employerCollection = HelperClass.GetEntityCollection(service, "new_employer");

                    return Request.CreateResponse(HttpStatusCode.OK, _repository.GetWorker(employerCollection, employeeCollection));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}