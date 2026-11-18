using backend.Dtos;
using backend.Models;

namespace backend.Services;

public enum RequestChangeResult
{
    Success,
    NotFound,
    Conflict
}

public sealed class RetakeRequestService
{
    private readonly object _lock = new();
    private readonly List<RetakeRequest> _requests = [];
    private int _nextId = 1;
    private int _nextNumber = 1;

    public IReadOnlyList<RetakeRequestResponse> GetAll()
    {
        lock (_lock)
        {
            return _requests.Select(ToResponse).ToArray();
        }
    }

    public RetakeRequestResponse? GetById(int id)
    {
        lock (_lock)
        {
            var request = _requests.Find(item => item.Id == id);
            return request is null ? null : ToResponse(request);
        }
    }

    public RetakeRequestResponse Create(CreateRetakeRequestRequest input)
    {
        lock (_lock)
        {
            var request = new RetakeRequest
            {
                Id = _nextId++,
                Number = _nextNumber++,
                Title = input.Title.Trim(),
                DisciplineId = input.DisciplineId,
                Description = input.Description.Trim()
            };
            _requests.Add(request);
            return ToResponse(request);
        }
    }

    public RequestChangeResult Assign(int id, int assigneeUserId, int currentUserId)
    {
        lock (_lock)
        {
            var request = _requests.Find(item => item.Id == id);
            if (request is null)
            {
                return RequestChangeResult.NotFound;
            }

            if (assigneeUserId != currentUserId ||
                request.Status != "New" ||
                request.AssigneeUserId is not null && request.AssigneeUserId != assigneeUserId)
            {
                return RequestChangeResult.Conflict;
            }

            request.AssigneeUserId = assigneeUserId;
            return RequestChangeResult.Success;
        }
    }

    public RequestChangeResult ChangeStatus(
        int id,
        string status,
        int currentUserId,
        DateOnly? retakeDate,
        TimeOnly? retakeTime,
        string? audience)
    {
        lock (_lock)
        {
            var request = _requests.Find(item => item.Id == id);
            if (request is null)
            {
                return RequestChangeResult.NotFound;
            }

            if (request.AssigneeUserId != currentUserId)
            {
                return RequestChangeResult.Conflict;
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
                    return RequestChangeResult.Conflict;
            }

            request.Status = status;
            return RequestChangeResult.Success;
        }
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
