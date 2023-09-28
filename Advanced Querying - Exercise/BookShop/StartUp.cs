namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var dbContext = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            string ageRestrictionInput = Console.ReadLine();
            string result = GetBooksByAgeRestriction(dbContext, ageRestrictionInput);
            Console.WriteLine(result);
        }

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
    }
}


