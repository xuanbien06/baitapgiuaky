
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lesson3_ORM_BookManagement.Models;
using Lesson3_CNLTWeb.Data;

public class RoomTypesController : Controller
{
    private readonly AppDbContext _context;

    public RoomTypesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: ROOMTYPE_BIT240041S
    public async Task<IActionResult> Index()
    {
        // Thêm Include để load dữ liệu danh sách phòng kèm theo từng loại
        var roomTypes = await _context.RoomTypes
                                      .Include(r => r.Rooms)
                                      .ToListAsync();
        return View(roomTypes);
    }

    // GET: ROOMTYPE_BIT240041S/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var roomtype_bit240041 = await _context.RoomTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (roomtype_bit240041 == null)
        {
            return NotFound();
        }

        return View(roomtype_bit240041);
    }

    // GET: ROOMTYPE_BIT240041S/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ROOMTYPE_BIT240041S/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Rooms")] RoomType_BIT240041 roomtype_bit240041)
    {
        ModelState.Remove("Rooms");

        if (ModelState.IsValid)
        {
            _context.Add(roomtype_bit240041);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(roomtype_bit240041);
    }

    // GET: ROOMTYPE_BIT240041S/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var roomtype_bit240041 = await _context.RoomTypes.FindAsync(id);
        if (roomtype_bit240041 == null)
        {
            return NotFound();
        }
        return View(roomtype_bit240041);
    }

    // POST: ROOMTYPE_BIT240041S/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Rooms")] RoomType_BIT240041 roomtype_bit240041)
    {
        if (id != roomtype_bit240041.Id)
        {
            return NotFound();
        }

        ModelState.Remove("Rooms");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(roomtype_bit240041);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.RoomTypes.Any(r => r.Id == roomtype_bit240041.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(roomtype_bit240041);
    }

    // 1. HÀM GET: CHẶN NGAY LÚC VỪA BẤM NÚT XÓA Ở DANH SÁCH
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var roomtype_bit240041 = await _context.RoomTypes.FirstOrDefaultAsync(m => m.Id == id);
        if (roomtype_bit240041 == null) return NotFound();

        // Lắp cảm biến kiểm tra vào hàm GET
        bool isUsedByRooms = await _context.Rooms.AnyAsync(r => r.RoomTypeId == id);
        if (isUsedByRooms)
        {
            ViewBag.ErrorMessage = $"Không thể xóa loại phòng '{roomtype_bit240041.Name}' vì đang có phòng trọ thuộc loại này. Vui lòng chuyển loại phòng hoặc xóa các phòng đó trước!";
        }

        return View(roomtype_bit240041);
    }

    // 2. HÀM POST: CHẶN LỚP THỨ 2 ĐỂ BẢO VỆ DATABASE
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var roomtype_bit240041 = await _context.RoomTypes.FindAsync(id);
        if (roomtype_bit240041 == null) return NotFound();

        // Lắp cảm biến kiểm tra vào hàm POST
        bool isUsedByRooms = await _context.Rooms.AnyAsync(r => r.RoomTypeId == id);
        if (isUsedByRooms)
        {
            ViewBag.ErrorMessage = $"Không thể xóa loại phòng '{roomtype_bit240041.Name}' vì đang có phòng trọ thuộc loại này. Vui lòng chuyển loại phòng hoặc xóa các phòng đó trước!";
            return View(roomtype_bit240041);
        }

        _context.RoomTypes.Remove(roomtype_bit240041);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
