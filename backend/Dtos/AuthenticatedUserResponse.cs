namespace backend.Dtos;

public sealed record AuthenticatedUserResponse(
    int Id,
    string Login,
    string FullName,
    string Role);
