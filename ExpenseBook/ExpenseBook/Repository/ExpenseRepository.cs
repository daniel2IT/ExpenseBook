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
            Entity expense = new Entity("new_expense");

            expense["new_no"] = Convert.ToString(HelperClass.GetMaxNo(service) + 1);
            expense["new_name"] = postExpense.Project;
            expense["new_date"] = Convert.ToDateTime(postExpense.Date);
            expense["new_spent"] = new Money((decimal)postExpense.Spent);
            expense["new_vat"] = new Money((decimal)postExpense.VAT);
            expense["new_total"] = new Money((decimal)postExpense.Total);
            expense["new_comment"] = postExpense.Comment;

            expense["new_employee"] = new EntityReference("new_employee", postExpense.EmployeeId);

            return expense;
        }

        public Guid Delete(EntityCollection expenseCollection, string Id)
        {
            Guid expenceId = expenseCollection.Entities.FirstOrDefault(expenseNo => expenseNo.GetAttributeValue<string>("new_no").Contains(Convert.ToString(Id))).Id;
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
                employeeModel.EmployerRefId = employeeCollection.Entities.FirstOrDefault(employeeId => employeeId.Id == employeeModel.EmployeeId).GetAttributeValue<EntityReference>("new_employer").Id;
                workerList.Add(employeeModel);
            }
            return workerList;
        }

        public IEnumerable<Expense> GetExpense(EntityCollection expenseCollection, EntityCollection employeeCollection)
        {
            List<Expense> expenseList = new List<Expense>();

            foreach (Entity expense in expenseCollection.Entities)
            {
                Expense expenseModel = new Expense();

                expenseModel.No = Convert.ToInt32(expense.Attributes["new_no"]);

                // Get Employee Id&&Name
                expenseModel.EmployeeId  = (Guid)((EntityReference)expense.Attributes["new_employee"]).Id;
                expenseModel.EmployeeName = ((EntityReference)expense.Attributes["new_employee"]).Name;

                // Get Employer Id&&Name
                expenseModel.EmployerId = employeeCollection.Entities.FirstOrDefault(employee => employee.Id == expenseModel.EmployeeId).GetAttributeValue<EntityReference>("new_employer").Id;
                expenseModel.EmployerName = employeeCollection.Entities.FirstOrDefault(employee => employee.Id == expenseModel.EmployeeId).GetAttributeValue<EntityReference>("new_employer").Name.ToString();

                expenseModel.Project = expense.Attributes["new_name"].ToString();
                expenseModel.Date = Convert.ToDateTime(expense.Attributes["new_date"]).ToShortDateString();
                expenseModel.Spent = Convert.ToDecimal(expense.GetAttributeValue<Money>("new_spent").Value);
                expenseModel.VAT = Convert.ToDecimal(expense.GetAttributeValue<Money>("new_vat").Value);
                expenseModel.Total = Convert.ToDecimal(expense.GetAttributeValue<Money>("new_total").Value);
                expenseModel.Comment = expense.Attributes["new_comment"].ToString();

                expenseList.Add(expenseModel);
            }
            return expenseList;
        }

        public ExecuteMultipleRequest UpdateExpense(ExecuteMultipleRequest executeMultiple ,EntityCollection expenseCollection, Expense putExpense)
        {
            Guid expenceId = expenseCollection.Entities.FirstOrDefault(expenseNo => expenseNo.GetAttributeValue<string>("new_no").Contains(Convert.ToString(putExpense.No))).Id;

            Entity expense = new Entity("new_expense", expenceId);
            expense["new_name"] = putExpense.Project;
            expense["new_date"] = Convert.ToDateTime(putExpense.Date);
            expense["new_spent"] = new Money((decimal)putExpense.Spent);
            expense["new_vat"] = new Money((decimal)putExpense.VAT);
            expense["new_total"] = new Money((decimal)putExpense.Total);
            expense["new_comment"] = putExpense.Comment;

            Guid employeeNewId = putExpense.EmployeeId;

            expense["new_employee"] = new EntityReference("new_employee", employeeNewId);
            UpdateRequest reqUpdateExpense = new UpdateRequest { Target = expense };
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