using Lesson3_CNLTWeb.Data;
using Lesson3_CNLTWeb.Models;
using Lesson3_ORM_BookManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Lesson3_CNLTWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        // Tiêm AppDbContext vào HomeController
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Đếm tổng số Loại phòng
            ViewBag.TotalRoomTypes = await _context.RoomTypes.CountAsync();

            // 2. Đếm tổng số Phòng trọ
            ViewBag.TotalRooms = await _context.Rooms.CountAsync();

            // 3. Đếm số phòng còn trống (IsAvailable == true)
            ViewBag.AvailableRooms = await _context.Rooms.CountAsync(r => r.IsAvailable == true);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode == 404)
            {
                ViewBag.ErrorMessage = "Trang bạn tìm kiếm không tồn tại, hoặc dữ liệu này đã bị xóa khỏi hệ thống!";
                ViewBag.StatusCode = "404";
            }
            else
            {
                ViewBag.ErrorMessage = "Đã xảy ra sự cố từ phía máy chủ. Vui lòng thử lại sau!";
                ViewBag.StatusCode = statusCode?.ToString() ?? "500";
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}