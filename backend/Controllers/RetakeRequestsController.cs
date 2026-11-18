using backend.Dtos;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/retake-requests")]
public sealed class RetakeRequestsController : ControllerBase
{
    private readonly RetakeRequestService _service;

    public RetakeRequestsController(RetakeRequestService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<RetakeRequestResponse>> GetAll() =>
        Ok(_service.GetAll());

    [HttpGet("{id:int}")]
    public ActionResult<RetakeRequestResponse> GetById(int id)
    {
        var request = _service.GetById(id);
        return request is null
            ? NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Заявка не найдена"
            })
            : Ok(request);
    }

    [HttpPost]
    public ActionResult<RetakeRequestResponse> Create(CreateRetakeRequestRequest input)
    {
        var created = _service.Create(input);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id:int}/assignee")]
    public IActionResult Assign(int id, AssignRetakeRequestRequest input, [FromHeader(Name = "X-User-Id")] int? currentUserId)
    {
        if (currentUserId is null or <= 0)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Укажите корректный заголовок X-User-Id"
            });
        }

        return ToActionResult(id, _service.Assign(id, input.AssigneeUserId, currentUserId.Value));
    }

    [HttpPatch("{id:int}/status")]
    public IActionResult ChangeStatus(
        int id,
        ChangeRetakeRequestStatusRequest input,
        [FromHeader(Name = "X-User-Id")] int? currentUserId)
    {
        if (currentUserId is null or <= 0)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Укажите корректный заголовок X-User-Id"
            });
        }

        return ToActionResult(id, _service.ChangeStatus(
            id,
            input.Status,
            currentUserId.Value,
            input.RetakeDate,
            input.RetakeTime,
            input.Audience));
    }

    private IActionResult ToActionResult(int id, RequestChangeResult result) =>
        result switch
        {
            RequestChangeResult.Success => Ok(_service.GetById(id)),
            RequestChangeResult.NotFound => NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Заявка не найдена"
            }),
            RequestChangeResult.Conflict => Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Действие противоречит текущему состоянию заявки"
            }),
            _ => throw new InvalidOperationException("Неизвестный результат изменения заявки")
        };
}
