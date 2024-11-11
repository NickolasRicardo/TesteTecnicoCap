using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.QueryStore;

public interface IMovimentoQuery
{
    Task<IEnumerable<MovimentoEntity>> ListarTodos();
    Task<IEnumerable<MovimentoEntity>>  ListarPorConta(string conta);
    Task<IEnumerable<MovimentoEntity>>  ListarPorData(DateTime data);
}