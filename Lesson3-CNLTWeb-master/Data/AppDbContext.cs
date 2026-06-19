using Lesson3_CNLTWeb.Models;
using Lesson3_ORM_BookManagement.Models; // Dòng này VS tự sinh ra khi em ấn Ctrl + .
using Microsoft.EntityFrameworkCore;
// using ... (có thể có thêm using cho Book nếu cần)

namespace Lesson3_CNLTWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

       

        // 3 Bảng của bài thi giữa kỳ (phải using đúng thư mục Models để hết 3 lỗi)
        public DbSet<RoomType_BIT240041> RoomTypes { get; set; }
        public DbSet<Room_BIT240041> Rooms { get; set; }
        public DbSet<RoomImage_BIT240041> RoomImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Bắt buộc phải có dòng này để giữ cấu trúc của các thiết lập khác
            base.OnModelCreating(modelBuilder);

            // 1. Seed 3 RoomType
            modelBuilder.Entity<RoomType_BIT240041>().HasData(
                new RoomType_BIT240041 { Id = 1, Name = "Phòng Trọ Sinh Viên" },
                new RoomType_BIT240041 { Id = 2, Name = "Chung Cư Mini" },
                new RoomType_BIT240041 { Id = 3, Name = "Phòng Studio Cao Cấp" }
            );

            // 2. Seed 5 Room (Không trùng tên trong cùng Loại, đúng điều kiện Price > 0, Area > 0)
            modelBuilder.Entity<Room_BIT240041>().HasData(
                new Room_BIT240041 { Id = 1, Name = "Phòng 101", Price = 1500000, Area = 20.5, IsAvailable = true, RoomTypeId = 1 },
                new Room_BIT240041 { Id = 2, Name = "Phòng 102", Price = 1800000, Area = 25.0, IsAvailable = false, RoomTypeId = 1 },
                new Room_BIT240041 { Id = 3, Name = "Phòng 201", Price = 3500000, Area = 35.0, IsAvailable = true, RoomTypeId = 2 },
                new Room_BIT240041 { Id = 4, Name = "Phòng 202", Price = 3800000, Area = 40.0, IsAvailable = true, RoomTypeId = 2 },
                new Room_BIT240041 { Id = 5, Name = "Phòng VIP 1", Price = 5500000, Area = 55.5, IsAvailable = true, RoomTypeId = 3 }
            );

            // 3. Seed RoomImage (Mỗi phòng ít nhất 2 ảnh, chỉ có 1 ảnh IsThumbnail = true)
            modelBuilder.Entity<RoomImage_BIT240041>().HasData(
                new RoomImage_BIT240041 { Id = 1, RoomId = 1, ImageUrl = "https://dummyimage.com/600x400/000/fff&text=Room101-1", IsThumbnail = true },
                new RoomImage_BIT240041 { Id = 2, RoomId = 1, ImageUrl = "https://dummyimage.com/600x400/000/fff&text=Room101-2", IsThumbnail = false },

                new RoomImage_BIT240041 { Id = 3, RoomId = 2, ImageUrl = "https://dummyimage.com/600x400/000/fff&text=Room102-1", IsThumbnail = true },
                new RoomImage_BIT240041 { Id = 4, RoomId = 2, ImageUrl = "https://dummyimage.com/600x400/000/fff&text=Room102-2", IsThumbnail = false },

                new RoomImage_BIT240041 { Id = 5, RoomId = 3, ImageUrl = "https://dummyimage.com/600x400/000/fff&text=Room201-1", IsThumbnail = true },
                new RoomImage_BIT240041 { Id = 6, RoomId = 3, ImageUrl = "https://dummyimage.com/600x400/000/fff&text=Room201-2", IsThumbnail = false },

                new RoomImage_BIT240041 { Id = 7, RoomId = 4, ImageUrl = "https://dummyimage.com/600x400/000/fff&text=Room202-1", IsThumbnail = true },
                new RoomImage_BIT240041 { Id = 8, RoomId = 4, ImageUrl = "https://dummyimage.com/600x400/000/fff&text=Room202-2", IsThumbnail = false },

                new RoomImage_BIT240041 { Id = 9, RoomId = 5, ImageUrl = "https://dummyimage.com/600x400/000/fff&text=VIP1-1", IsThumbnail = true },
                new RoomImage_BIT240041 { Id = 10, RoomId = 5, ImageUrl = "https://dummyimage.com/600x400/000/fff&text=VIP1-2", IsThumbnail = false }
            );
        }
    }
}