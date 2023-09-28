namespace BookShop
{
    using System.Collections;
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var dbContext = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            string result = GetBooksByCategory(dbContext, "horror mystery drama");
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

        //Problem 06:
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
    }
}


