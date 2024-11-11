using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.QueryStore;

public interface IIdempotenciaQuery
{
    Task<IEnumerable<IdempotenciaEntity>> ListarTodasIdempotencias();
    Task<IdempotenciaEntity?> BuscarIdempotenciaPorId(string Id);
}