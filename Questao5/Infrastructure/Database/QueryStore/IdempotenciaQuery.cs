using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.QueryStore;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore;

public class IdempotenciaQuery : IIdempotenciaQuery
{
    private readonly DatabaseConfig _databaseConfig;

    public IdempotenciaQuery(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public async Task<IEnumerable<IdempotenciaEntity>> ListarTodasIdempotencias()
    {
        await using (var connection = new SqliteConnection(_databaseConfig.Name))
        {
            var resultado = await connection.QueryAsync<IdempotenciaEntity>(BuscarIdempotencia);

            return resultado;
        }
    }

    public async Task<IdempotenciaEntity?> BuscarIdempotenciaPorId(string Id)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("id", Id);

        await using (var connection = new SqliteConnection(_databaseConfig.Name))
        {
            var resultado = await connection.QueryFirstOrDefaultAsync<IdempotenciaEntity>(BuscarPorId, parametros);

            return resultado;
        }
    }


    #region Sql

    private static string BuscarIdempotencia = @"
        SELECT 
             chave_idempotencia  AS Id,
             Requisicao          AS Requisicao,
             Resultado           AS Resultado
        FROM 
            idempotencia
    ";

    private static string BuscarPorId = BuscarIdempotencia + @"
        WHERE
             Id = @id
    ";

    #endregion
}