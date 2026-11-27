namespace backend.Models;

public sealed class RetakeRequest
{
    public int Id { get; set; }
    public int Number { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int DisciplineId { get; set; }
    public string Status { get; set; } = "New";
    public DateTimeOffset CreatedAt { get; set; }
    public int CreatedByUserId { get; set; }
    public int? AssigneeUserId { get; set; }
    public DateOnly? RetakeDate { get; set; }
    public TimeOnly? RetakeTime { get; set; }
    public string? Audience { get; set; }
    public Discipline Discipline { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
    public User? AssigneeUser { get; set; }
}
