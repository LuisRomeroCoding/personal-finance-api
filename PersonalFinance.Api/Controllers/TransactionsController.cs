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
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        int userId = User.GetUserId();

        var transactions = await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .ToListAsync();

        return Ok(transactions);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto dto)
    {
        int userId = User.GetUserId();

        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId && (c.UserId == null || c.UserId == userId));

        if (category == null)
        {
            return BadRequest(new { message = "Invalid category specified"});
        }

        var transaction = new Transaction
        {
            UserId = userId,
            CategoryId = dto.CategoryId,
            Amount = dto.Amount,
            Date = dto.Date,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return Ok(transaction);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        int userId = User.GetUserId();

        var transaction = _context.Transactions
            .FirstOrDefault(t => t.Id == id && t.UserId == userId);

        if (transaction == null)
        {
            return NotFound(new { message = "Transaction not fount" });
        }

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Transaction deleted successfully" });
    }
}