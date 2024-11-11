using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.QueryStore;
using Questao5.Domain.Interfaces.Services;

namespace Questao5.Application.Services;

public class IdempotenciaService : IIdempotenciaService
{
    private readonly IIdempotenciaQuery _idempotenciaQuery;
    private readonly IIdempotenciaCommand _idempotenciaCommand;
    private readonly ILogger<IdempotenciaService> _logger;

    public IdempotenciaService(ILogger<IdempotenciaService> logger, IIdempotenciaCommand idempotenciaCommand,
        IIdempotenciaQuery idempotenciaQuery)
    {
        _logger = logger;
        _idempotenciaCommand = idempotenciaCommand;
        _idempotenciaQuery = idempotenciaQuery;
    }

    public async Task ValidarDuplicidadeRequisicao(string requisicaoId)
    {
        var idempotencia = await _idempotenciaQuery.BuscarIdempotenciaPorId(requisicaoId);

        if (idempotencia is not null)
        {
            _logger.LogWarning("Requisição já realizada: {RequestId}", requisicaoId);

            throw new InvalidOperationException(idempotencia.Resultado);
        }
    }

    public async Task CriarIdempotencia(IdempotenciaEntity entidade)
    {
        await _idempotenciaCommand.CriarIdempotencia(entidade);
    }
}