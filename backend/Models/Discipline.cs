namespace backend.Models;

public sealed class Discipline
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<RetakeRequest> RetakeRequests { get; set; } = [];
}
