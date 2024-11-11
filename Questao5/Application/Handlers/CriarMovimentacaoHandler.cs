using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Response;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.Services;

namespace Questao5.Application.Handlers;

public class
    CriarMovimentacaoHandler : IRequestHandler<CriarMovimentacaoCommandRequest, CriarMovimentacaoCommandResponse>
{
    private readonly ILogger<CriarMovimentacaoHandler> _logger;
    private readonly IMovimentoService _movimentoService;
    private readonly IContaCorrenteService _contaCorrenteService;
    private readonly IIdempotenciaService _idempotenciaService;

    public CriarMovimentacaoHandler(IMovimentoService movimentoService, IContaCorrenteService contaCorrenteService,
        ILogger<CriarMovimentacaoHandler> logger, IIdempotenciaService idempotenciaService)
    {
        _logger = logger;
        _movimentoService = movimentoService;
        _idempotenciaService = idempotenciaService;
        _contaCorrenteService = contaCorrenteService;
    }

    public async Task<CriarMovimentacaoCommandResponse> Handle(CriarMovimentacaoCommandRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling request: {RequestId}", request.RequestId);

        await _idempotenciaService.ValidarDuplicidadeRequisicao(request.RequestId);
        var contaCorrente = await _contaCorrenteService.BuscarContaCorrente(request.NumeroContaCorrente);

        var movimento = new MovimentoEntity
        {
            IdContaCorrente = contaCorrente.Id,
            DataMovimento = DateTime.Now,
            TipoMovimento = request.TipoMovimento,
            Valor = request.Valor
        };

        movimento = await _movimentoService.CriarMovimento(movimento);

        var idempotenciaEntity = new IdempotenciaEntity
        {
            Id = request.RequestId,
            Requisicao = JsonConvert.SerializeObject(request),
            Resultado = JsonConvert.SerializeObject(movimento)
        };

        await _idempotenciaService.CriarIdempotencia(idempotenciaEntity);

        _logger.LogInformation("Movimentação criada com sucesso para a requisição: {RequestId}", request.RequestId);

        return new CriarMovimentacaoCommandResponse
        {
            MovimentoId = Guid.Parse(movimento.Id)
        };
    }
}