using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.QueryStore;

public interface IMovimentoCommand
{
    Task CriarMovimento(MovimentoEntity movimento);
    Task AtualizarMovimento(MovimentoEntity movimento);
    
}