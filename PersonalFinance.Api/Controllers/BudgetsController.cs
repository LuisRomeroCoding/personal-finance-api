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
public class BudgetsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BudgetsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetBudgets([FromQuery] int? month, int? year)
    {
        int userId = User.GetUserId();

        var query = _context.Budgets
            .Include(b => b.Category)
            .Where(b => b.UserId == userId);

        if (month.HasValue)
        {
            query = query.Where(b => b.Month == month);
        }

        if (year.HasValue)
        {
            query = query.Where(b => b.Year == year);
        }

        var budgets = await query.ToListAsync();
        return Ok(budgets);
    }

    [HttpPost]
    public async Task<IActionResult> UpsertBudget([FromBody] CreateBudgetDto dto)
    {
        int userId = User.GetUserId();

        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId && (c.UserId == null || c.UserId == userId));

        if (category == null)
        {
            return BadRequest(new { message = "Invalid category specified." });
        }

        var existingBudget = await _context.Budgets
            .FirstOrDefaultAsync(b => b.UserId == userId && b.CategoryId == dto.CategoryId && b.Month == dto.Month && b.Year == dto.Year);

        if (existingBudget != null)
        {
            existingBudget.MonthlyLimit = dto.MonthlyLimit;
            _context.Budgets.Update(existingBudget);
            await _context.SaveChangesAsync();
            return Ok(existingBudget);
        }

        var budget = new Budget
        {
            UserId = userId,
            CategoryId = dto.CategoryId,
            MonthlyLimit = dto.MonthlyLimit,
            Month = dto.Month,
            Year = dto.Year,
        };

        _context.Budgets.Add(budget);
        await _context.SaveChangesAsync();

        return Ok(budget);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(int id)
    {
        int userId = User.GetUserId();

        var budget = await _context.Budgets
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

        if (budget == null)
        {
            return BadRequest(new { message = "Budget not found." });
        }

        _context.Budgets.Remove(budget);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Budget deleted succesfully." });
    }
}