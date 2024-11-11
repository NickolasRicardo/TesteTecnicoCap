using System.Data;
using System.Globalization;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Interfaces.QueryStore;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore;

public class MovimentoQuery : IMovimentoQuery
{
    private readonly DatabaseConfig _databaseConfig;

    public MovimentoQuery(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public async Task<IEnumerable<MovimentoEntity>> ListarTodos()
    {
        await using (var connection = new SqliteConnection(_databaseConfig.Name))
        {
            var resultado = await connection.QueryAsync<MovimentoEntity>(BuscarMovimento);

            return resultado;
        }
    }

    public async Task<IEnumerable<MovimentoEntity>> ListarPorConta(string conta)
    {
        try
        {
            DynamicParameters parametros = new DynamicParameters();

            parametros.Add("IdContaCorrente", conta);

            await using (var connection = new SqliteConnection(_databaseConfig.Name))
            {
                var resultado = await connection.QueryAsync<MovimentoEntity>(BuscarPorConta, parametros);

                return resultado;
            }
        }
        catch(Exception ex){
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<MovimentoEntity>> ListarPorData(DateTime data)
    {
        DynamicParameters parametros = new DynamicParameters();

        parametros.Add("DataMovimento", data);

        await using (var connection = new SqliteConnection(_databaseConfig.Name))
        {
            var resultado = await connection.QueryAsync<MovimentoEntity>(BuscarPorData, parametros);

            return resultado;
        }
    }
    
    #region Sql

    private static string BuscarMovimento = @"
        SELECT 
              idmovimento        AS Id,
              idcontacorrente    AS IdContaCorrente,
              datamovimento      AS DataMovimento,
              tipomovimento      AS TipoMovimento,
              valor              AS Valor
        FROM 
            Movimento
    ";

    private static string BuscarPorConta = BuscarMovimento + @"
        WHERE
             idcontacorrente = @IdContaCorrente
    ";

    private static string BuscarPorData = BuscarMovimento + @"
        WHERE
             datamovimento = @DataMovimento
    ";

    #endregion
}