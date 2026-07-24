public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        if (db.Cars.Any())
        {
            return;
        }

        db.Cars.AddRange(
            new Car { Id = "bmw-x5", Brand = "BMW", Model = "X5", Year = 2022, Price = "5,850,000 TL" },
            new Car { Id = "mercedes-e200", Brand = "Mercedes", Model = "E200", Year = 2021, Price = "4,420,000 TL" },
            new Car { Id = "audi-q8", Brand = "Audi", Model = "Q8", Year = 2020, Price = "6,250,000 TL" }
        );

        db.SaveChanges();
    }
}