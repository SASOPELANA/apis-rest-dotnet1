using apisRestNet.Api.Data;
using apisRestNet.Api.Dto;
using apisRestNet.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apisRestNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly AppDbContext _context;


    public CarsController(AppDbContext context)
    {
        _context = context;
    }


    public bool IsValidYear(int year)

    {
        var currentYear = DateTime.Now.Year;
        return year >= currentYear - 5;
    }


    private CarDto ToDto(Car car)
    {
        return new CarDto
        {
            Id = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Price = car.Price,
            Year = car.Year
        };
    }

    private Car ToEntity(CarDto dto)
    {
        return new Car
        {
            Id = dto.Id,
            Brand = dto.Brand,
            Model = dto.Model,
            Price = dto.Price,
            Year = dto.Year
        };
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetCard()
    {
        var res = await _context.Cars.ToListAsync();
        return res.Select(ToDto).ToList();
    }


    // Get api/cars/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CarDto>> GetCar(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null) return NotFound();
        return ToDto(car);
    }


    // POST: api/cars
    [HttpPost]
    public async Task<ActionResult<CarDto>> CreateCar(CarDto carDto)
    {
        if (!IsValidYear(carDto.Year))
            return BadRequest("The car cannot be older than 5 years.");

        var car = ToEntity(carDto);
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        carDto.Id = car.Id;
        return CreatedAtAction(nameof(GetCar), new { id = car.Id }, carDto);
    }

    // PUT: api/cars/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCar(int id, CarDto carDto)
    {
        if (id != carDto.Id)
            return BadRequest();

        if (!IsValidYear(carDto.Year))
            return BadRequest("The car cannot be older than 5 years.");

        var res = await _context.Cars.FindAsync(id);
        if (res == null)
            return NotFound();

        res.Brand = carDto.Brand;
        res.Model = carDto.Model;
        res.Price = carDto.Price;
        res.Year = carDto.Year;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/cars/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        var res = await _context.Cars.FindAsync(id);
        if (res == null)
            return NotFound();

        _context.Cars.Remove(res);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}