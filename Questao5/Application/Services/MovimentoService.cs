using Newtonsoft.Json;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.QueryStore;
using Questao5.Domain.Interfaces.Services;

namespace Questao5.Application.Services;

public class MovimentoService : IMovimentoService
{
    private readonly IMovimentoCommand _movimentoCommand;
    private readonly IMovimentoQuery _movimentoQuery;
    private readonly ILogger<MovimentoService> _logger;

    public MovimentoService(IMovimentoCommand movimentoCommand, IMovimentoQuery movimentoQuery,
        ILogger<MovimentoService> logger)
    {
        _movimentoCommand = movimentoCommand;
        _movimentoQuery = movimentoQuery;
        _logger = logger;
    }

    public async Task<MovimentoEntity> CriarMovimento(MovimentoEntity movimento)
    {
        _logger.LogInformation("Criando movimento: {MovimentoId}", movimento.Id);

        await _movimentoCommand.CriarMovimento(movimento);

        _logger.LogInformation("Movimento criado com sucesso: {MovimentoId}", movimento.Id);

        return movimento;
    }

    public async Task<IEnumerable<MovimentoEntity>> BuscarMovimentacoes(string codigoContaCorrente)
    {
        _logger.LogInformation("Buscando movimento da conta: {codigoContaCorrente}", codigoContaCorrente);

        var movimentacoes = await _movimentoQuery.ListarPorConta(codigoContaCorrente);
        
        _logger.LogInformation("Movimentos encontrados: {Movimentos}", JsonConvert.SerializeObject(movimentacoes));

        return movimentacoes;
    }
}