using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.QueryStore;

public interface IIdempotenciaCommand
{
    Task CriarIdempotencia(IdempotenciaEntity idempotencia);
    Task AtualizarIdempotencia(IdempotenciaEntity idempotencia);
}