namespace EntityRelations;
public class Program
{
    public static void Main(string[] args)
    {
        ApplicationContext context = new ApplicationContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}
