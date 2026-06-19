using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http; // BẮT BUỘC THÊM DÒNG NÀY

namespace Lesson3_ORM_BookManagement.Models
{
    [Table("RoomImages_BIT240041")]
    public class RoomImage_BIT240041
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Đường dẫn ảnh không được để trống.")]
        [Display(Name = "Đường dẫn ảnh")]
        public string ImageUrl { get; set; }

        // --------- THÊM 2 DÒNG NÀY VÀO ---------
        [NotMapped] // Lệnh này giúp EF Core bỏ qua biến này, không tạo cột dưới Database
        public IFormFile ImageFile { get; set; }
        // ---------------------------------------

        [Display(Name = "Ảnh đại diện")]
        public bool IsThumbnail { get; set; }

        [Required]
        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Room_BIT240041 Room { get; set; }
    }
}