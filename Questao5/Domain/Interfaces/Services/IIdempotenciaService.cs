using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.Services;

public interface IIdempotenciaService
{
    Task ValidarDuplicidadeRequisicao(string requisicaoId);

    Task CriarIdempotencia(IdempotenciaEntity entidade);
}