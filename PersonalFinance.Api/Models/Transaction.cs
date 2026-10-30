using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Api.Models;

public class Transaction
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    public User? User { get; set; } = null;

    [Required]
    public int CategoryId { get; set; }

    public Category? Category { get; set; } = null;

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}