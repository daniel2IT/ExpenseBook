using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System.Collections.Generic;

namespace ExpenseBook.Repository
{
    public interface IExpenseRepository
    {
        IEnumerable<Expense> GetExpense(EntityCollection expenseCollection, EntityCollection employeeCollection);
      
        Entity CreateExpense(Expense postExpense, EntityCollection employeeCollection, CrmServiceClient service);
    }
}