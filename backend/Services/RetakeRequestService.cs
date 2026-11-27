using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public enum RequestChangeResult
{
    Success,
    NotFound,
    Conflict,
    InvalidReference,
    Forbidden
}

public sealed class RetakeRequestService(AppDbContext dbContext)
{
    public async Task<IReadOnlyList<RetakeRequestResponse>> GetAllAsync(
        CancellationToken cancellationToken = default) =>
        await dbContext.RetakeRequests
            .AsNoTracking()
            .OrderBy(item => item.Id)
            .Select(item => ToResponse(item))
            .ToArrayAsync(cancellationToken);

    public async Task<RetakeRequestResponse?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var request = await dbContext.RetakeRequests
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
        return request is null ? null : ToResponse(request);
    }

    public async Task<(RequestChangeResult Result, RetakeRequestResponse? Request)> CreateAsync(
        CreateRetakeRequestRequest input,
        int createdByUserId,
        CancellationToken cancellationToken = default)
    {
        if (!await dbContext.Disciplines.AnyAsync(
                item => item.Id == input.DisciplineId, cancellationToken) ||
            !await dbContext.Users.AnyAsync(
                item => item.Id == createdByUserId, cancellationToken))
        {
            return (RequestChangeResult.InvalidReference, null);
        }

        var request = new RetakeRequest
        {
            Title = input.Title.Trim(),
            DisciplineId = input.DisciplineId,
            Description = input.Description.Trim(),
            CreatedByUserId = createdByUserId
        };
        dbContext.RetakeRequests.Add(request);
        await dbContext.SaveChangesAsync(cancellationToken);

        return (RequestChangeResult.Success, ToResponse(request));
    }

    public async Task<(RequestChangeResult Result, RetakeRequestResponse? Request)> AssignAsync(
        int id,
        int assigneeUserId,
        CancellationToken cancellationToken = default)
    {
        var request = await dbContext.RetakeRequests
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (request is null)
        {
            return (RequestChangeResult.NotFound, null);
        }

        if (request.Status != "New" ||
            request.AssigneeUserId is not null && request.AssigneeUserId != assigneeUserId)
        {
            return (RequestChangeResult.Conflict, null);
        }

        var assignee = await dbContext.Users
            .SingleOrDefaultAsync(item => item.Id == assigneeUserId, cancellationToken);
        if (assignee is null)
        {
            return (RequestChangeResult.InvalidReference, null);
        }

        if (assignee.Role != "Teacher")
        {
            return (RequestChangeResult.Forbidden, null);
        }

        request.AssigneeUserId = assigneeUserId;
        await dbContext.SaveChangesAsync(cancellationToken);
        return (RequestChangeResult.Success, ToResponse(request));
    }

    public async Task<(RequestChangeResult Result, RetakeRequestResponse? Request)> ChangeStatusAsync(
        int id,
        string status,
        int currentUserId,
        DateOnly? retakeDate,
        TimeOnly? retakeTime,
        string? audience,
        CancellationToken cancellationToken = default)
    {
        var request = await dbContext.RetakeRequests
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (request is null)
        {
            return (RequestChangeResult.NotFound, null);
        }

        if (request.AssigneeUserId != currentUserId)
        {
            return (RequestChangeResult.Forbidden, null);
        }

        switch (status)
        {
            case "InProgress" when request.Status == "New":
                request.RetakeDate = retakeDate;
                request.RetakeTime = retakeTime;
                request.Audience = audience?.Trim();
                break;
            case "Closed" when request.Status == "InProgress":
                break;
            case "Cancelled" when request.Status is "New" or "InProgress":
                break;
            default:
                return (RequestChangeResult.Conflict, null);
        }

        request.Status = status;
        await dbContext.SaveChangesAsync(cancellationToken);
        return (RequestChangeResult.Success, ToResponse(request));
    }

    private static RetakeRequestResponse ToResponse(RetakeRequest request) =>
        new(
            request.Id,
            request.Number,
            request.Title,
            request.DisciplineId,
            request.Description,
            request.Status,
            request.AssigneeUserId,
            request.RetakeDate,
            request.RetakeTime,
            request.Audience);
}
