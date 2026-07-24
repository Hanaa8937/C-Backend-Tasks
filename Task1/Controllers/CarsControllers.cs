using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private List<Car> cars = new List<Car>
    {
        new Car { Id = "bmw-x5", Brand = "BMW", Model = "X5", Year = 2022, Price = "5,850,000 TL" },
        new Car { Id = "mercedes-e200", Brand = "Mercedes", Model = "E200", Year = 2021, Price = "4,420,000 TL" },
        new Car { Id = "audi-q8", Brand = "Audi", Model = "Q8", Year = 2020, Price = "6,250,000 TL" }
    };

    [HttpGet]
    public ActionResult<List<Car>> GetAll()
    {
        return Ok(cars);
    }
}