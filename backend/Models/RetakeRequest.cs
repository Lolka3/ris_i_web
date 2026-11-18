namespace backend.Models;

public sealed class RetakeRequest
{
    public int Id { get; set; }
    public int Number { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public int DisciplineId { get; set; }
    public string Status { get; set; } = "New";
    public int? AssigneeUserId { get; set; }
    public DateOnly? RetakeDate { get; set; }
    public TimeOnly? RetakeTime { get; set; }
    public string? Audience { get; set; }
}
