using System.ComponentModel.DataAnnotations;

namespace PersonalFinance.Api.DTOs;

public class CreateCategoryDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, RegularExpression("^(Income|Expense)$", ErrorMessage = "Type must be either 'Income' or 'Expense'.")]
    public string Type { get; set; } = string.Empty;

    public string? ColorHex { get; set; }
}