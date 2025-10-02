using LibraryManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Data
{
    public static class SeedData
    {
        public static async Task Initialize(LibraryContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Seed roles
            await SeedRoles(roleManager);

            // Seed users
            await SeedUsers(userManager);

            // Seed books
            await SeedBooks(context);

            // Seed borrowings
            await SeedBorrowings(context, userManager);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Librarian", "Member" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // Admin user
            if (await userManager.FindByEmailAsync("admin@library.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@library.com",
                    Email = "admin@library.com",
                    FirstName = "Admin",
                    LastName = "User",
                    Address = "123 Library St, Book City, BC 12345",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Librarian user
            if (await userManager.FindByEmailAsync("librarian@library.com") == null)
            {
                var librarianUser = new ApplicationUser
                {
                    UserName = "librarian@library.com",
                    Email = "librarian@library.com",
                    FirstName = "Jane",
                    LastName = "Librarian",
                    Address = "456 Book Ave, Reading Town, RT 67890",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(librarianUser, "Librarian123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(librarianUser, "Librarian");
                }
            }

            // Member users
            var members = new[]
            {
                new { Email = "john.doe@email.com", FirstName = "John", LastName = "Doe", Address = "789 Reader Rd, Novel City, NC 11111" },
                new { Email = "jane.smith@email.com", FirstName = "Jane", LastName = "Smith", Address = "321 Story St, Fiction Town, FT 22222" },
                new { Email = "bob.johnson@email.com", FirstName = "Bob", LastName = "Johnson", Address = "654 Chapter Ln, Poetry Place, PP 33333" }
            };

            foreach (var member in members)
            {
                if (await userManager.FindByEmailAsync(member.Email) == null)
                {
                    var memberUser = new ApplicationUser
                    {
                        UserName = member.Email,
                        Email = member.Email,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        Address = member.Address,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(memberUser, "Member123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(memberUser, "Member");
                    }
                }
            }
        }

        private static async Task SeedBooks(LibraryContext context)
        {
            if (context.Books.Any())
                return;

            var books = new[]
            {
                new Book
                {
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    ISBN = "9780743273565",
                    Genre = "Classic Literature",
                    Publisher = "Scribner",
                    PublishedDate = new DateTime(1925, 4, 10),
                    TotalCopies = 5,
                    AvailableCopies = 3,
                    Price = 12.99m,
                    Description = "A classic American novel set in the Jazz Age, exploring themes of wealth, love, and the American Dream.",
                    ImageUrl = "https://example.com/great-gatsby.jpg"
                },
                new Book
                {
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    ISBN = "9780061120084",
                    Genre = "Classic Literature",
                    Publisher = "J.B. Lippincott & Co.",
                    PublishedDate = new DateTime(1960, 7, 11),
                    TotalCopies = 4,
                    AvailableCopies = 2,
                    Price = 14.99m,
                    Description = "A gripping tale of racial injustice and childhood innocence in the American South.",
                    ImageUrl = "https://example.com/mockingbird.jpg"
                },
                new Book
                {
                    Title = "1984",
                    Author = "George Orwell",
                    ISBN = "9780451524935",
                    Genre = "Dystopian Fiction",
                    Publisher = "Secker & Warburg",
                    PublishedDate = new DateTime(1949, 6, 8),
                    TotalCopies = 6,
                    AvailableCopies = 4,
                    Price = 13.99m,
                    Description = "A dystopian social science fiction novel about totalitarian control and surveillance.",
                    ImageUrl = "https://example.com/1984.jpg"
                },
                new Book
                {
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    ISBN = "9780141439518",
                    Genre = "Romance",
                    Publisher = "T. Egerton",
                    PublishedDate = new DateTime(1813, 1, 28),
                    TotalCopies = 3,
                    AvailableCopies = 1,
                    Price = 11.99m,
                    Description = "A romantic novel that critiques the British landed gentry at the end of the 18th century.",
                    ImageUrl = "https://example.com/pride-prejudice.jpg"
                },
                new Book
                {
                    Title = "The Catcher in the Rye",
                    Author = "J.D. Salinger",
                    ISBN = "9780316769174",
                    Genre = "Coming-of-age Fiction",
                    Publisher = "Little, Brown and Company",
                    PublishedDate = new DateTime(1951, 7, 16),
                    TotalCopies = 4,
                    AvailableCopies = 3,
                    Price = 15.99m,
                    Description = "A controversial novel about teenage rebellion and alienation in post-war America.",
                    ImageUrl = "https://example.com/catcher-rye.jpg"
                },
                new Book
                {
                    Title = "Harry Potter and the Philosopher's Stone",
                    Author = "J.K. Rowling",
                    ISBN = "9780747532699",
                    Genre = "Fantasy",
                    Publisher = "Bloomsbury",
                    PublishedDate = new DateTime(1997, 6, 26),
                    TotalCopies = 8,
                    AvailableCopies = 5,
                    Price = 16.99m,
                    Description = "The first book in the beloved Harry Potter series about a young wizard's adventures.",
                    ImageUrl = "https://example.com/harry-potter.jpg"
                },
                new Book
                {
                    Title = "The Lord of the Rings",
                    Author = "J.R.R. Tolkien",
                    ISBN = "9780544003415",
                    Genre = "Fantasy",
                    Publisher = "George Allen & Unwin",
                    PublishedDate = new DateTime(1954, 7, 29),
                    TotalCopies = 5,
                    AvailableCopies = 2,
                    Price = 25.99m,
                    Description = "An epic high fantasy novel about the quest to destroy the One Ring.",
                    ImageUrl = "https://example.com/lotr.jpg"
                },
                new Book
                {
                    Title = "Dune",
                    Author = "Frank Herbert",
                    ISBN = "9780441172719",
                    Genre = "Science Fiction",
                    Publisher = "Chilton Books",
                    PublishedDate = new DateTime(1965, 8, 1),
                    TotalCopies = 3,
                    AvailableCopies = 1,
                    Price = 18.99m,
                    Description = "A science fiction novel set in the distant future amidst a feudal interstellar society.",
                    ImageUrl = "https://example.com/dune.jpg"
                }
            };

            context.Books.AddRange(books);
            await context.SaveChangesAsync();
        }

        private static async Task SeedBorrowings(LibraryContext context, UserManager<ApplicationUser> userManager)
        {
            if (context.Borrowings.Any())
                return;

            var johnDoe = await userManager.FindByEmailAsync("john.doe@email.com");
            var janeSmith = await userManager.FindByEmailAsync("jane.smith@email.com");
            var bobJohnson = await userManager.FindByEmailAsync("bob.johnson@email.com");

            if (johnDoe == null || janeSmith == null || bobJohnson == null)
                return;

            var borrowings = new[]
            {
                new Borrowing
                {
                    UserId = johnDoe.Id,
                    BookId = 1, // The Great Gatsby
                    BorrowDate = DateTime.UtcNow.AddDays(-10),
                    DueDate = DateTime.UtcNow.AddDays(4),
                    Status = BorrowingStatus.Active
                },
                new Borrowing
                {
                    UserId = johnDoe.Id,
                    BookId = 5, // The Catcher in the Rye
                    BorrowDate = DateTime.UtcNow.AddDays(-20),
                    DueDate = DateTime.UtcNow.AddDays(-6),
                    ReturnDate = DateTime.UtcNow.AddDays(-5),
                    Status = BorrowingStatus.Returned
                },
                new Borrowing
                {
                    UserId = janeSmith.Id,
                    BookId = 2, // To Kill a Mockingbird
                    BorrowDate = DateTime.UtcNow.AddDays(-15),
                    DueDate = DateTime.UtcNow.AddDays(-1),
                    Status = BorrowingStatus.Active,
                    FineAmount = 2.50m
                },
                new Borrowing
                {
                    UserId = janeSmith.Id,
                    BookId = 4, // Pride and Prejudice
                    BorrowDate = DateTime.UtcNow.AddDays(-8),
                    DueDate = DateTime.UtcNow.AddDays(6),
                    Status = BorrowingStatus.Active
                },
                new Borrowing
                {
                    UserId = bobJohnson.Id,
                    BookId = 6, // Harry Potter
                    BorrowDate = DateTime.UtcNow.AddDays(-25),
                    DueDate = DateTime.UtcNow.AddDays(-11),
                    ReturnDate = DateTime.UtcNow.AddDays(-10),
                    Status = BorrowingStatus.Returned
                },
                new Borrowing
                {
                    UserId = bobJohnson.Id,
                    BookId = 7, // The Lord of the Rings
                    BorrowDate = DateTime.UtcNow.AddDays(-5),
                    DueDate = DateTime.UtcNow.AddDays(9),
                    Status = BorrowingStatus.Active
                }
            };

            context.Borrowings.AddRange(borrowings);
            await context.SaveChangesAsync();
        }
    }
}