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

            string result = GetBooksByAuthor(dbContext, "po");
            Console.WriteLine(result);
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
    }
}


