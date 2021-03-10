using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ExpenseBook.Controllers
{
    public class BooksController : ApiController
    {
        private static int bookCount;
        // GET: Books
        [HttpGet]
        public List<Book> Get()
        {
            try
            {
                List<Book> books = new List<Book>();
                CrmServiceClient service = HelperClass.getCRMServie();

                // Get  Expenses 
                EntityCollection expenseCollection = HelperClass.Query(service, "new_expense", "null");
                
                // Get Employee
                EntityCollection employeeCollection = HelperClass.Query(service, "new_employee", "null");

                foreach (Entity app in expenseCollection.Entities)
                {
                    Book book = new Book();
                    book.No = Convert.ToInt32(app.Attributes["new_no"]);

                    book.EmployeeName = ((EntityReference)app.Attributes["new_employee"]).Name;

                    // Get Employeer Name
                    Guid EmployeeId = (Guid)((EntityReference)app.Attributes["new_employee"]).Id;
                    book.EmployeerName = employeeCollection.Entities.FirstOrDefault(x => x.Id == EmployeeId).GetAttributeValue<EntityReference>("new_employer").Name.ToString();

                    book.Project = app.Attributes["new_name"].ToString();
                    book.Date = Convert.ToDateTime(app.Attributes["new_date"]);
                    book.Spent = Convert.ToDecimal(app.GetAttributeValue<Money>("new_spent").Value);
                    book.VAT = Convert.ToDecimal(app.GetAttributeValue<Money>("new_vat").Value);
                    book.Total= Convert.ToDecimal(app.GetAttributeValue<Money>("new_total").Value);
                    book.Comment = app.Attributes["new_comment"].ToString();
                   
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
        [HttpPost]
        public string Post(Book book)
        {
            try
            {
                var service = HelperClass.getCRMServie();

                Entity expenseEntity = new Entity("new_expense");
                
                expenseEntity["new_no"] = Convert.ToString(HelperClass.GetMaxNo(service) + 1); 

                expenseEntity["new_name"] = book.Project;
                expenseEntity["new_date"] = book.Date;
                expenseEntity["new_spent"] = new Money((decimal)book.Spent);
                expenseEntity["new_vat"] = new Money((decimal)book.VAT);
                expenseEntity["new_total"] = new Money((decimal)book.Total);
                expenseEntity["new_comment"] = book.Comment;

                // Get Employee set -> (reference)
                EntityCollection employeeCollection = HelperClass.Query(service, "new_employee", book.EmployeeName);
                var EmployeeId = employeeCollection.Entities[0].GetAttributeValue<Guid>("new_employeeid");
                expenseEntity["new_employee"] = new EntityReference("new_employee", EmployeeId);

                // Get Employeer set -> (reference)
                EntityCollection employerCollection =  HelperClass.Query(service, "new_employer", "null");
                Entity employeeEntity = new Entity("new_employee", EmployeeId); // get employee entity
                var EmployerId = employerCollection.Entities[0].GetAttributeValue<Guid>("new_employerid");
                employeeEntity["new_employer"] = new EntityReference("new_employer", EmployerId);

                service.Create(expenseEntity);
                service.Update(employeeEntity);
                

                return "Added Successfully ! " + book.EmployeeName;
            }
            catch (Exception ex)
            {
                return "Failed to add: " + new ArgumentException(ex.Message);
            }
        }

        // PUT api/values/5
        [HttpPut]
        public string Put(Book book)
        {
            if (!ModelState.IsValid)
            {
                return Convert.ToString(BadRequest("Not a valid model"));
            }
            try
            {
                var service = HelperClass.getCRMServie();

                //////////////////////////////// Get 100% Expense ////////////////////////////////
                QueryExpression queryExpense = new QueryExpression("new_expense");
                queryExpense.ColumnSet.AddColumns("new_no", "new_name", "statuscode", "new_date", "new_spent", "new_vat", "new_total", "new_comment", "new_employee", "new_expenseid");

                queryExpense.Criteria.AddCondition("new_no", ConditionOperator.Equal, (Convert.ToString(book.No))); // Nomer tolko odin
                queryExpense.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1)); 

                EntityCollection expenseCollection = service.RetrieveMultiple(queryExpense);

                Guid expenceId = (Guid)service.RetrieveMultiple(queryExpense)[0].Attributes["new_expenseid"];
                //Guid expenceId = ((EntityReference)expenseCollection[0].Attributes["new_expenseid"]).Id;
                Entity expenseEntity = new Entity("new_expense", expenceId);

                expenseEntity["new_name"] = book.Project;
                expenseEntity["new_date"] = book.Date;
                expenseEntity["new_spent"] = new Money((decimal)book.Spent);
                expenseEntity["new_vat"] = new Money((decimal)book.VAT);
                expenseEntity["new_total"] = new Money((decimal)book.Total);
                expenseEntity["new_comment"] = book.Comment;

               
                // Imia Employee poluchili
                String prevEmployeeName = ((EntityReference)expenseCollection[0].Attributes["new_employee"]).Name;

                //////////////////////////////// Get 100% Employee ////////////////////////////////
                QueryExpression queryEmployee = new QueryExpression { EntityName = "new_employee", ColumnSet = new ColumnSet("new_name", "new_employeeid", "new_employer", "statuscode") };
               
                queryEmployee.Criteria.AddCondition("new_name", ConditionOperator.Equal, book.EmployeeName);
                queryEmployee.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));
                EntityCollection employeeCollection = service.RetrieveMultiple(queryEmployee);

                Guid EmployeeNewId = (Guid)service.RetrieveMultiple(queryEmployee)[0].Attributes["new_employeeid"]; // get id Employeer 

                expenseEntity["new_employee"] = new EntityReference("new_employee", EmployeeNewId); // Updated Expense LookUp
               
                service.Update(expenseEntity);
                ///////////////////////////////////////////////////////////////////////////////////////////
                QueryExpression queryEmployer = new QueryExpression { EntityName = "new_employer", ColumnSet = new ColumnSet("new_name", "new_employerid", "statuscode") };

                queryEmployer.Criteria.AddCondition("new_name", ConditionOperator.Equal, book.EmployeerName);
                queryEmployer.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));

                Guid EmployerId = (Guid)service.RetrieveMultiple(queryEmployer)[0].Attributes["new_employerid"]; // get id Employeer 

                // UPADTE EMPLOYEE NAME
                Entity employeeEntity = new Entity("new_employee", EmployeeNewId); // get employee entity
                employeeEntity["new_employer"] = new EntityReference("new_employer", EmployerId);
                service.Update(employeeEntity);


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

                QueryExpression queryExpense = new QueryExpression("new_expense");
                queryExpense.ColumnSet.AddColumns("new_no", "statuscode", "new_date", "new_expenseid");

                queryExpense.Criteria.AddCondition("new_no", ConditionOperator.Equal, (Convert.ToString(id))); // Nomer tolko odin
                queryExpense.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));

                EntityCollection expenseCollection = service.RetrieveMultiple(queryExpense);

                Guid expenceId = (Guid)service.RetrieveMultiple(queryExpense)[0].Attributes["new_expenseid"];
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