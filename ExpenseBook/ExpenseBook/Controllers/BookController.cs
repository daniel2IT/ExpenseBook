using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ExpenseBook.Controllers
{
    public class BookController : ApiController
    {

        // GET: Employer
        public List<Book> Get()
        {
            try
            {
                var executeMultiple = HelperClass.MultipleRequestSetUp();

                Entity accountEmployee = new Entity("new_employee");

                List<Book> books = new List<Book>();
                var service = HelperClass.getCRMServie();

                QueryExpression queryExpense = new QueryExpression("new_expense"); // new_employer

                queryExpense.ColumnSet.AddColumns("new_name", "statuscode", "new_date", "new_spent" , "new_vat", "new_total", "new_comment", "new_employee");

                queryExpense.Criteria.AddCondition("new_name", ConditionOperator.NotNull);
                queryExpense.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));
                queryExpense.Criteria.AddCondition("new_date", ConditionOperator.NotNull);
                queryExpense.Criteria.AddCondition("new_spent", ConditionOperator.NotNull);
                queryExpense.Criteria.AddCondition("new_vat", ConditionOperator.NotNull);
                queryExpense.Criteria.AddCondition("new_total", ConditionOperator.NotNull);
                queryExpense.Criteria.AddCondition("new_comment", ConditionOperator.NotNull);

                
                EntityCollection expenseCollection = service.RetrieveMultiple(queryExpense);


                QueryExpression queryEmployer = new QueryExpression("new_employee"); //new_expense

                queryEmployer.ColumnSet.AddColumns("new_name", "statuscode", "new_employer");

                queryEmployer.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));
                queryEmployer.Criteria.AddCondition("new_name", ConditionOperator.NotNull);

                EntityCollection employerCollection = service.RetrieveMultiple(queryEmployer);


                // Get Employeer Entity
                int bookCount = 0;
                foreach (Entity app in expenseCollection.Entities)
                {
                    Book book = new Book();

                    book.No = bookCount + 1;
                    // Get Employeer 
                   // Guid EmployerId = (Guid)((EntityReference)expenseCollection.Entities[bookCount].Attributes["new_employer"]).Id;
                   // EntityReference employeer = new EntityReference("new_employer", EmployerId);
                    book.EmployeerName = (String)((EntityReference)employerCollection.Entities[bookCount].Attributes["new_employer"]).Name; // 0

                    // Get Employee Entity Data
                   // Guid EmployeeId = (Guid)((EntityReference)expenseCollection.Entities[bookCount].Attributes["new_employee"]).Id;
                   // EntityReference employee = new EntityReference("new_employee", EmployeeId);
                    book.EmployeeName = (String)((EntityReference)expenseCollection.Entities[bookCount].Attributes["new_employee"]).Name; // 0

                    book.Project = app.Attributes["new_name"].ToString();
                    book.Date = Convert.ToDateTime(app.Attributes["new_date"]);
                    book.Spent = Convert.ToDecimal(app.GetAttributeValue<Money>("new_spent").Value);
                    book.VAT = Convert.ToDecimal(app.GetAttributeValue<Money>("new_vat").Value);
                    book.Total= Convert.ToDecimal(app.GetAttributeValue<Money>("new_total").Value);
                    book.Comment = app.Attributes["new_comment"].ToString();
                   
                    bookCount++;
                    books.Add(book);
                }

                return books;
            }
            catch(Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
