using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Employee
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Department { get; set; }

    public string? Designation { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
