using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Response;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Services;
using Xunit;

namespace Questao5.Test.Handlers
{
    public class CriarMovimentacaoHandlerTests
    {
        private readonly Mock<ILogger<CriarMovimentacaoHandler>> _loggerMock;
        private readonly Mock<IMovimentoService> _movimentoServiceMock;
        private readonly Mock<IContaCorrenteService> _contaCorrenteServiceMock;
        private readonly Mock<IIdempotenciaService> _idempotenciaServiceMock;

        private readonly CriarMovimentacaoHandler _handler;

        public CriarMovimentacaoHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CriarMovimentacaoHandler>>();
            _movimentoServiceMock = new Mock<IMovimentoService>();
            _contaCorrenteServiceMock = new Mock<IContaCorrenteService>();
            _idempotenciaServiceMock = new Mock<IIdempotenciaService>();

            _handler = new CriarMovimentacaoHandler(
                _movimentoServiceMock.Object,
                _contaCorrenteServiceMock.Object,
                _loggerMock.Object,
                _idempotenciaServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsResponse()
        {
            // Arrange
            var request = new CriarMovimentacaoCommandRequest
            {
                RequestId = Guid.NewGuid().ToString(),
                NumeroContaCorrente = 123456,
                TipoMovimento = "Deposito",
                Valor = 100.00
            };

            var contaCorrente = new ContaCorrenteEntity
            {
                Id = Guid.NewGuid().ToString(),
                Numero = 123456,
                Nome = "Teste da silva",
                Ativo = 1
            };

            var movimento = new MovimentoEntity
            {
                Id = Guid.NewGuid().ToString(),
                IdContaCorrente = contaCorrente.Id,
                DataMovimento = DateTime.Now,
                TipoMovimento = request.TipoMovimento,
                Valor = request.Valor
            };

            var idempotenciaEntity = new IdempotenciaEntity
            {
                Id = request.RequestId,
                Requisicao = JsonConvert.SerializeObject(request),
                Resultado = JsonConvert.SerializeObject(movimento)
            };

            _contaCorrenteServiceMock.Setup(x => x.BuscarContaCorrente(request.NumeroContaCorrente))
                .ReturnsAsync(contaCorrente);

            _movimentoServiceMock.Setup(x => x.CriarMovimento(It.IsAny<MovimentoEntity>()))
                .ReturnsAsync(movimento);

            _idempotenciaServiceMock.Setup(x => x.CriarIdempotencia(It.IsAny<IdempotenciaEntity>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(Guid.Parse(movimento.Id), response.MovimentoId);
        }

        // Add more test cases as needed
    }
}
