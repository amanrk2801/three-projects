using LibraryManagement.DTOs;
using LibraryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BorrowingsController : ControllerBase
    {
        private readonly IBorrowingService _borrowingService;

        public BorrowingsController(IBorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> GetBorrowings([FromQuery] BorrowingSearchDto searchDto)
        {
            var (borrowings, totalCount) = await _borrowingService.GetBorrowingsAsync(searchDto);

            var response = new
            {
                borrowings,
                totalCount,
                page = searchDto.Page,
                pageSize = searchDto.PageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / searchDto.PageSize)
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBorrowing(int id)
        {
            var borrowing = await _borrowingService.GetBorrowingByIdAsync(id);
            if (borrowing == null)
                return NotFound();

            // Check if user can access this borrowing
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            if (borrowing.UserId != userId && !userRoles.Contains("Admin") && !userRoles.Contains("Librarian"))
                return Forbid();

            return Ok(borrowing);
        }

        [HttpGet("my-borrowings")]
        public async Task<IActionResult> GetMyBorrowings()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var borrowings = await _borrowingService.GetUserBorrowingsAsync(userId);
            return Ok(borrowings);
        }

        [HttpGet("overdue")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> GetOverdueBorrowings()
        {
            var borrowings = await _borrowingService.GetOverdueBorrowingsAsync();
            return Ok(borrowings);
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook([FromBody] CreateBorrowingDto createBorrowingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var borrowing = await _borrowingService.BorrowBookAsync(userId, createBorrowingDto);
            if (borrowing == null)
                return BadRequest(new { message = "Unable to borrow book. It may not be available or you may already have it borrowed." });

            return CreatedAtAction(nameof(GetBorrowing), new { id = borrowing.Id }, borrowing);
        }

        [HttpPost("return")]
        [Authorize(Roles = "Admin,Librarian")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookDto returnBookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var borrowing = await _borrowingService.ReturnBookAsync(returnBookDto);
            if (borrowing == null)
                return BadRequest(new { message = "Unable to return book. Borrowing record not found or already returned." });

            return Ok(borrowing);
        }

        [HttpGet("{id}/calculate-fine")]
        public async Task<IActionResult> CalculateFine(int id)
        {
            var borrowing = await _borrowingService.GetBorrowingByIdAsync(id);
            if (borrowing == null)
                return NotFound();

            // Check if user can access this borrowing
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRoles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            if (borrowing.UserId != userId && !userRoles.Contains("Admin") && !userRoles.Contains("Librarian"))
                return Forbid();

            var fine = await _borrowingService.CalculateFineAsync(id);
            return Ok(new { borrowingId = id, fineAmount = fine });
        }
    }
}