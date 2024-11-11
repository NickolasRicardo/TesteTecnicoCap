using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.QueryStore;

public interface IContaCorrenteCommand
{
    Task CriarContaCorrente(ContaCorrenteEntity contaCorrente);
    Task AtualizarContaCorrente(ContaCorrenteEntity contaCorrente);
    Task DeletarContaCorrente(ContaCorrenteEntity contaCorrente);
}