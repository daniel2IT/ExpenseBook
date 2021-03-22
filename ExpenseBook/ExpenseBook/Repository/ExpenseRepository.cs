using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseBook.Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        public Entity CreateExpense(Expense postExpense, EntityCollection employeeCollection, CrmServiceClient service)
        {
            Entity expenseEntity = new Entity("new_expense");

            expenseEntity["new_no"] = Convert.ToString(HelperClass.GetMaxNo(service) + 1);
            expenseEntity["new_name"] = postExpense.Project;
            expenseEntity["new_date"] = postExpense.Date;
            expenseEntity["new_spent"] = new Money((decimal)postExpense.Spent);
            expenseEntity["new_vat"] = new Money((decimal)postExpense.VAT);
            expenseEntity["new_total"] = new Money((decimal)postExpense.Total);
            expenseEntity["new_comment"] = postExpense.Comment;

            expenseEntity["new_employee"] = new EntityReference("new_employee", postExpense.EmployeeId);

            return expenseEntity;
        }

        public Guid Delete(EntityCollection expenseCollection, string Id)
        {
            Guid expenceId = expenseCollection.Entities.FirstOrDefault(expense => expense.GetAttributeValue<string>("new_no").Contains(Convert.ToString(Id))).Id;

            return expenceId;
        }

        public IEnumerable<Worker> GetWorker(EntityCollection employerCollection, EntityCollection employeeCollection)
        {
            List<Worker> workerList = new List<Worker>();

            foreach (Entity employer in employerCollection.Entities)
            {
                Worker employerModel = new Worker();

                employerModel.EmployerId = employer.Id;
                employerModel.EmployerName = employer.Attributes["new_name"].ToString();

                workerList.Add(employerModel);
            }

            // Get Employee Section
            foreach (Entity employee in employeeCollection.Entities)
            {
                Worker employeeModel = new Worker();
                employeeModel.EmployeeName = employee.Attributes["new_name"].ToString();

                // Get Employee ID
                employeeModel.EmployeeId = (Guid)employee.Attributes["new_employeeid"];

                // Get Employer ID
                employeeModel.EmployerRefId = employeeCollection.Entities.FirstOrDefault(entity => entity.Id == employeeModel.EmployeeId).GetAttributeValue<EntityReference>("new_employer").Id;
                workerList.Add(employeeModel);
            }


            return workerList;
        }

        public IEnumerable<Expense> GetExpense(EntityCollection expenseCollection, EntityCollection employeeCollection)
        {
            List<Expense> expenses = new List<Expense>();

            foreach (Entity app in expenseCollection.Entities)
            {
                Expense expense = new Expense();

                expense.No = Convert.ToInt32(app.Attributes["new_no"]);

                // Get Employee Id&&Name
                expense.EmployeeId  = (Guid)((EntityReference)app.Attributes["new_employee"]).Id;
                expense.EmployeeName = ((EntityReference)app.Attributes["new_employee"]).Name;

                // Get Employer Id&&Name
                expense.EmployerId = employeeCollection.Entities.FirstOrDefault(employee => employee.Id == expense.EmployeeId).GetAttributeValue<EntityReference>("new_employer").Id;
                expense.EmployerName = employeeCollection.Entities.FirstOrDefault(employee => employee.Id == expense.EmployeeId).GetAttributeValue<EntityReference>("new_employer").Name.ToString();
                
                expense.Project = app.Attributes["new_name"].ToString();
                expense.Date = Convert.ToDateTime(app.Attributes["new_date"]).ToShortDateString();
                expense.Spent = Convert.ToDecimal(app.GetAttributeValue<Money>("new_spent").Value);
                expense.VAT = Convert.ToDecimal(app.GetAttributeValue<Money>("new_vat").Value);
                expense.Total = Convert.ToDecimal(app.GetAttributeValue<Money>("new_total").Value);
                expense.Comment = app.Attributes["new_comment"].ToString();

                expenses.Add(expense);
            }

            return expenses;
        }

        public ExecuteMultipleRequest UpdateExpense(ExecuteMultipleRequest executeMultiple ,EntityCollection expenseCollection, Expense putExpense)
        {
            Guid expenceId = expenseCollection.Entities.FirstOrDefault(expense => expense.GetAttributeValue<string>("new_no").Contains(Convert.ToString(putExpense.No))).Id;

            Entity expenseEntity = new Entity("new_expense", expenceId);
            expenseEntity["new_name"] = putExpense.Project;
            expenseEntity["new_date"] = putExpense.Date;
            expenseEntity["new_spent"] = new Money((decimal)putExpense.Spent);
            expenseEntity["new_vat"] = new Money((decimal)putExpense.VAT);
            expenseEntity["new_total"] = new Money((decimal)putExpense.Total);
            expenseEntity["new_comment"] = putExpense.Comment;

            Guid employeeNewId = putExpense.EmployeeId;

            expenseEntity["new_employee"] = new EntityReference("new_employee", employeeNewId);
            UpdateRequest reqUpdateExpense = new UpdateRequest { Target = expenseEntity };
            executeMultiple.Requests.Add(reqUpdateExpense);

            Guid employerId = putExpense.EmployerId;

            Entity employeeEntity = new Entity("new_employee", employeeNewId);
            employeeEntity["new_employer"] = new EntityReference("new_employer", employerId);
            UpdateRequest reqUpdateEmployee = new UpdateRequest { Target = employeeEntity };

            executeMultiple.Requests.Add(reqUpdateEmployee);

            return executeMultiple;
        }
    }
}