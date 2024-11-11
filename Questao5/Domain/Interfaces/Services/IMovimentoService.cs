using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.Services;

public interface IMovimentoService
{
    Task<MovimentoEntity> CriarMovimento(MovimentoEntity movimento);

    Task<IEnumerable<MovimentoEntity>> BuscarMovimentacoes(string CodigoContaCorrente);
}