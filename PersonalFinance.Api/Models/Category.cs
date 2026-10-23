using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Api.Models;

public class Category
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public User? User { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(7)]
    public string ColorHex { get; set; } = "#3B82F6";
}