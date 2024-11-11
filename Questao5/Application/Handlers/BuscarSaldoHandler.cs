using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Extensions;
using Questao5.Domain.Interfaces.Services;

namespace Questao5.Application.Handlers;

public class BuscarSaldoHandler : IRequestHandler<BuscarSaldoQueryParams, BuscarSaldoQueryResponse>
{
    private readonly ILogger<BuscarSaldoHandler> _logger;
    private readonly IMovimentoService _movimentoService;
    private readonly IContaCorrenteService _contaCorrenteService;

    public BuscarSaldoHandler(IMovimentoService movimentoService, IContaCorrenteService contaCorrenteService,
        ILogger<BuscarSaldoHandler> logger)
    {
        _logger = logger;
        _movimentoService = movimentoService;
        _contaCorrenteService = contaCorrenteService;
    }

    public async Task<BuscarSaldoQueryResponse> Handle(BuscarSaldoQueryParams request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling request: {RequestId}", cancellationToken);

        var contaCorrente = await _contaCorrenteService.BuscarContaCorrente(request.NumeroContaCorrente);

        var movimentos = await _movimentoService.BuscarMovimentacoes(contaCorrente.Id);

        var saldo = movimentos.Where(m => m.TipoMovimento == TipoMovimentoEnum.Credito.GetDescription()).Sum(x => x.Valor) -
                    movimentos.Where(m => m.TipoMovimento == TipoMovimentoEnum.Debito.GetDescription()).Sum(x => x.Valor);

        _logger.LogInformation("Movimentação criada com sucesso para a requisição: {RequestId}", cancellationToken);

        return new BuscarSaldoQueryResponse
        {
            Saldo = saldo,
            NomeConta = contaCorrente.Nome,
            NumeroConta = contaCorrente.Numero
        };
    }
}