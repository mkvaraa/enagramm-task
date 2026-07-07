using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MoneyTransfer.Application.DTOs;
using MoneyTransfer.Application.Interfaces;

namespace MoneyTransfer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransfersController : ControllerBase
{
    private readonly ITransferService _service;
    private readonly IValidator<TransferRequest> _validator;

    public TransfersController(ITransferService service, IValidator<TransferRequest> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest request, CancellationToken ct)
    {
        var validation = await _validator.ValidateAsync(request, ct);
        if (!validation.IsValid)
        {
            return BadRequest(new
            {
                error = "VALIDATION_FAILED",
                message = "Request is invalid.",
                details = validation.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            });
        }

        var result = await _service.TransferAsync(request, ct);

        if (result.IsSuccess)
            return Ok(new { success = true, transaction = result.Value });

        return UnprocessableEntity(new
        {
            success = false,
            error = result.ErrorCode,
            message = result.ErrorMessage
        });
    }
}