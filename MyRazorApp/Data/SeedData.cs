using MyRazorApp.Models;

namespace MyRazorApp.Data
{
    public static class SeedData
    {
        public static void Seed(SchoolDbContext context)
        {
            if (context.Classes.Any())
                return;

            var random = new Random();
            var classes = new List<Class>();

            for (int i = 1; i <= 100; i++)
            {
                classes.Add(new Class
                {
                    Name = $"Sample Class {i}",
                    PersonCount = random.Next(10, 50),
                    Description = $"This is a sample description for class {i}.",
                    IsActive = true
                });
            }

            context.Classes.AddRange(classes);
            context.SaveChanges();
        }
    }
}
