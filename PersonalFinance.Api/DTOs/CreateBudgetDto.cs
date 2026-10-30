using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Api.DTOs;

public class CreateBudgetDto
{
    [Required]
    public int CategoryId { get; set; }

    [Required, Range(0.01, 1000000000, ErrorMessage = "Montlhy limit must be greater than zero.")]
    public decimal MonthlyLimit { get; set; }

    [Required, Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
    public int Month { get; set; }

    [Required, Range(2000, 2100, ErrorMessage = "Invalid year.")]
    public int Year { get; set; }
}