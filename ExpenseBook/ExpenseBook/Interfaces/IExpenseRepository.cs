using ExpenseBook.Models;
using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace ExpenseBook.Repository
{
    public interface IExpenseRepository
    {
        IEnumerable<Expense> GetExpense(EntityCollection expenseCollection, EntityCollection employeeCollection);
    }
}