using LibraryManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOs
{
    public class BorrowingDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookAuthor { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BorrowingStatus Status { get; set; }
        public decimal? FineAmount { get; set; }
        public string? Notes { get; set; }
        public bool IsOverdue { get; set; }
        public int DaysOverdue { get; set; }
        public bool IsReturned { get; set; }
    }

    public class CreateBorrowingDto
    {
        [Required]
        public int BookId { get; set; }

        public DateTime? DueDate { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class ReturnBookDto
    {
        [Required]
        public int BorrowingId { get; set; }

        public BorrowingStatus Status { get; set; } = BorrowingStatus.Returned;

        [Range(0, double.MaxValue, ErrorMessage = "Fine amount cannot be negative")]
        public decimal? FineAmount { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class BorrowingSearchDto
    {
        public string? UserId { get; set; }
        public int? BookId { get; set; }
        public BorrowingStatus? Status { get; set; }
        public DateTime? BorrowDateFrom { get; set; }
        public DateTime? BorrowDateTo { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public bool? IsOverdue { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "BorrowDate";
        public string SortOrder { get; set; } = "desc";
    }
}