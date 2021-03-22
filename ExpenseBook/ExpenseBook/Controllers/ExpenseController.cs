using ExpenseBook.Models;
using ExpenseBook.Repository;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
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
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                using (CrmServiceClient service = HelperClass.getCRMServie())
                {
                    // Get Collection Data
                    EntityCollection expenseCollection = HelperClass.GetEntityCollection(service, "new_expense");
                    EntityCollection employeeCollection = HelperClass.GetEntityCollection(service, "new_employee");

                    // Merge && Get Data From Collection
                    IEnumerable<Expense> getExpenses = _repository.GetExpense(expenseCollection, employeeCollection);
                    
                    return Request.CreateResponse(HttpStatusCode.OK, getExpenses);
                } 
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        // POST api/values
        [HttpPost]
        public string Post(Expense postExpense)
        {
            try
            {
                using (CrmServiceClient service = HelperClass.getCRMServie())
                {

                    // Get Employee set -> (for reference)
                    EntityCollection employeeCollection = HelperClass.GetEntityCollection(service, "new_employee");

                    // Create Expense
                    service.Create(_repository.CreateExpense(postExpense, employeeCollection, service));

                    return "Added Successfully ! ";
                }
            }
            catch (Exception ex)
            {
                return "Failed to add: " + new ArgumentException(ex.Message);
            }
        }

        // PUT api/values/5
        [HttpPut]
        public string Put(Expense putExpense)
        {
            try
            {
                var service = HelperClass.getCRMServie();

                ExecuteMultipleRequest executeMultiple = HelperClass.MultipleRequestSetUp();

                // Get Collection Data
                EntityCollection expenseCollection = HelperClass.GetEntityCollection(service, "new_expense");
                EntityCollection employeeCollection = HelperClass.GetEntityCollection(service, "new_employee");
                EntityCollection employerCollection = HelperClass.GetEntityCollection(service, "new_employer");

                _repository.UpdateExpense(executeMultiple, expenseCollection, putExpense);

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
        public string Delete(int Id)
        {
            try
            {
                var service = HelperClass.getCRMServie();

                // Get Collection Data
                EntityCollection expenseCollection = HelperClass.GetEntityCollection(service, "new_expense");

                service.Delete("new_expense", _repository.Delete(expenseCollection, Convert.ToString(Id)));

                return "Record Successfully Deleted";
            }
            catch (Exception ex)
            {
                return "Failed To Delete: " + new ArgumentException(ex.Message);
            }
        }
    }
}