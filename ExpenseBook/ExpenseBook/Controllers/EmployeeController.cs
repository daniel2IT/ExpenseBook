using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                List<Employee> employees = new List<Employee>();
                var service = HelperClass.getCRMServie();

                QueryExpression queryEmployee = new QueryExpression("new_employee");

                queryEmployee.ColumnSet.AddColumns("new_name", "statuscode");

                queryEmployee.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));
                queryEmployee.Criteria.AddCondition("new_name", ConditionOperator.NotNull);

                EntityCollection employeeCollection = service.RetrieveMultiple(queryEmployee);


                foreach (Entity app in employeeCollection.Entities) 
                {
                    Employee employee = new Employee();

                    // Get Employee
                    employee.EmployeeName = app.Attributes["new_name"].ToString(); // Get Current Employee Name

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