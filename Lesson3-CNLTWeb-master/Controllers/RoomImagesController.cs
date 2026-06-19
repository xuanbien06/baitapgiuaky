using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lesson3_ORM_BookManagement.Models;
using Lesson3_CNLTWeb.Data;
using Microsoft.AspNetCore.Hosting; // Thư viện xử lý môi trường đường dẫn

namespace Lesson3_CNLTWeb.Controllers
{
    public class RoomImagesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // Bổ sung biến này

        // Cập nhật hàm khởi tạo (Constructor)
        public RoomImagesController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: INDEX (Giữ nguyên)
        public async Task<IActionResult> Index(int? roomId)
        {
            if (roomId == null) return RedirectToAction("Index", "Rooms");
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return NotFound();

            ViewBag.RoomId = room.Id;
            ViewBag.RoomName = room.Name;

            var images = await _context.RoomImages
                .Include(r => r.Room)
                .Where(r => r.RoomId == roomId)
                .ToListAsync();

            return View(images);
        }

        // GET: CREATE (Giữ nguyên)
        public IActionResult Create(int roomId)
        {
            var room = _context.Rooms.Find(roomId);
            ViewBag.RoomName = room?.Name;

            var model = new RoomImage_BIT240041 { RoomId = roomId };
            return View(model);
        }

        // POST: CREATE - XỬ LÝ UPLOAD FILE Ở ĐÂY
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IsThumbnail,RoomId,ImageFile")] RoomImage_BIT240041 roomImage)
        {
            // Bỏ qua lỗi bắt buộc nhập của các biến này vì ta sẽ tự xử lý
            ModelState.Remove("Room");
            ModelState.Remove("ImageUrl");

            if (ModelState.IsValid)
            {
                // 1. Nếu có file được chọn gửi lên
                if (roomImage.ImageFile != null)
                {
                    // Tạo đường dẫn đến thư mục wwwroot/uploads/rooms
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "rooms");

                    // Nếu thư mục chưa có thì tự động tạo mới
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    // Đổi tên file để không bị trùng (Thêm chuỗi mã hóa Guid lên trước tên)
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + roomImage.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Copy file từ form vào thư mục của hệ thống
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await roomImage.ImageFile.CopyToAsync(fileStream);
                    }

                    // Gán chuỗi URL cho Database (ví dụ: /uploads/rooms/anh1.jpg)
                    roomImage.ImageUrl = "/uploads/rooms/" + uniqueFileName;
                }
                else
                {
                    // Nếu không chọn file mà cứ bấm lưu
                    ModelState.AddModelError("ImageFile", "Vui lòng chọn một file ảnh từ máy.");
                    var room = _context.Rooms.Find(roomImage.RoomId);
                    ViewBag.RoomName = room?.Name;
                    return View(roomImage);
                }

                // 2. LOGIC THUMBNAIL (Như cũ)
                if (roomImage.IsThumbnail)
                {
                    var oldImages = await _context.RoomImages.Where(i => i.RoomId == roomImage.RoomId).ToListAsync();
                    foreach (var img in oldImages)
                    {
                        img.IsThumbnail = false;
                        _context.Update(img);
                    }
                }

                // 3. Lưu dữ liệu
                _context.Add(roomImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { roomId = roomImage.RoomId });
            }

            return View(roomImage);
        }

        // POST: SET THUMBNAIL (Giữ nguyên)
        [HttpPost]
        public async Task<IActionResult> SetThumbnail(int id, int roomId)
        {
            var images = await _context.RoomImages.Where(i => i.RoomId == roomId).ToListAsync();
            foreach (var img in images)
            {
                img.IsThumbnail = (img.Id == id);
                _context.Update(img);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { roomId = roomId });
        }

        // POST: DELETE (Giữ nguyên)
        [HttpPost]
        public async Task<IActionResult> Delete(int id, int roomId)
        {
            var roomImage = await _context.RoomImages.FindAsync(id);
            if (roomImage != null)
            {
                _context.RoomImages.Remove(roomImage);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), new { roomId = roomId });
        }
    }
}