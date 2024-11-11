using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Response;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Questao5.Infrastructure.Services.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimentoController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovimentoController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    
    /// <summary>
    ///  Buscar saldo
    /// </summary>
    /// <response code="200">Sucesso ao buscar saldo.</response>
    /// <response code="400" >Erro ao buscar saldo.</response>
    /// <response code="500">Aconteceu uma exceção não tratada ao buscar saldo.</response>
    [HttpPost("Saldo/{NumeroContaCorrente}")]
    public async Task<ActionResult<BuscarSaldoQueryResponse>> BuscarMovimentacao([FromRoute] BuscarSaldoQueryParams request)
    {
        try
        {
            request.Validate();
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    
    /// <summary>
    ///  Criar movimentação
    /// </summary>
    /// <response code="200">Sucesso ao gerar movimentação.</response>
    /// <response code="400" >Erro ao gerar movimentação.</response>
    /// <response code="500">Aconteceu uma exceção não tratada ao gerar movimentação.</response>
    [HttpPost("Registrar")]
    public async Task<ActionResult<CriarMovimentacaoCommandResponse>> CriarMovimentacao([FromBody] CriarMovimentacaoCommandRequest request)
    {
        try
        {
            request.Validate();
            var response = await _mediator.Send(request);
            return Ok(response);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}