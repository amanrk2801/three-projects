using LibraryManagement.DTOs;

namespace LibraryManagement.Services
{
    public interface IBorrowingService
    {
        Task<(IEnumerable<BorrowingDto> Borrowings, int TotalCount)> GetBorrowingsAsync(BorrowingSearchDto searchDto);
        Task<BorrowingDto?> GetBorrowingByIdAsync(int id);
        Task<IEnumerable<BorrowingDto>> GetUserBorrowingsAsync(string userId);
        Task<BorrowingDto?> BorrowBookAsync(string userId, CreateBorrowingDto createBorrowingDto);
        Task<BorrowingDto?> ReturnBookAsync(ReturnBookDto returnBookDto);
        Task<IEnumerable<BorrowingDto>> GetOverdueBorrowingsAsync();
        Task<decimal> CalculateFineAsync(int borrowingId);
    }
}