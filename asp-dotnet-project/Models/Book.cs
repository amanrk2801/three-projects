using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string ISBN { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Genre { get; set; }

        [StringLength(100)]
        public string? Publisher { get; set; }

        public DateTime? PublishedDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Total copies must be at least 1")]
        public int TotalCopies { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Available copies cannot be negative")]
        public int AvailableCopies { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Price { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

        // Computed properties
        public bool IsAvailable => AvailableCopies > 0;
        public int BorrowedCopies => TotalCopies - AvailableCopies;
    }
}