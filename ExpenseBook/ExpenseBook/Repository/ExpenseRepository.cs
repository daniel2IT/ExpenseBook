using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
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

            Guid EmployeeId = employeeCollection.Entities[0].GetAttributeValue<Guid>("new_employeeid");
            expenseEntity["new_employee"] = new EntityReference("new_employee", EmployeeId);

            return expenseEntity;
        }

        public IEnumerable<Expense> GetExpense(EntityCollection expenseCollection, EntityCollection employeeCollection)
        {
            List<Expense> expenses = new List<Expense>();

            foreach (Entity app in expenseCollection.Entities)
            {
                Expense expense = new Expense();

                expense.No = Convert.ToInt32(app.Attributes["new_no"]);
                expense.EmployeeName = ((EntityReference)app.Attributes["new_employee"]).Name;

                // Get Employeer Name
                Guid EmployeeId = (Guid)((EntityReference)app.Attributes["new_employee"]).Id;
                expense.EmployeerName = employeeCollection.Entities.FirstOrDefault(x => x.Id == EmployeeId).GetAttributeValue<EntityReference>("new_employer").Name.ToString();

                expense.Project = app.Attributes["new_name"].ToString();
                expense.Date = Convert.ToDateTime(app.Attributes["new_date"]);
                expense.Spent = Convert.ToDecimal(app.GetAttributeValue<Money>("new_spent").Value);
                expense.VAT = Convert.ToDecimal(app.GetAttributeValue<Money>("new_vat").Value);
                expense.Total = Convert.ToDecimal(app.GetAttributeValue<Money>("new_total").Value);
                expense.Comment = app.Attributes["new_comment"].ToString();

                expenses.Add(expense);
            }

            return expenses;
        }


    }
}