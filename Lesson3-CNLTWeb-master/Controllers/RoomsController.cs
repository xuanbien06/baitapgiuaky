using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lesson3_ORM_BookManagement.Models;
using Lesson3_CNLTWeb.Data;
using Microsoft.AspNetCore.Mvc.Rendering; // BẮT BUỘC PHẢI CÓ THƯ VIỆN NÀY ĐỂ DÙNG SELECTLIST

namespace Lesson3_CNLTWeb.Controllers
{
    public class RoomsController : Controller
    {
        private readonly AppDbContext _context;

        public RoomsController(AppDbContext context)
        {
            _context = context;
        }



        // GET: ROOMS
        public async Task<IActionResult> Index(string searchString, int? roomTypeId, bool? isAvailable, decimal? maxPrice, string sortOrder)
        {
            // 1. LƯU LẠI GIÁ TRỊ TÌM KIẾM ĐỂ HIỂN THỊ LẠI TRÊN GIAO DIỆN
            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentMaxPrice"] = maxPrice;

            // Chuẩn bị SelectList cho Loại phòng, Trạng thái và Sắp xếp
            ViewData["RoomTypes"] = new SelectList(_context.RoomTypes, "Id", "Name", roomTypeId);

            var availableList = new List<SelectListItem> {
        new SelectListItem { Text = "Còn phòng", Value = "true" },
        new SelectListItem { Text = "Đã thuê", Value = "false" }
    };
            ViewData["IsAvailableList"] = new SelectList(availableList, "Value", "Text", isAvailable?.ToString().ToLower());

            var sortList = new List<SelectListItem> {
        new SelectListItem { Text = "Giá tăng dần", Value = "price_asc" },
        new SelectListItem { Text = "Giá giảm dần", Value = "price_desc" },
        new SelectListItem { Text = "Diện tích giảm dần", Value = "area_desc" }
    };
            ViewData["SortList"] = new SelectList(sortList, "Value", "Text", sortOrder);

            // 2. BẮT ĐẦU LINQ QUERY (Dùng IQueryable để tối ưu tốc độ, chưa truy vấn DB ngay)
            var rooms = _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .AsQueryable();

            // 3. XỬ LÝ LỌC (FILTERING)
            if (!string.IsNullOrEmpty(searchString))
            {
                rooms = rooms.Where(r => r.Name.Contains(searchString));
            }

            if (roomTypeId.HasValue)
            {
                rooms = rooms.Where(r => r.RoomTypeId == roomTypeId.Value);
            }

            if (isAvailable.HasValue)
            {
                rooms = rooms.Where(r => r.IsAvailable == isAvailable.Value);
            }

            if (maxPrice.HasValue)
            {
                rooms = rooms.Where(r => r.Price <= maxPrice.Value);
            }

            // 4. XỬ LÝ SẮP XẾP (SORTING)
            switch (sortOrder)
            {
                case "price_asc":
                    rooms = rooms.OrderBy(r => r.Price);
                    break;
                case "price_desc":
                    rooms = rooms.OrderByDescending(r => r.Price);
                    break;
                case "area_desc":
                    rooms = rooms.OrderByDescending(r => r.Area);
                    break;
                default:
                    rooms = rooms.OrderByDescending(r => r.Id); // Mặc định hiển thị phòng mới nhất
                    break;
            }

            // 5. THỰC THI TRUY VẤN VÀ TRẢ VỀ VIEW
            return View(await rooms.ToListAsync());
        }

        // 2. GET: DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .Include(r => r.RoomImages)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (room == null) return NotFound();

            return View(room);
        }

        // 3. GET: CREATE
        public IActionResult Create()
        {
            // SỬA Ở ĐÂY: Gói dữ liệu vào SelectList (Id làm giá trị, Name để hiển thị)
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "Id", "Name");
            return View();
        }

        // 4. POST: CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Area,IsAvailable,RoomTypeId")] Room_BIT240041 room)
        {

            ModelState.Remove("RoomType");
            ModelState.Remove("RoomImages");

            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // SỬA Ở ĐÂY
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "Id", "Name", room.RoomTypeId);
            return View(room);
        }

        // 5. GET: EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();

            // SỬA Ở ĐÂY
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "Id", "Name", room.RoomTypeId);
            return View(room);
        }

        // 6. POST: EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Area,IsAvailable,RoomTypeId")] Room_BIT240041 room)
        {
            if (id != room.Id) return NotFound();

            ModelState.Remove("RoomType");
            ModelState.Remove("RoomImages");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            // SỬA Ở ĐÂY
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "Id", "Name", room.RoomTypeId);
            return View(room);
        }

        // 7. GET: DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var room = await _context.Rooms
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (room == null) return NotFound();

            return View(room);
        }

        // 8. POST: DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}