using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinancialFlow.Application.UseCases.Transactions.Commands.RegisterTransaction;

namespace FinancialFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public sealed class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Registra uma nova transação financeira
    /// </summary>
    /// <param name="command">Dados da transação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>ID da transação criada</returns>
    /// <response code="201">Transação criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="401">Não autorizado</response>
    /// <response code="422">Erro de validação</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RegisterTransaction(
        [FromBody] RegisterTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsSuccess)
        {
            return CreatedAtAction(
                nameof(GetTransactionById),
                new { id = result.Value },
                new { id = result.Value });
        }

        return UnprocessableEntity(new { error = result.Error });
    }

    /// <summary>
    /// Obtém uma transação por ID (placeholder para CreatedAtAction)
    /// </summary>
    /// <param name="id">ID da transação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados da transação</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransactionById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        // TODO: Implementar query para buscar transação por ID
        return Ok(new { id, message = "Endpoint a ser implementado" });
    }
}