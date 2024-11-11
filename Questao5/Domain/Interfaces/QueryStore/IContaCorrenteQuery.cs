using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces.QueryStore;

public interface IContaCorrenteQuery
{
    Task<IEnumerable<ContaCorrenteEntity>> ListarTodas();
    Task<IEnumerable<ContaCorrenteEntity>>  ListarPorStatusAtivo(bool status);
    Task<ContaCorrenteEntity>  BuscarId(string id);
    Task<ContaCorrenteEntity>  BuscarNumero(long numero);
    Task<ContaCorrenteEntity>  BuscarNome(string nome);


}