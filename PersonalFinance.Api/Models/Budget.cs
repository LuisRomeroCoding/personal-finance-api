using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinance.Api.Models;

public class Budget
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    
    public User? User { get; set; } = null;
    
    [Required]
    public int CategiryId { get; set; }

    public Category? Category { get; set; } = null;

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal MonthlyLimit { get; set; }

    [Required]
    public int Month { get; set; }

    [Required]
    public int Year { get; set; }
}