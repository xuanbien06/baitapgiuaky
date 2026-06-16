using Microsoft.AspNetCore.Mvc;
using Lesson3_CNLTWeb.Models;
using Lesson3_CNLTWeb.Data;
using System.Linq;

namespace Lesson3_CNLTWeb.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;

        // Tiêm IBookRepository vào Controller
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // Xử lý Tìm kiếm, Sắp xếp và Xem danh sách
        public IActionResult Index(string searchString, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.PriceSortParam = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.CurrentFilter = searchString;

            // 1. Lấy toàn bộ dữ liệu
            var books = _bookRepository.GetAll();

            // 2. TÌM KIẾM bằng LINQ
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(s => s.Title.Contains(searchString) || s.Author.Contains(searchString));
            }

            // 3. SẮP XẾP bằng LINQ
            switch (sortOrder)
            {
                case "price_desc":
                    books = books.OrderByDescending(s => s.Price); // Giá giảm dần
                    break;
                default:
                    books = books.OrderBy(s => s.Price); // Mặc định: Giá tăng dần
                    break;
            }

            return View(books.ToList());
        }

        // GET: Hiển thị form Thêm
        public IActionResult Create()
        {
            return View();
        }

        // POST: Xử lý lưu Thêm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.Add(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Hiển thị form Sửa
        public IActionResult Edit(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        // POST: Xử lý lưu Sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book book)
        {
            if (id != book.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _bookRepository.Update(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Hiển thị giao diện xác nhận Xóa
        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null) return NotFound();
            return View(book);
        }

        // POST: Thực thi Xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _bookRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Xem chi tiết
        public IActionResult Detail(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null) return NotFound();
            return View(book);
        }
    }
}