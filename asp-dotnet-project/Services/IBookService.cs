using LibraryManagement.DTOs;

namespace LibraryManagement.Services
{
    public interface IBookService
    {
        Task<(IEnumerable<BookDto> Books, int TotalCount)> GetBooksAsync(BookSearchDto searchDto);
        Task<BookDto?> GetBookByIdAsync(int id);
        Task<BookDto> CreateBookAsync(CreateBookDto createBookDto);
        Task<BookDto?> UpdateBookAsync(int id, UpdateBookDto updateBookDto);
        Task<bool> DeleteBookAsync(int id);
        Task<IEnumerable<string>> GetGenresAsync();
        Task<IEnumerable<BookDto>> GetAvailableBooksAsync();
        Task<bool> IsBookAvailableAsync(int bookId);
    }
}