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

    public async Task<IActionResult> Index(int? id)
    {
        var employees = await _db.Employees.ToListAsync();

        Employee? employee = null;
        if (id.HasValue)
            employee = await _db.Employees.FindAsync(id);

        ViewBag.Employee = employee;
        return View(employees);
    }

    [HttpPost]
    public async Task<IActionResult> Save(Employee model)
    {
        if (!ModelState.IsValid)
        {
            var employees = await _db.Employees.ToListAsync();
            ViewBag.Employee = model;
            return View("Index", employees);
        }

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
            existing.Designation = model.Designation;
        }

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee != null)
        {
            _db.Employees.Remove(employee);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
