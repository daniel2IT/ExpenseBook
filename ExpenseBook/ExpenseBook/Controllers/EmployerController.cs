using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

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

                QueryExpression queryEmployee = new QueryExpression("new_employer");

                queryEmployee.ColumnSet.AddColumns("new_name", "statuscode");

                queryEmployee.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));
                queryEmployee.Criteria.AddCondition("new_name", ConditionOperator.NotNull);

                EntityCollection employeeCollection = service.RetrieveMultiple(queryEmployee);


                foreach (Entity app in employeeCollection.Entities)
                {
                    Employer employee = new Employer();

                    employee.EmployerName = app.Attributes["new_name"].ToString(); // Get Current Employee Name

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