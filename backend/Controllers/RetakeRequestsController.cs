using backend.Dtos;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/retake-requests")]
public sealed class RetakeRequestsController : ControllerBase
{
    private readonly RetakeRequestService _service;

    public RetakeRequestsController(RetakeRequestService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RetakeRequestResponse>>> GetAll(
        CancellationToken cancellationToken) =>
        Ok(await _service.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RetakeRequestResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var request = await _service.GetByIdAsync(id, cancellationToken);
        return request is null
            ? NotFound(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Заявка не найдена"
            })
            : Ok(request);
    }

    [HttpPost]
    public async Task<ActionResult<RetakeRequestResponse>> Create(
        CreateRetakeRequestRequest input,
        CancellationToken cancellationToken)
    {
        var (result, created) = await _service.CreateAsync(input, cancellationToken);
        if (result == RequestChangeResult.InvalidReference)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Указана несуществующая дисциплина или не настроен пользователь-создатель"
            });
        }

        return CreatedAtAction(nameof(GetById), new { id = created!.Id }, created);
    }

    [HttpPatch("{id:int}/assignee")]
    public async Task<IActionResult> Assign(
        int id,
        AssignRetakeRequestRequest input,
        [FromHeader(Name = "X-User-Id")] int? currentUserId,
        CancellationToken cancellationToken)
    {
        if (currentUserId is null or <= 0)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Укажите корректный заголовок X-User-Id"
            });
        }

        var result = await _service.AssignAsync(
            id, input.AssigneeUserId, currentUserId.Value, cancellationToken);
        return ToActionResult(result);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> ChangeStatus(
        int id,
        ChangeRetakeRequestStatusRequest input,
        [FromHeader(Name = "X-User-Id")] int? currentUserId,
        CancellationToken cancellationToken)
    {
        if (currentUserId is null or <= 0)
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Укажите корректный заголовок X-User-Id"
            });
        }

        var result = await _service.ChangeStatusAsync(
            id,
            input.Status,
            currentUserId.Value,
            input.RetakeDate,
            input.RetakeTime,
            input.Audience,
            cancellationToken);
        return ToActionResult(result);
    }

    private IActionResult ToActionResult(
        (RequestChangeResult Result, RetakeRequestResponse? Request) result) =>
        result.Result switch
        {
            RequestChangeResult.Success => Ok(result.Request),
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
            RequestChangeResult.InvalidReference => BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Указан несуществующий пользователь"
            }),
            _ => throw new InvalidOperationException("Неизвестный результат изменения заявки")
        };
}
