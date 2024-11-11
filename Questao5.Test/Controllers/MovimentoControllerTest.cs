using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Response;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Services.Controllers;


namespace Questao5.Test.Controllers;
public class MovimentoControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly MovimentoController _controller;

    public MovimentoControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new MovimentoController(_mediatorMock.Object);
    }

    [Fact]
    public async Task BuscarMovimentacao_ReturnsOk_WhenRequestIsValid()
    {
        var request = new BuscarSaldoQueryParams { NumeroContaCorrente = 12345 };
        var response = new BuscarSaldoQueryResponse { Saldo = 100.0 };

        _mediatorMock.Setup(m => m.Send(It.IsAny<BuscarSaldoQueryParams>(), default)).ReturnsAsync(response);

        var result = await _controller.BuscarMovimentacao(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task BuscarMovimentacao_ReturnsBadRequest_WhenValidationExceptionIsThrown()
    {
        var request = new BuscarSaldoQueryParams { NumeroContaCorrente = 12345 };

        _mediatorMock.Setup(m => m.Send(It.IsAny<BuscarSaldoQueryParams>(), default)).ThrowsAsync(new ValidationException("Validation error"));

        var result = await _controller.BuscarMovimentacao(request);

        var badRequestResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal("Validation error", badRequestResult.Value);
    }

    [Fact]
    public async Task CriarMovimentacao_ReturnsOk_WhenRequestIsValid()
    {
        var request = new CriarMovimentacaoCommandRequest { NumeroContaCorrente = 12345, Valor = 100.0, RequestId = Guid.NewGuid().ToString(), TipoMovimento = "C" };

        var response = new CriarMovimentacaoCommandResponse { MovimentoId = Guid.NewGuid() };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CriarMovimentacaoCommandRequest>(), default)).ReturnsAsync(response);

        var result = await _controller.CriarMovimentacao(request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(response, okResult.Value);
    }

    [Fact]
    public async Task CriarMovimentacao_ReturnsBadRequest_WhenValidationExceptionIsThrown()
    {
        var request = new CriarMovimentacaoCommandRequest { NumeroContaCorrente = 12345, Valor = 100.0, RequestId= Guid.NewGuid().ToString(), TipoMovimento = "C" };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CriarMovimentacaoCommandRequest>(), default)).ThrowsAsync(new ValidationException("Validation error"));

        var result = await _controller.CriarMovimentacao(request);

        var badRequestResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal("Validation error", badRequestResult.Value);
    }
}