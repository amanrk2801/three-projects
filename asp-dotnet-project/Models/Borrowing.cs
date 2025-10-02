using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class Borrowing
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int BookId { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public BorrowingStatus Status { get; set; } = BorrowingStatus.Active;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? FineAmount { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;

        [ForeignKey("BookId")]
        public virtual Book Book { get; set; } = null!;

        // Computed properties
        public bool IsOverdue => Status == BorrowingStatus.Active && DateTime.UtcNow > DueDate;
        public int DaysOverdue => IsOverdue ? (DateTime.UtcNow - DueDate).Days : 0;
        public bool IsReturned => Status == BorrowingStatus.Returned;
    }

    public enum BorrowingStatus
    {
        Active,
        Returned,
        Lost,
        Damaged
    }
}