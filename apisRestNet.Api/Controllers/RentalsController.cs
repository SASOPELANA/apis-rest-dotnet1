using apisRestNet.Api.Data;
using apisRestNet.Api.Dto;
using apisRestNet.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apisRestNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalsController : ControllerBase
{
    private readonly AppDbContext _context;

    public RentalsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRental([FromBody] RentaDto rentalDto)
    {
        if (rentalDto.StartDate >= rentalDto.EndDate)
            return BadRequest("Invalid rental period.");

        var car = await _context.Cars.FindAsync(rentalDto.CarId);
        if (car == null)
            return NotFound("Car not found.");

        var user = await _context.Users.FindAsync(rentalDto.UserId);
        if (user == null)
            return NotFound("User not found.");

        // Check availability
        var isCarAvailable = !await _context.Rentals.AnyAsync(r =>
            r.CarId == rentalDto.CarId &&
            (
                (rentalDto.StartDate >= r.StartDate && rentalDto.StartDate < r.EndDate) ||
                (rentalDto.EndDate > r.StartDate && rentalDto.EndDate <= r.EndDate) ||
                (rentalDto.StartDate <= r.StartDate && rentalDto.EndDate >= r.EndDate)
            ));

        if (!isCarAvailable)
            return Conflict("Car is not available for the selected period.");

        var totalDays = (rentalDto.EndDate - rentalDto.StartDate).Days;
        if (totalDays <= 0)
            return BadRequest("Rental must be at least one day.");

        var totalPrice = totalDays * car.Price;

        var rental = new Rental
        {
            UserId = rentalDto.UserId,
            CarId = rentalDto.CarId,
            StartDate = rentalDto.StartDate,
            EndDate = rentalDto.EndDate
            //Price = totalPrice
        };

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            rental.Id,
            TotalPrice = totalPrice
        });
    }
}