using AutoMapper;
using LibraryManagement.Data;
using LibraryManagement.DTOs;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class BorrowingService : IBorrowingService
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;
        private const decimal DailyFineRate = 0.50m; // $0.50 per day
        private const int DefaultLoanPeriodDays = 14; // 2 weeks

        public BorrowingService(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<BorrowingDto> Borrowings, int TotalCount)> GetBorrowingsAsync(BorrowingSearchDto searchDto)
        {
            var query = _context.Borrowings
                .Include(b => b.User)
                .Include(b => b.Book)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchDto.UserId))
            {
                query = query.Where(b => b.UserId == searchDto.UserId);
            }

            if (searchDto.BookId.HasValue)
            {
                query = query.Where(b => b.BookId == searchDto.BookId.Value);
            }

            if (searchDto.Status.HasValue)
            {
                query = query.Where(b => b.Status == searchDto.Status.Value);
            }

            if (searchDto.BorrowDateFrom.HasValue)
            {
                query = query.Where(b => b.BorrowDate >= searchDto.BorrowDateFrom.Value);
            }

            if (searchDto.BorrowDateTo.HasValue)
            {
                query = query.Where(b => b.BorrowDate <= searchDto.BorrowDateTo.Value);
            }

            if (searchDto.DueDateFrom.HasValue)
            {
                query = query.Where(b => b.DueDate >= searchDto.DueDateFrom.Value);
            }

            if (searchDto.DueDateTo.HasValue)
            {
                query = query.Where(b => b.DueDate <= searchDto.DueDateTo.Value);
            }

            if (searchDto.IsOverdue.HasValue && searchDto.IsOverdue.Value)
            {
                query = query.Where(b => b.Status == BorrowingStatus.Active && b.DueDate < DateTime.UtcNow);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = searchDto.SortBy.ToLower() switch
            {
                "duedate" => searchDto.SortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(b => b.DueDate)
                    : query.OrderBy(b => b.DueDate),
                "returndate" => searchDto.SortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(b => b.ReturnDate)
                    : query.OrderBy(b => b.ReturnDate),
                "status" => searchDto.SortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(b => b.Status)
                    : query.OrderBy(b => b.Status),
                _ => searchDto.SortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(b => b.BorrowDate)
                    : query.OrderBy(b => b.BorrowDate)
            };

            // Apply pagination
            var borrowings = await query
                .Skip((searchDto.Page - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();

            var borrowingDtos = borrowings.Select(b => new BorrowingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                UserName = b.User.FullName,
                BookId = b.BookId,
                BookTitle = b.Book.Title,
                BookAuthor = b.Book.Author,
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate,
                ReturnDate = b.ReturnDate,
                Status = b.Status,
                FineAmount = b.FineAmount,
                Notes = b.Notes,
                IsOverdue = b.IsOverdue,
                DaysOverdue = b.DaysOverdue,
                IsReturned = b.IsReturned
            });

            return (borrowingDtos, totalCount);
        }

        public async Task<BorrowingDto?> GetBorrowingByIdAsync(int id)
        {
            var borrowing = await _context.Borrowings
                .Include(b => b.User)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (borrowing == null)
                return null;

            return new BorrowingDto
            {
                Id = borrowing.Id,
                UserId = borrowing.UserId,
                UserName = borrowing.User.FullName,
                BookId = borrowing.BookId,
                BookTitle = borrowing.Book.Title,
                BookAuthor = borrowing.Book.Author,
                BorrowDate = borrowing.BorrowDate,
                DueDate = borrowing.DueDate,
                ReturnDate = borrowing.ReturnDate,
                Status = borrowing.Status,
                FineAmount = borrowing.FineAmount,
                Notes = borrowing.Notes,
                IsOverdue = borrowing.IsOverdue,
                DaysOverdue = borrowing.DaysOverdue,
                IsReturned = borrowing.IsReturned
            };
        }

        public async Task<IEnumerable<BorrowingDto>> GetUserBorrowingsAsync(string userId)
        {
            var borrowings = await _context.Borrowings
                .Include(b => b.Book)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BorrowDate)
                .ToListAsync();

            return borrowings.Select(b => new BorrowingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                UserName = b.User?.FullName ?? "",
                BookId = b.BookId,
                BookTitle = b.Book.Title,
                BookAuthor = b.Book.Author,
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate,
                ReturnDate = b.ReturnDate,
                Status = b.Status,
                FineAmount = b.FineAmount,
                Notes = b.Notes,
                IsOverdue = b.IsOverdue,
                DaysOverdue = b.DaysOverdue,
                IsReturned = b.IsReturned
            });
        }

        public async Task<BorrowingDto?> BorrowBookAsync(string userId, CreateBorrowingDto createBorrowingDto)
        {
            // Check if book exists and is available
            var book = await _context.Books.FindAsync(createBorrowingDto.BookId);
            if (book == null || !book.IsActive || book.AvailableCopies <= 0)
                return null;

            // Check if user already has this book borrowed
            var existingBorrowing = await _context.Borrowings
                .FirstOrDefaultAsync(b => b.UserId == userId && 
                                        b.BookId == createBorrowingDto.BookId && 
                                        b.Status == BorrowingStatus.Active);

            if (existingBorrowing != null)
                return null; // User already has this book

            // Create new borrowing
            var borrowing = new Borrowing
            {
                UserId = userId,
                BookId = createBorrowingDto.BookId,
                BorrowDate = DateTime.UtcNow,
                DueDate = createBorrowingDto.DueDate ?? DateTime.UtcNow.AddDays(DefaultLoanPeriodDays),
                Status = BorrowingStatus.Active,
                Notes = createBorrowingDto.Notes
            };

            // Reduce available copies
            book.AvailableCopies--;

            _context.Borrowings.Add(borrowing);
            await _context.SaveChangesAsync();

            // Load related entities for response
            await _context.Entry(borrowing)
                .Reference(b => b.User)
                .LoadAsync();

            await _context.Entry(borrowing)
                .Reference(b => b.Book)
                .LoadAsync();

            return new BorrowingDto
            {
                Id = borrowing.Id,
                UserId = borrowing.UserId,
                UserName = borrowing.User.FullName,
                BookId = borrowing.BookId,
                BookTitle = borrowing.Book.Title,
                BookAuthor = borrowing.Book.Author,
                BorrowDate = borrowing.BorrowDate,
                DueDate = borrowing.DueDate,
                ReturnDate = borrowing.ReturnDate,
                Status = borrowing.Status,
                FineAmount = borrowing.FineAmount,
                Notes = borrowing.Notes,
                IsOverdue = borrowing.IsOverdue,
                DaysOverdue = borrowing.DaysOverdue,
                IsReturned = borrowing.IsReturned
            };
        }

        public async Task<BorrowingDto?> ReturnBookAsync(ReturnBookDto returnBookDto)
        {
            var borrowing = await _context.Borrowings
                .Include(b => b.User)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.Id == returnBookDto.BorrowingId);

            if (borrowing == null || borrowing.Status != BorrowingStatus.Active)
                return null;

            // Update borrowing
            borrowing.ReturnDate = DateTime.UtcNow;
            borrowing.Status = returnBookDto.Status;
            borrowing.FineAmount = returnBookDto.FineAmount ?? await CalculateFineAsync(borrowing.Id);
            borrowing.Notes = returnBookDto.Notes;
            borrowing.UpdatedAt = DateTime.UtcNow;

            // Increase available copies (unless book is lost)
            if (returnBookDto.Status != BorrowingStatus.Lost)
            {
                borrowing.Book.AvailableCopies++;
            }

            await _context.SaveChangesAsync();

            return new BorrowingDto
            {
                Id = borrowing.Id,
                UserId = borrowing.UserId,
                UserName = borrowing.User.FullName,
                BookId = borrowing.BookId,
                BookTitle = borrowing.Book.Title,
                BookAuthor = borrowing.Book.Author,
                BorrowDate = borrowing.BorrowDate,
                DueDate = borrowing.DueDate,
                ReturnDate = borrowing.ReturnDate,
                Status = borrowing.Status,
                FineAmount = borrowing.FineAmount,
                Notes = borrowing.Notes,
                IsOverdue = borrowing.IsOverdue,
                DaysOverdue = borrowing.DaysOverdue,
                IsReturned = borrowing.IsReturned
            };
        }

        public async Task<IEnumerable<BorrowingDto>> GetOverdueBorrowingsAsync()
        {
            var overdueBorrowings = await _context.Borrowings
                .Include(b => b.User)
                .Include(b => b.Book)
                .Where(b => b.Status == BorrowingStatus.Active && b.DueDate < DateTime.UtcNow)
                .OrderBy(b => b.DueDate)
                .ToListAsync();

            return overdueBorrowings.Select(b => new BorrowingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                UserName = b.User.FullName,
                BookId = b.BookId,
                BookTitle = b.Book.Title,
                BookAuthor = b.Book.Author,
                BorrowDate = b.BorrowDate,
                DueDate = b.DueDate,
                ReturnDate = b.ReturnDate,
                Status = b.Status,
                FineAmount = b.FineAmount,
                Notes = b.Notes,
                IsOverdue = b.IsOverdue,
                DaysOverdue = b.DaysOverdue,
                IsReturned = b.IsReturned
            });
        }

        public async Task<decimal> CalculateFineAsync(int borrowingId)
        {
            var borrowing = await _context.Borrowings.FindAsync(borrowingId);
            if (borrowing == null || borrowing.Status != BorrowingStatus.Active)
                return 0;

            if (DateTime.UtcNow <= borrowing.DueDate)
                return 0;

            var daysOverdue = (DateTime.UtcNow - borrowing.DueDate).Days;
            return daysOverdue * DailyFineRate;
        }
    }
}