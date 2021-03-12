using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ExpenseBook.Controllers
{
    public class BooksController : ApiController
    {
        // GET: Books
        [HttpGet]
        public List<Book> Get()
        {
            try
            {
                List<Book> books = new List<Book>();
                CrmServiceClient service = HelperClass.getCRMServie();

                // Get  Expenses 
                EntityCollection expenseCollection = HelperClass.Query(service, "new_expense", "", "");
                
                // Get Employee
                EntityCollection employeeCollection = HelperClass.Query(service, "new_employee", "", "");

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
                EntityCollection employeeCollection = HelperClass.Query(service, "new_employee", book.EmployeeName, "");
                var EmployeeId = employeeCollection.Entities[0].GetAttributeValue<Guid>("new_employeeid");
                expenseEntity["new_employee"] = new EntityReference("new_employee", EmployeeId);

                // Get Employeer set -> (reference)
                EntityCollection employerCollection =  HelperClass.Query(service, "new_employer", book.EmployeerName, "");
                Entity employeeEntity = new Entity("new_employee", EmployeeId); // get employee entity
                Guid EmployerId = employerCollection.Entities[0].GetAttributeValue<Guid>("new_employerid");
                employeeEntity["new_employer"] = new EntityReference("new_employer", EmployerId);

                service.Create(expenseEntity);
                service.Update(employeeEntity);
                

                return "Added Successfully ! ";
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

                // Get Expense
                EntityCollection expenseCollection = HelperClass.Query(service, "new_expense", "new_expenseid", Convert.ToString(book.No));
                Guid expenceId = expenseCollection.Entities[0].GetAttributeValue<Guid>("new_expenseid");

                Entity expenseEntity = new Entity("new_expense", expenceId);
                expenseEntity["new_name"] = book.Project;
                expenseEntity["new_date"] = book.Date;
                expenseEntity["new_spent"] = new Money((decimal)book.Spent);
                expenseEntity["new_vat"] = new Money((decimal)book.VAT);
                expenseEntity["new_total"] = new Money((decimal)book.Total);
                expenseEntity["new_comment"] = book.Comment;
                
                // Get Employee && Update Expense
                EntityCollection employeeCollection = HelperClass.Query(service, "new_employee", "new_employeeid", book.EmployeeName);
                Guid employeeNewId = employeeCollection.Entities[0].GetAttributeValue<Guid>("new_employeeid");

                expenseEntity["new_employee"] = new EntityReference("new_employee", employeeNewId); // Updated Expense LookUp
                service.Update(expenseEntity);

                // Get Employer && Update Employee
                EntityCollection employerCollection = HelperClass.Query(service, "new_employer", "new_employerid", book.EmployeerName);
                Guid employerId = employerCollection.Entities[0].GetAttributeValue<Guid>("new_employerid");

                Entity employeeEntity = new Entity("new_employee", employeeNewId);
                employeeEntity["new_employer"] = new EntityReference("new_employer", employerId);
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