using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Interfaces.Services;
using Xunit;
using Questao5.Domain.Extensions;

namespace Questao5.Test
{
    public class BuscarSaldoHandlerTests
    {
        private readonly Mock<ILogger<BuscarSaldoHandler>> _loggerMock;
        private readonly Mock<IMovimentoService> _movimentoServiceMock;
        private readonly Mock<IContaCorrenteService> _contaCorrenteServiceMock;

        private readonly BuscarSaldoHandler _handler;

        public BuscarSaldoHandlerTests()
        {
            _loggerMock = new Mock<ILogger<BuscarSaldoHandler>>();
            _movimentoServiceMock = new Mock<IMovimentoService>();
            _contaCorrenteServiceMock = new Mock<IContaCorrenteService>();

            _handler = new BuscarSaldoHandler(
                _movimentoServiceMock.Object,
                _contaCorrenteServiceMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsResponse()
        {
            // Arrange
            var request = new BuscarSaldoQueryParams
            {
                NumeroContaCorrente = 123456
            };

            var contaCorrente = new ContaCorrenteEntity
            {
                Id = Guid.NewGuid().ToString(),
                Numero = 123456,
                Nome = "Teste da silva",
                Ativo = 1
            };

            var movimentos = new[]
            {
                new MovimentoEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    IdContaCorrente = contaCorrente.Id,
                    DataMovimento = DateTime.Now,
                    TipoMovimento = TipoMovimentoEnum.Credito.GetDescription(),
                    Valor = 100.00
                },
                new MovimentoEntity
                {
                    Id = Guid.NewGuid().ToString(),
                    IdContaCorrente = contaCorrente.Id,
                    DataMovimento = DateTime.Now,
                    TipoMovimento = TipoMovimentoEnum.Debito.GetDescription(),
                    Valor = 50.00
                }
            };

            _contaCorrenteServiceMock.Setup(x => x.BuscarContaCorrente(request.NumeroContaCorrente))
                .ReturnsAsync(contaCorrente);

            _movimentoServiceMock.Setup(x => x.BuscarMovimentacoes(contaCorrente.Id))
                .ReturnsAsync(movimentos);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(50.00, response.Saldo);
            Assert.Equal(contaCorrente.Nome, response.NomeConta);
            Assert.Equal(contaCorrente.Numero, response.NumeroConta);

        }

    }
}
