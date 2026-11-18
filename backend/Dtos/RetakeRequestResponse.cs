namespace backend.Dtos;

public sealed record RetakeRequestResponse(
    int Id,
    int Number,
    string Title,
    int DisciplineId,
    string Description,
    string Status,
    int? AssigneeUserId,
    DateOnly? RetakeDate,
    TimeOnly? RetakeTime,
    string? Audience);
