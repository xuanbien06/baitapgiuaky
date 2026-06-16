using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson3_CNLTWeb.Models
{
    [Table("Book")] // Ép EF Core tìm đúng bảng tên là Book trong SQL Server
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Tác giả không được để trống")]
        public string? Author { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        public decimal Price { get; set; }
    }
}