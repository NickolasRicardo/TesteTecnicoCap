using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.QueryStore;
using Questao5.Domain.Interfaces.Services;

namespace Questao5.Application.Services;

public class ContaCorrenteService : IContaCorrenteService
{
    private readonly IContaCorrenteQuery _contaCorrenteQuery;
    private readonly ILogger<ContaCorrenteService> _logger;

    public ContaCorrenteService(IContaCorrenteQuery contaCorrenteQuery, ILogger<ContaCorrenteService> logger)
    {
        _contaCorrenteQuery = contaCorrenteQuery;
        _logger = logger;
    }

    public async Task<ContaCorrenteEntity> BuscarContaCorrente(long numeroContaCorrente)
    {
        _logger.LogInformation("Validando conta corrente: {NumeroContaCorrente}", numeroContaCorrente);

        var contaCorrente = await _contaCorrenteQuery.BuscarNumero(numeroContaCorrente);

        if (contaCorrente == null)
        {
            _logger.LogError("Conta corrente não encontrada: {NumeroContaCorrente}", numeroContaCorrente);
            throw new InvalidOperationException("Conta corrente não encontrada.");
        }

        if (contaCorrente.Ativo == 0)
        {
            _logger.LogError("Conta corrente inativa: {NumeroContaCorrente}", numeroContaCorrente);
            throw new InvalidOperationException("Conta corrente inativa.");
        }

        _logger.LogInformation("Conta corrente validada com sucesso: {NumeroContaCorrente}", numeroContaCorrente);

        return contaCorrente;
    }
}