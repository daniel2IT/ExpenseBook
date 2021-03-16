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
                List<Employer> employerList = new List<Employer>();
                var service = HelperClass.getCRMServie();

                EntityCollection employerCollection = HelperClass.GetEntityCollection(service, "new_employer");

                foreach (Entity employer in employerCollection.Entities)
                {
                    Employer employerModel = new Employer();

                    // Get Employer
                    employerModel.EmployerId = employer.Id;
                    employerModel.EmployerName = employer.Attributes["new_name"].ToString();

                    employerList.Add(employerModel);
                }

                return Request.CreateResponse(HttpStatusCode.OK, employerList);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}

/*using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
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
                using (CrmServiceClient service = HelperClass.getCRMServie())
                {
                    List<Employer> employerList = new List<Employer>();
               

                    foreach (Entity employer in employerCollection.Entities)
                    {
                        Employer employerModel = new Employer();

                        // Get Employer
                        employerModel.EmployerId = employer.Id;
                        employerModel.EmployerName = employer.Attributes["new_name"].ToString();

                        employerList.Add(employerModel);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, employerList);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}*/