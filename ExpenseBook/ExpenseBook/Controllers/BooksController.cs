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
        private static int bookCount;
        // GET: Books
        [System.Web.Http.HttpGet]
        public List<Book> Get()
        {
            try
            {
                bookCount = 0; // refresh No.
                List<Book> books = new List<Book>();
                var service = HelperClass.getCRMServie();

                /* Get All Expenses ... */
                QueryExpression queryExpense = new QueryExpression("new_expense"); // new_employer
                queryExpense.ColumnSet.AddColumns("new_name", "statuscode", "new_date", "new_spent" , "new_vat", "new_total", "new_comment", "new_employee");

                queryExpense.Criteria.AddCondition("new_name", ConditionOperator.NotNull);
                queryExpense.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));
                queryExpense.Criteria.AddCondition("new_date", ConditionOperator.NotNull);
                queryExpense.Criteria.AddCondition("new_spent", ConditionOperator.NotNull);
                queryExpense.Criteria.AddCondition("new_vat", ConditionOperator.NotNull);
                queryExpense.Criteria.AddCondition("new_total", ConditionOperator.NotNull);
                queryExpense.Criteria.AddCondition("new_comment", ConditionOperator.NotNull);

                queryExpense.Criteria.AddCondition("new_employee", ConditionOperator.NotNull);

                
                EntityCollection expenseCollection = service.RetrieveMultiple(queryExpense);

                ////////////////////////////////////////////////////////////////////////////////////
                QueryExpression queryEmployee = new QueryExpression("new_employee");

                queryEmployee.ColumnSet.AddColumns("new_name", "statuscode", "new_employer");

                queryEmployee.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));
                queryEmployee.Criteria.AddCondition("new_name", ConditionOperator.NotNull);

                //queryEmployee.Criteria.AddCondition("new_employer", ConditionOperator.NotNull); // nebutinai.. nes mums reikia tiesiog paimti ... 

                EntityCollection employeeCollection = service.RetrieveMultiple(queryEmployee);

                //List<string> employeeDuplicates = new List<string>();

            
                
                foreach (Entity app in expenseCollection.Entities) // Visi projektai ... 
                {
                    Book book = new Book();
                    book.No = bookCount + 1;

                    // Get Employee
                    book.EmployeeName = ((EntityReference)app.Attributes["new_employee"]).Name; // Get Current Employee Name

                    // Get Employeer 
                    Guid EmployeeId = (Guid)((EntityReference)app.Attributes["new_employee"]).Id;
                    book.EmployeerName = employeeCollection.Entities.FirstOrDefault(x => x.Id == EmployeeId).GetAttributeValue<EntityReference>("new_employer").Name.ToString();



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
                Entity expenseEntity = new Entity("new_expense");
                
                expenseEntity["new_no"] = Convert.ToString(bookCount + 1);
                expenseEntity["new_name"] = book.Project;
                expenseEntity["new_date"] = book.Date;
                expenseEntity["new_spent"] = new Money((decimal)book.Spent);
                expenseEntity["new_vat"] = new Money((decimal)book.VAT);
                expenseEntity["new_total"] = new Money((decimal)book.Total);
                expenseEntity["new_comment"] = book.Comment;

                // Reference  Expence -> Employee 
                QueryExpression queryEmployee = new QueryExpression { EntityName = "new_employee", ColumnSet = new ColumnSet("new_name", "new_employeeid", "new_employer", "statuscode") }; 

                queryEmployee.Criteria.AddCondition("new_name", ConditionOperator.Equal, book.EmployeeName);
                queryEmployee.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));

                var EmployeeId = (Guid)service.RetrieveMultiple(queryEmployee)[0].Attributes["new_employeeid"]; 
                expenseEntity["new_employee"] = new EntityReference("new_employee", EmployeeId);

                // Reference Employee <- Employer cia reikia UPDATE 
                QueryExpression queryEmployer = new QueryExpression { EntityName = "new_employer", ColumnSet = new ColumnSet("new_name", "new_employerid", "statuscode") };

                queryEmployer.Criteria.AddCondition("new_name", ConditionOperator.Equal, book.EmployeerName);
                queryEmployer.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));

                Entity employeeEntity = new Entity("new_employee", EmployeeId); // get employee entity

                var EmployerId = (Guid)service.RetrieveMultiple(queryEmployer)[0].Attributes["new_employerid"]; // get id Employeer 
                employeeEntity["new_employer"] = new EntityReference("new_employer", EmployerId);

                //expenseEntity["new_employer"] = new EntityReference("new_employer", EmployerId);

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
        [System.Web.Http.HttpPut]
        public string Put(Book book)
        {
            if (!ModelState.IsValid)
            {
                return Convert.ToString(BadRequest("Not a valid model"));
            }
            try
            {
                var service = HelperClass.getCRMServie();

                // BOook obnowlennye --->> CRM PREZHNIE <<< --- Nuzhno obnowit  
                // Sdes tolko No. nie obnowilsia w Book ... poetomu berem wsie dannye po nomeru 

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

                //((EntityReference)expenseCollection.Entities.Attributes["new_employee"]).Name; 





                // Get Employee
                //*    book.EmployeeName = ((EntityReference)app.Attributes["new_employee"]).Name; // Get Current Employee Name

                // Get Employeer 
                /*  Guid EmployeeId = (Guid)((EntityReference)app.Attributes["new_employee"]).Id;
                  book.EmployeerName = employeeCollection.Entities.FirstOrDefault(x => x.Id == EmployeeId).GetAttributeValue<EntityReference>("new_employer").Name.ToString();*/


                /*         // Reference Employee <- Employer cia reikia UPDATE 
                         QueryExpression queryEmployer = new QueryExpression { EntityName = "new_employer", ColumnSet = new ColumnSet("new_name", "new_employerid", "statuscode") };

                         queryEmployer.Criteria.AddCondition("new_name", ConditionOperator.Equal, book.EmployeerName);
                         queryEmployer.Criteria.AddCondition("statuscode", ConditionOperator.Equal, (1));

                         Entity employeeEntity = new Entity("new_employee", EmployeeId); // get employee entity

                         var EmployerId = (Guid)service.RetrieveMultiple(queryEmployer)[0].Attributes["new_employerid"]; // get id Employeer 
                         employeeEntity["new_employer"] = new EntityReference("new_employer", EmployerId);
         *//*

                ////////////////////////////////////////////////////////////////////////////////////


                //queryEmployee.Criteria.AddCondition("new_employer", ConditionOperator.NotNull); // nebutinai.. nes mums reikia tiesiog paimti ... 

                EntityCollection employeeCollection = service.RetrieveMultiple(queryEmployee);
                ////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////

                // Get Employeer
                Guid EmployeeId = (Guid)((EntityReference)app.Attributes["new_employee"]).Id;
                book.EmployeerName = employeeCollection.Entities.FirstOrDefault(x => x.Id == EmployeeId).GetAttributeValue<EntityReference>("new_employer").Name.ToString();

                *//* var existingStudent = ctx.Students.Where(s => s.StudentID == student.Id)
                                                    .FirstOrDefault<Student>();*/



                return "Updated Successfully ! ";
            }
            catch (Exception ex)
            {
                return "Failed to Update: " + new ArgumentException(ex.Message);
            }
            
        }

        // DELETE api/values/5
        [System.Web.Http.HttpDelete]
        public void Delete(int id)
        {
        }
    }
}