using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Authorize]
public class EmployeeController : Controller
{
    private readonly AppDbContext _db;

    public EmployeeController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetData()
    {
        var employees = await _db.Employees.OrderBy(e => e.Id).ToListAsync();
        return Json(new { data = employees });
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null) return NotFound();
        return Json(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] Employee model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (model.Id == 0)
        {
            model.CreatedAt = DateTime.UtcNow;
            _db.Employees.Add(model);
        }
        else
        {
            var existing = await _db.Employees.FindAsync(model.Id);
            if (existing == null) return NotFound();

            existing.Name = model.Name;
            existing.Email = model.Email;
            existing.Phone = model.Phone;
            existing.Department = model.Department;
            existing.Salary = model.Salary;
        }

        await _db.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
        return Ok();
    }
}
