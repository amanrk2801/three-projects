using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string? Genre { get; set; }
        public string? Publisher { get; set; }
        public DateTime? PublishedDate { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAvailable { get; set; }
        public int BorrowedCopies { get; set; }
    }

    public class CreateBookDto
    {
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

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Total copies must be at least 1")]
        public int TotalCopies { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Available copies cannot be negative")]
        public int AvailableCopies { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative")]
        public decimal? Price { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }
    }

    public class UpdateBookDto
    {
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(100)]
        public string? Author { get; set; }

        [StringLength(20)]
        public string? ISBN { get; set; }

        [StringLength(50)]
        public string? Genre { get; set; }

        [StringLength(100)]
        public string? Publisher { get; set; }

        public DateTime? PublishedDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Total copies must be at least 1")]
        public int? TotalCopies { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Available copies cannot be negative")]
        public int? AvailableCopies { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative")]
        public decimal? Price { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public bool? IsActive { get; set; }
    }

    public class BookSearchDto
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? ISBN { get; set; }
        public bool? IsAvailable { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Title";
        public string SortOrder { get; set; } = "asc";
    }
}