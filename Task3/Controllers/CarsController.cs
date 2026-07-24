using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly AppDbContext db;

    public CarsController(AppDbContext db)
    {
        this.db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Car>>> GetAll()
    {
        return Ok(await db.Cars.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> GetById(string id)
    {
        Car? car = await db.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound(new { error = $"No car found with ID '{id}'." });
        }
        return Ok(car);
    }

    [HttpPost]
    public async Task<ActionResult<Car>> Create([FromBody] Car newCar)
    {
        db.Cars.Add(newCar);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = newCar.Id }, newCar);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Car updatedCar)
    {
        Car? car = await db.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound(new { error = $"No car found with ID '{id}'." });
        }

        car.Brand = updatedCar.Brand;
        car.Model = updatedCar.Model;
        car.Year = updatedCar.Year;
        car.Price = updatedCar.Price;

        await db.SaveChangesAsync();
        return Ok(car);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        Car? car = await db.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound(new { error = $"No car found with ID '{id}'." });
        }

        db.Cars.Remove(car);
        await db.SaveChangesAsync();
        return NoContent();
    }
}