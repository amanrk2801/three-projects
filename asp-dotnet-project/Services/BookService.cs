using AutoMapper;
using LibraryManagement.Data;
using LibraryManagement.DTOs;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;

        public BookService(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<BookDto> Books, int TotalCount)> GetBooksAsync(BookSearchDto searchDto)
        {
            var query = _context.Books.Where(b => b.IsActive);

            // Apply filters
            if (!string.IsNullOrEmpty(searchDto.Title))
            {
                query = query.Where(b => b.Title.Contains(searchDto.Title));
            }

            if (!string.IsNullOrEmpty(searchDto.Author))
            {
                query = query.Where(b => b.Author.Contains(searchDto.Author));
            }

            if (!string.IsNullOrEmpty(searchDto.Genre))
            {
                query = query.Where(b => b.Genre == searchDto.Genre);
            }

            if (!string.IsNullOrEmpty(searchDto.ISBN))
            {
                query = query.Where(b => b.ISBN.Contains(searchDto.ISBN));
            }

            if (searchDto.IsAvailable.HasValue)
            {
                if (searchDto.IsAvailable.Value)
                {
                    query = query.Where(b => b.AvailableCopies > 0);
                }
                else
                {
                    query = query.Where(b => b.AvailableCopies == 0);
                }
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting
            query = searchDto.SortBy.ToLower() switch
            {
                "author" => searchDto.SortOrder.ToLower() == "desc" 
                    ? query.OrderByDescending(b => b.Author) 
                    : query.OrderBy(b => b.Author),
                "publisheddate" => searchDto.SortOrder.ToLower() == "desc" 
                    ? query.OrderByDescending(b => b.PublishedDate) 
                    : query.OrderBy(b => b.PublishedDate),
                "genre" => searchDto.SortOrder.ToLower() == "desc" 
                    ? query.OrderByDescending(b => b.Genre) 
                    : query.OrderBy(b => b.Genre),
                _ => searchDto.SortOrder.ToLower() == "desc" 
                    ? query.OrderByDescending(b => b.Title) 
                    : query.OrderBy(b => b.Title)
            };

            // Apply pagination
            var books = await query
                .Skip((searchDto.Page - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();

            var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);

            return (bookDtos, totalCount);
        }

        public async Task<BookDto?> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive);

            return book != null ? _mapper.Map<BookDto>(book) : null;
        }

        public async Task<BookDto> CreateBookAsync(CreateBookDto createBookDto)
        {
            var book = _mapper.Map<Book>(createBookDto);
            book.CreatedAt = DateTime.UtcNow;

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto?> UpdateBookAsync(int id, UpdateBookDto updateBookDto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null || !book.IsActive)
                return null;

            // Update only provided fields
            if (!string.IsNullOrEmpty(updateBookDto.Title))
                book.Title = updateBookDto.Title;

            if (!string.IsNullOrEmpty(updateBookDto.Author))
                book.Author = updateBookDto.Author;

            if (!string.IsNullOrEmpty(updateBookDto.ISBN))
                book.ISBN = updateBookDto.ISBN;

            if (!string.IsNullOrEmpty(updateBookDto.Genre))
                book.Genre = updateBookDto.Genre;

            if (!string.IsNullOrEmpty(updateBookDto.Publisher))
                book.Publisher = updateBookDto.Publisher;

            if (updateBookDto.PublishedDate.HasValue)
                book.PublishedDate = updateBookDto.PublishedDate;

            if (updateBookDto.TotalCopies.HasValue)
            {
                var difference = updateBookDto.TotalCopies.Value - book.TotalCopies;
                book.TotalCopies = updateBookDto.TotalCopies.Value;
                book.AvailableCopies += difference; // Adjust available copies
            }

            if (updateBookDto.AvailableCopies.HasValue)
                book.AvailableCopies = updateBookDto.AvailableCopies.Value;

            if (updateBookDto.Price.HasValue)
                book.Price = updateBookDto.Price;

            if (updateBookDto.Description != null)
                book.Description = updateBookDto.Description;

            if (updateBookDto.ImageUrl != null)
                book.ImageUrl = updateBookDto.ImageUrl;

            if (updateBookDto.IsActive.HasValue)
                book.IsActive = updateBookDto.IsActive.Value;

            book.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<BookDto>(book);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return false;

            // Soft delete
            book.IsActive = false;
            book.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<string>> GetGenresAsync()
        {
            return await _context.Books
                .Where(b => b.IsActive && !string.IsNullOrEmpty(b.Genre))
                .Select(b => b.Genre!)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();
        }

        public async Task<IEnumerable<BookDto>> GetAvailableBooksAsync()
        {
            var books = await _context.Books
                .Where(b => b.IsActive && b.AvailableCopies > 0)
                .OrderBy(b => b.Title)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<bool> IsBookAvailableAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            return book != null && book.IsActive && book.AvailableCopies > 0;
        }
    }
}