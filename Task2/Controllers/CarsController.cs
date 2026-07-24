using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    // static so the list survives between requests, not just within one request
    private static List<Car> cars = new List<Car>
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

    [HttpGet("{id}")]
    public ActionResult<Car> GetById(string id)
    {
        Car? car = cars.FirstOrDefault(c => c.Id == id);
        if (car == null)
        {
            return NotFound(new { error = $"No car found with ID '{id}'." });
        }
        return Ok(car);
    }

    [HttpPost]
    public ActionResult<Car> Create([FromBody] Car newCar)
    {
        cars.Add(newCar);
        return CreatedAtAction(nameof(GetById), new { id = newCar.Id }, newCar);
    }

    [HttpPut("{id}")]
    public IActionResult Update(string id, [FromBody] Car updatedCar)
    {
        Car? car = cars.FirstOrDefault(c => c.Id == id);
        if (car == null)
        {
            return NotFound(new { error = $"No car found with ID '{id}'." });
        }

        car.Brand = updatedCar.Brand;
        car.Model = updatedCar.Model;
        car.Year = updatedCar.Year;
        car.Price = updatedCar.Price;

        return Ok(car);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        Car? car = cars.FirstOrDefault(c => c.Id == id);
        if (car == null)
        {
            return NotFound(new { error = $"No car found with ID '{id}'." });
        }

        cars.Remove(car);
        return NoContent();
    }
}