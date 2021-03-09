using ExpenseBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpenseBook.Interfaces
{
    public interface IBookRepository
    {
        void Add(Book item);
        IEnumerable<Book> GetAll();
        Book Remove(string key);
        void Update(Book item);
    }
}