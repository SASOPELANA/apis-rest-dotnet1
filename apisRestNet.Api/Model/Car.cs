namespace apisRestNet.Api.Model;

public class Car
{
    public int Id { get; set; }
    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public decimal Price { get; set; }
    public int Year { get; set; }


    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}