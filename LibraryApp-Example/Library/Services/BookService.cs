using Library.Contracts;
using Library.Data;
using Library.Data.Models;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext dbContext;

        public BookService(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        async Task IBookService.AddBookAsync(AddBookViewModel model)
        {
            Book book = new Book
            {
                Title = model.Title,
                Author = model.Author,
                ImageUrl = model.Url,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Rating = decimal.Parse(model.Rating)
            };

            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();
        }

        async Task IBookService.AddBookToCollectionAsync(string userId, BookViewModel book)
        {
            bool alreadyAdded = await dbContext.IdentityUserBooks
                .AnyAsync(ub => ub.CollectorId == userId && ub.BookId == book.Id);

            if (!alreadyAdded)
            {
                var userBook = new IdentityUserBooks
                {
                    CollectorId = userId,
                    BookId = book.Id
                };
                await dbContext.IdentityUserBooks.AddAsync(userBook);
                await dbContext.SaveChangesAsync();
            }
        }

        async Task<IEnumerable<AllBookViewModel>> IBookService.GetAllBooksAsync()
        {
            return await dbContext
                .Books
                .Select(b => new AllBookViewModel()
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ImageUrl = b.ImageUrl,
                    Rating = b.Rating,
                    Category = b.category.Name
                })
                .ToListAsync();
        }

        async Task<BookViewModel?> IBookService.GetBookByIdAsync(int id)
        {
            return await dbContext.Books
                .Where(b => b.Id == id)
                .Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    ImageUrl = b.ImageUrl,
                    Rating = b.Rating,
                    Description = b.Description,
                    CategoryId = b.CategoryId
                }).FirstOrDefaultAsync();
        }

        async Task<IEnumerable<AllBookViewModel>> IBookService.GetMyBooksAsync(string userId)
        {
           return await dbContext
               .IdentityUserBooks
               .Where(ub => ub.CollectorId == userId) //this is to filter the required entities
               .Select(b => new AllBookViewModel //this is to select the exact fields of every entity
               {
                   Id = b.Book.Id,
                   Title = b.Book.Title,
                   Author = b.Book.Author,
                   ImageUrl = b.Book.ImageUrl,
                   Description = b.Book.Description,
                   Category = b.Book.category.Name
               })
               .ToListAsync();
        }

        async Task<AddBookViewModel> IBookService.GetNewAddBookModelAsync()
        {
           var categories = await dbContext.Categories
               .Select(c => new CategoryViewModel
               {
                   Id = c.Id,
                   Name = c.Name
               }).ToListAsync();

           var model = new AddBookViewModel
           {
               Categories = categories
           };
           return model;
        }

        async Task IBookService.RemoveBookFromCollectionAsync(string userId, BookViewModel book)
        {
            //check if the book is already added in the user booklist
            bool alreadyAdded = await dbContext.IdentityUserBooks
                .AnyAsync(ub => ub.CollectorId == userId && ub.BookId == book.Id);

            //if it is added -> removing it
            if (alreadyAdded)
            {
                //find the book
                var userBook = await dbContext.IdentityUserBooks
                    .FirstOrDefaultAsync(ub => ub.CollectorId == userId && ub.BookId == book.Id);
                //mark it as removed
                dbContext.IdentityUserBooks.Remove(userBook);

                //persist changes to the db
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
