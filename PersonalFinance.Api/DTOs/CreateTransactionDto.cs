using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Api.DTOs;

public class CreateTransactionDto
{
    [Required]
    public int CategoryId { get; set; }

    [Required, Range(0.01, 1000000000, ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public string? Description { get; set; }
}