using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;

namespace ExpenseBook.Repository
{
    public interface IExpenseRepository
    {
        IEnumerable<Expense> GetExpense(EntityCollection expenseCollection, EntityCollection employeeCollection);
        IEnumerable<Worker> GetWorker(EntityCollection employerCollection, EntityCollection employeeCollection);
        Entity CreateExpense(Expense postExpense, EntityCollection employeeCollection, CrmServiceClient service);
        ExecuteMultipleRequest UpdateExpense(ExecuteMultipleRequest executeMultiple, EntityCollection expenseCollection, Expense putExpense);
        Guid Delete(EntityCollection expenseCollection, string s);
    }
}