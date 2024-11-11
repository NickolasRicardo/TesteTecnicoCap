using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.QueryStore;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore;

public class ContaCorrenteQuery : IContaCorrenteQuery
{
    private readonly DatabaseConfig _databaseConfig;

    public ContaCorrenteQuery(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public async Task<IEnumerable<ContaCorrenteEntity>> ListarTodas()
    {
        await using (var connection = new SqliteConnection(_databaseConfig.Name))
        {
            var resultado = await connection.QueryAsync<ContaCorrenteEntity>(BuscarContaCorrente);

            return resultado;
        }
    }

    public async Task<IEnumerable<ContaCorrenteEntity>> ListarPorStatusAtivo(bool status)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("Ativo", status);

        await using (var connection = new SqliteConnection(_databaseConfig.Name))
        {
            var resultado = await connection.QueryAsync<ContaCorrenteEntity>(BuscarPorStatus, parametros);

            return resultado;
        }
    }

    public async Task<ContaCorrenteEntity> BuscarId(string Id)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("id", Id);

        await using (var connection = new SqliteConnection(_databaseConfig.Name))
        {
            var resultado = await connection.QueryFirstOrDefaultAsync<ContaCorrenteEntity>(BuscarPorId, parametros);

            return resultado;
        }
    }

    public async Task<ContaCorrenteEntity> BuscarNumero(long numero)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("numero", numero);

        await using (var connection = new SqliteConnection(_databaseConfig.Name))
        {
            var resultado = await connection.QueryFirstOrDefaultAsync<ContaCorrenteEntity>(BuscarPorNumero, parametros);

            return resultado;
        }
    }

    public async Task<ContaCorrenteEntity> BuscarNome(string nome)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("nome", nome);

        await using (var connection = new SqliteConnection(_databaseConfig.Name))
        {
            var resultado = await connection.QueryFirstOrDefaultAsync<ContaCorrenteEntity>(BuscarPorNome, parametros);

            return resultado;
        }
    }

    #region Sql

    private static string BuscarContaCorrente = @"
        SELECT 
              idcontacorrente   AS Id,
              numero            AS Numero,
              nome              AS Nome,
              ativo             AS Ativo
        FROM 
            contacorrente
    ";

    private static string BuscarPorStatus = BuscarContaCorrente + @"
        WHERE
             Ativo = @status
    ";

    private static string BuscarPorId = BuscarContaCorrente + @"
        WHERE
             Id = @id
    ";

    private static string BuscarPorNumero = BuscarContaCorrente + @"
        WHERE
             numero = @numero
    ";

    private static string BuscarPorNome = BuscarContaCorrente + @"
        WHERE
             Nome = @nome
    ";

    #endregion
}