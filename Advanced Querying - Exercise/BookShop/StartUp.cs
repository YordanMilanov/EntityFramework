namespace BookShop
{
    using System.Collections;
    using System.Globalization;
    using System.Text;
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using var dbContext = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

          int removedBooks = RemoveBooks(dbContext);
            Console.WriteLine(removedBooks);
        }

        //Problem 02: 100
        public static string GetBooksByAgeRestriction(BookShopContext dbContext, string command) 
        {
            bool hasParsed = Enum.TryParse(typeof(AgeRestriction), command, true, out object? ageRestrictionObject);
           AgeRestriction ageRestriction;

            if (hasParsed)
            {
                ageRestriction = (AgeRestriction)ageRestrictionObject;
            

                String[] bookTitles = dbContext.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToArray();
            return String.Join(Environment.NewLine, bookTitles);
            }
            return null;
        }

        //Problem 03: 100

        public static string GetGoldenBooks(BookShopContext dbContext)
        {
            string[] bookTitles = dbContext.Books
                .Where(b => b.EditionType == EditionType.Gold
                && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        //Problem 04: 100 +
        public static string GetBooksByPrice(BookShopContext dbContext)
        {
            var Books = dbContext.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    title = b.Title,
                    price = b.Price
                })
                .ToArray();

            List<string> output = new List<string>();

            foreach (var b in Books)
            {
                output.Add($"{b.title} - ${b.price:f2}");
            }

            return string.Join(Environment.NewLine, output);
        }

        //Problem 05: 100 +
        public static string GetBooksNotReleasedIn(BookShopContext dbContext, int year)
        {
            string[] bookTitles = dbContext.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        //Problem 06: 100
        public static string GetBooksByCategory(BookShopContext dbContext, string input)
        {
            string[] categories = input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower()) //Select == Map in Java
                .ToArray();

            string[] bookTitles = dbContext.Books
                .Where(b => b.BookCategories
                        .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        //Problem 07: 100 +
        public static string GetBooksReleasedBefore(BookShopContext dbContext, string date)
        {
            string format = "dd-MM-yyyy";
            DateTime dateTime;
            if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                var Books = dbContext.Books
                    .Where(b => b.ReleaseDate < dateTime)
                    .Select(b => new
                    {
                        b.Title,
                        b.EditionType,
                        b.Price,
                        b.ReleaseDate
                    })
                    .OrderByDescending(b => b.ReleaseDate)

                    .ToArray();

                StringBuilder sb = new StringBuilder();
                foreach (var b in Books)
                {
                    sb.AppendLine($"{b.Title} - {b.EditionType} - ${b.Price:f2}");
                }

                return sb.ToString();
            }
            else
            {
               return "Invalid date format";
            }
        }

        //Problem 08: 100 +
        public static string GetAuthorNamesEndingIn(BookShopContext dbContext, string input)
        {
            string[] authors = dbContext.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToArray();

            return string.Join(Environment.NewLine, authors);
        }

        //Problem 09: 100 +
        public static string GetBookTitlesContaining(BookShopContext dbContext, string input) 
        {
            string[] bookTitles = dbContext.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        //Problem 10: 100 +
        public static string GetBooksByAuthor(BookShopContext dbContext, string input)
        {
            string[] books = dbContext.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} (${b.Author.FirstName} {b.Author.LastName})")
                .ToArray();
                
                
                return string.Join(Environment.NewLine, books);
        }

        //Problem 11: 100 +
        public static string CountBooks(BookShopContext dbContext, int lengthCheck)
        {
            int count = dbContext.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();

            return $"There are {count} books with longer title than {lengthCheck} symbols";
        }

        //Problem 12: 100
        public static string CountCopiesByAuthor(BookShopContext dbContext)
        {
            StringBuilder sb = new StringBuilder();
            var AuthorWithBookCopies = dbContext.Authors
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName,
                    TotalCopies = a.Books.Sum(b => b.Copies)
                } 
                )
                .OrderByDescending(b => b.TotalCopies)
                .ToArray();

            foreach (var a in AuthorWithBookCopies)
            {
                sb.AppendLine($"{a.FullName} - {a.TotalCopies}");
            }

            return sb.ToString();
        }

        //Problem 13: 100
        public static string GetTotalProfitByCategory(BookShopContext dbContext)
        {
            StringBuilder sb = new StringBuilder();
            var TotalProfitByCategory = dbContext.Categories
                .Select(c => new
                {
                   CategoryName = c.Name,
                   TotalProfit = c.CategoryBooks
                                    .Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.CategoryName)
                .ToArray();

            foreach (var a in TotalProfitByCategory)
            {
                sb.AppendLine($"{a.CategoryName} ${a.TotalProfit:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        //Problem 14: 100
        public static string GetMostRecentBooks(BookShopContext dbContext)
        {
            StringBuilder sb = new StringBuilder();

            var mostRecentBooksByCategory = dbContext.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    MostRecentBooks = c.CategoryBooks.
                        OrderByDescending(cb => cb.Book.ReleaseDate)
                        .Take(3)
                        .Select(cb => new
                        {
                            BookTitle = cb.Book.Title,
                            ReleaseYear = cb.Book.ReleaseDate.Value.Year
                        }).ToArray()
                })
                .ToArray();
            foreach (var category in mostRecentBooksByCategory)
            {
                sb.AppendLine($"--{category.CategoryName}");
                foreach (var book in category.MostRecentBooks)
                {
                    sb.AppendLine($"{book.BookTitle} ({book.ReleaseYear})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 15 - Without Bulk update
        public static void IncreasePrices(BookShopContext dbContext)
        {
            Book[] booksRealeasedBefore2010 = dbContext.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year < 2010)
                .ToArray(); //Materializing the query does not detach th eentities from the changeTracker

            foreach (var book in booksRealeasedBefore2010)
            {
                book.Price += 5;
            }

            dbContext.SaveChanges();
        }

        //Problem 15 - With Bulk update
        public static void BulkIncreasePrices(BookShopContext dbContext)
        {
            Book[] booksRealeasedBefore2010 = dbContext.Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year < 2010)
                .ToArray(); //Materializing the query does not detach th eentities from the changeTracker

            foreach (var book in booksRealeasedBefore2010)
            {
                book.Price += 5;
            }

            dbContext.BulkUpdate(booksRealeasedBefore2010);
        }

        //Problem 16 - Delete
        public static int RemoveBooks(BookShopContext dbContext) {
            Book[] removedBooksCount = dbContext.Books
                .Where(b => b.Copies < 4200).ToArray();

            int count = 0;
            foreach (var book in removedBooksCount)
            {
                dbContext.Books.Remove(book);
                count++;
            }

            return count;
        }
    }
}


