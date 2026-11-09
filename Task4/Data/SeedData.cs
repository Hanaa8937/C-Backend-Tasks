public static class SeedData
{
    public static void Initialize(AppDbContext db)
    {
        if (db.Cars.Any())
        {
            return;
        }

        db.Cars.AddRange(
            new Car { Id = "bmw-x5", Brand = "BMW", Model = "X5", Year = 2022, Price = "5,850,000 TL", Body = "SUV" },
            new Car { Id = "mercedes-e200", Brand = "Mercedes", Model = "E200", Year = 2021, Price = "4,420,000 TL", Body = "Sedan" },
            new Car { Id = "audi-q8", Brand = "Audi", Model = "Q8", Year = 2020, Price = "6,250,000 TL", Body = "SUV" },
            new Car { Id = "porsche-macan", Brand = "Porsche", Model = "Macan", Year = 2021, Price = "5,300,000 TL", Body = "SUV" },
            new Car { Id = "bmw-m4", Brand = "BMW", Model = "M4 Competition Sport", Year = 2023, Price = "7,900,000 TL", Body = "Coupe" },
            new Car { Id = "volvo-xc90", Brand = "Volvo", Model = "XC90", Year = 2021, Price = "4,760,000 TL", Body = "SUV" }
        );

        db.SaveChanges();
    }
}