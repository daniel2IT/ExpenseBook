using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
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
    public class BooksController : ApiController
    {

        // GET: Books
        [System.Web.Http.HttpGet]
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

                QueryExpression queryEmployee = new QueryExpression("new_employee"); //new_expense

                queryEmployee.ColumnSet.AddColumns("new_name", "statuscode", "new_employer");

                queryEmployee.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));
                queryEmployee.Criteria.AddCondition("new_name", ConditionOperator.NotNull);

                EntityCollection employerCollection = service.RetrieveMultiple(queryEmployee);


                // Get Employeer Entity
                // If Create or Edit Langas open this .. Sdelat nado If(openedCreateWindow .. )  ! poskolku kogda zapuskaju Create, ono proxodit cherez wsio ...
                int bookCount = 0;
                foreach (Entity app in expenseCollection.Entities)
                {
                    Book book = new Book();

                    book.No = bookCount + 1;

                    // Get Employeer 

                    //  Guid EmployerId = (Guid)((EntityReference)expenseCollection.Entities[bookCount].Attributes["new_employer"]).Id;
                    // EntityReference employeer = new EntityReference("new_employer", EmployerId);


                    //book.EmployeerName = (String)((EntityReference)employerCollection.Entities[bookCount].Attributes["new_employer"]).Name; // 0

                    // Get Employee Entity Data
                    //  Guid EmployeeId = (Guid)((EntityReference)expenseCollection.Entities[bookCount].Attributes["new_employee"]).Id;
                    //  EntityReference employee = new EntityReference("new_employee", EmployeeId);

                    book.EmployeerName = (String)((EntityReference)employerCollection.Entities[bookCount].Attributes["new_employer"]).Name; // get All Employeers bad idea..

                    book.EmployeeName = (String)((EntityReference)expenseCollection.Entities[bookCount].Attributes["new_employee"]).Name; // Get Current Employee Name

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
        [System.Web.Http.HttpPost]
        public string Post(Book book)
        {
            try
            {
            var service = HelperClass.getCRMServie();

                // Create new Expense record
                
                // Jeigu employeer neturi tokio employee -> tai prideda pas save
                // Jeigu employeer turi jau employee ->> tai blogai Exceptionas , kad tas turi jau toki ... 


                Entity expenseEntity = new Entity("new_expense");
                expenseEntity["new_name"] = book.Project;
                expenseEntity["new_date"] = book.Date;
                expenseEntity["new_spent"] = new Money((decimal)book.Spent);
                expenseEntity["new_vat"] = new Money((decimal)book.VAT);
                expenseEntity["new_total"] = new Money((decimal)book.Total);
                expenseEntity["new_comment"] = book.Comment;

               

                QueryExpression queryEmployee = new QueryExpression { EntityName = "new_employee", ColumnSet = new ColumnSet("new_name", "new_employeeid") }; // Nelogiska pagal name... pataisyti
                queryEmployee.Criteria.AddCondition("new_name", ConditionOperator.Equal, book.EmployeeName);

                var EmployeeId = (Guid)service.RetrieveMultiple(queryEmployee)[0].Attributes["new_employeeid"];
                expenseEntity["new_employee"] = new EntityReference("new_employee", EmployeeId);

                Guid expenseId = service.Create(expenseEntity);


                return "Added Successfully ! " + book.EmployeeName;
            }
            catch (Exception ex)
            {
                return "Failed to add: " + new ArgumentException(ex.Message);
            }
        }

        // PUT api/values/5
        [System.Web.Http.HttpPut]
        public string Put([FromBody] string value)
        {
            return "Updated Successfully ! ";
        }

        // DELETE api/values/5
        [System.Web.Http.HttpDelete]
        public void Delete(int id)
        {
        }
    }
}