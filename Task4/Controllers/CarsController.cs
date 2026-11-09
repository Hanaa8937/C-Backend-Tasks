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
    public async Task<ActionResult> GetAll(
        [FromQuery] string? brand,
        [FromQuery] string? body,
        [FromQuery] int? minYear,
        [FromQuery] int? maxYear,
        [FromQuery] string? search,
        [FromQuery] string? sort,
        [FromQuery] string? order,
        [FromQuery] int page = 1)
    {
        IQueryable<Car> query = db.Cars;

        if (!string.IsNullOrEmpty(brand))
        {
            query = query.Where(c => c.Brand == brand);
        }

        if (!string.IsNullOrEmpty(body))
        {
            query = query.Where(c => c.Body == body);
        }

        if (minYear.HasValue)
        {
            query = query.Where(c => c.Year >= minYear.Value);
        }

        if (maxYear.HasValue)
        {
            query = query.Where(c => c.Year <= maxYear.Value);
        }

        if (!string.IsNullOrEmpty(search))
        {
            string term = search.ToLower();
            query = query.Where(c => c.Brand.ToLower().Contains(term) || c.Model.ToLower().Contains(term));
        }

        if (sort == "price")
        {
            query = order == "desc" ? query.OrderByDescending(c => c.Price) : query.OrderBy(c => c.Price);
        }
        else if (sort == "year")
        {
            query = order == "desc" ? query.OrderByDescending(c => c.Year) : query.OrderBy(c => c.Year);
        }
        else
        {
            query = query.OrderBy(c => c.Id);
        }

        int totalCount = await query.CountAsync();
        int pageSize = 10;
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        List<Car> data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new { data, page, pageSize, totalCount, totalPages });
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
}