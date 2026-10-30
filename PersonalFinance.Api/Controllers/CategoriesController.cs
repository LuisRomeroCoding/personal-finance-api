using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinance.Api.Data;
using PersonalFinance.Api.DTOs;
using PersonalFinance.Api.Extensions;
using PersonalFinance.Api.Models;

namespace PersonalFinance.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public  CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        int userId = User.GetUserId();

        var categories = await _context.Categories
            .Where(c => c.UserId == null || c.UserId == userId)
            .ToListAsync();

        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto)
    {
        int userId = User.GetUserId();

        var category = new Category
        {
            UserId = userId,
            Name = dto.Name,
            Type = dto.Type,
            ColorHex = dto.ColorHex ?? "#CCCCCC"
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return Ok(category);
    }
}