using System.Linq;

namespace EllaX.Data
{
    public class ApplicationInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            ApplicationInitializer initializer = new ApplicationInitializer();
            initializer.SeedEverything(context);
        }

        public void SeedEverything(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Peers.Any())
            {
                // Db has been seeded
            }
        }
    }
}
