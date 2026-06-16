using Lesson3_CNLTWeb.Models;
using System.Collections.Generic;
using System.Linq;

namespace Lesson3_CNLTWeb.Data
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        // Tiêm DbContext vào Repository
        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả sách từ Database thay vì ArrayList
        public IEnumerable<Book> GetAll()
        {
            return _context.Books.ToList();
        }

        // Tìm sách theo ID
        public Book GetById(int id)
        {
            return _context.Books.FirstOrDefault(b => b.Id == id);
        }

        // Thêm sách mới vào Database
        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        // Cập nhật sách
        public void Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }

        // Xóa sách
        public void Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }
    }
}