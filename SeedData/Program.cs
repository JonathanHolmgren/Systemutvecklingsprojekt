

using DataLayer;

namespace SeedData // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
           
            Context context = new Context();

            Console.WriteLine("Would you like to reset the Database? Y/N");
            
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                Seed.Populate(context);

            }
        }
    }
}
 

