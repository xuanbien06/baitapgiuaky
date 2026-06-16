using Lesson3_CNLTWeb.Models;
using System.Collections.Generic;

namespace Lesson3_CNLTWeb.Data
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAll();
        Book GetById(int id);
        void Add(Book book);
        void Update(Book book);
        void Delete(int id);
    }
}