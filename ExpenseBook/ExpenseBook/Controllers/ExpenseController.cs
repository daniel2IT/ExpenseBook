using ExpenseBook.Models;
using ExpenseBook.Repository;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpenseBook.Controllers
{
    public class ExpenseController : ApiController
    {

        private readonly IExpenseRepository _repository;

        public ExpenseController(IExpenseRepository repository)
        {
            _repository = repository; 
        }

        // GET: Books
        [Route()]
        public HttpResponseMessage Get()
        {
            try
            {
                CrmServiceClient service = HelperClass.getCRMServie();

                // Get Collection Data
                EntityCollection expenseCollection = HelperClass.GetEntityCollection(service, "new_expense");
                EntityCollection employeeCollection = HelperClass.GetEntityCollection(service, "new_employee");

                // Merge && Get Data From Collection
                IEnumerable<Expense> expenses = _repository.GetExpense(expenseCollection, employeeCollection);

                return Request.CreateResponse(HttpStatusCode.OK, expenses);
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        // POST api/values
        [HttpPost]
        public string Post(Expense expense)
        {
            try
            {
                var service = HelperClass.getCRMServie();

                ExecuteMultipleRequest executeMultiple = HelperClass.MultipleRequestSetUp();

                Entity expenseEntity = new Entity("new_expense");
                
                expenseEntity["new_no"] = Convert.ToString(HelperClass.GetMaxNo(service) + 1); 

                expenseEntity["new_name"] = expense.Project;
                expenseEntity["new_date"] = expense.Date;
                expenseEntity["new_spent"] = new Money((decimal)expense.Spent);
                expenseEntity["new_vat"] = new Money((decimal)expense.VAT);
                expenseEntity["new_total"] = new Money((decimal)expense.Total);
                expenseEntity["new_comment"] = expense.Comment;

                // Get Employee set -> (reference)
                EntityCollection employeeCollection = HelperClass.Query(service, "new_employee", expense.EmployeeName, "");
                var EmployeeId = employeeCollection.Entities[0].GetAttributeValue<Guid>("new_employeeid");
                expenseEntity["new_employee"] = new EntityReference("new_employee", EmployeeId);

                service.Create(expenseEntity);

                return "Added Successfully ! ";
            }
            catch (Exception ex)
            {
                return "Failed to add: " + new ArgumentException(ex.Message);
            }
        }

        // PUT api/values/5
        [HttpPut]
        public string Put(Expense expense)
        {
            if (!ModelState.IsValid)
            {
                return Convert.ToString(BadRequest("Not a valid model"));
            }
            try
            {
                var service = HelperClass.getCRMServie();

                ExecuteMultipleRequest executeMultiple = HelperClass.MultipleRequestSetUp();

                // Get Expense
                EntityCollection expenseCollection = HelperClass.Query(service, "new_expense", "new_expenseid", Convert.ToString(expense.No));
                Guid expenceId = expenseCollection.Entities[0].GetAttributeValue<Guid>("new_expenseid");

                Entity expenseEntity = new Entity("new_expense", expenceId);
                expenseEntity["new_name"] = expense.Project;
                expenseEntity["new_date"] = expense.Date;
                expenseEntity["new_spent"] = new Money((decimal)expense.Spent);
                expenseEntity["new_vat"] = new Money((decimal)expense.VAT);
                expenseEntity["new_total"] = new Money((decimal)expense.Total);
                expenseEntity["new_comment"] = expense.Comment;
                
                // Get Employee && Update Expense
                EntityCollection employeeCollection = HelperClass.Query(service, "new_employee", "new_employeeid", expense.EmployeeName);
                Guid employeeNewId = employeeCollection.Entities[0].GetAttributeValue<Guid>("new_employeeid");

                expenseEntity["new_employee"] = new EntityReference("new_employee", employeeNewId);
                UpdateRequest reqUpdateExpense = new UpdateRequest { Target = expenseEntity };
                executeMultiple.Requests.Add(reqUpdateExpense);
             

                // Get Employer && Update Employee
                EntityCollection employerCollection = HelperClass.Query(service, "new_employer", "new_employerid", expense.EmployeerName);
                Guid employerId = employerCollection.Entities[0].GetAttributeValue<Guid>("new_employerid");

                Entity employeeEntity = new Entity("new_employee", employeeNewId);
                employeeEntity["new_employer"] = new EntityReference("new_employer", employerId);
                UpdateRequest reqUpdateEmployee = new UpdateRequest { Target = employeeEntity };
                executeMultiple.Requests.Add(reqUpdateEmployee);

                ExecuteMultipleResponse executeMultipleResponses = (ExecuteMultipleResponse)service.Execute(executeMultiple);

                return "Updated Successfully ! ";
            }
            catch (Exception ex)
            {
                return "Failed to Update: " + new ArgumentException(ex.Message);
            }
        }

        // DELETE api/values/5
        [HttpDelete]
        public string Delete(int id)
        {
            try
            {
                var service = HelperClass.getCRMServie();

                EntityCollection expenseCollection = HelperClass.Query(service, "new_expense", "new_expenseid", Convert.ToString(id));
                Guid expenceId = expenseCollection.Entities[0].GetAttributeValue<Guid>("new_expenseid");

                service.Delete("new_expense",expenceId);

                return "Record Successfully Deleted";
            }
            catch (Exception ex)
            {
                return "Failed To Delete: " + new ArgumentException(ex.Message);
            }
        }
    }
}