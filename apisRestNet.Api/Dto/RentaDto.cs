using System.ComponentModel.DataAnnotations;

namespace apisRestNet.Api.Dto;

public class RentaDto
{
    [Required] public int UserId { get; set; }

    [Required] public int CarId { get; set; }

    [Required] public DateTime StartDate { get; set; }

    [Required] public DateTime EndDate { get; set; }
}