namespace backend.Models;

public sealed class User
{
    public int Id { get; set; }
    public required string Login { get; set; }
    public required string FullName { get; set; }
    public required string Role { get; set; }
    public required string PasswordHash { get; set; }
    public ICollection<RetakeRequest> CreatedRequests { get; set; } = [];
    public ICollection<RetakeRequest> AssignedRequests { get; set; } = [];
}
