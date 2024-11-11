using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.QueryStore;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore;
//todo terminar de criar as classes do banco e iniciar handlers.
public class IdempotenciaCommand : IIdempotenciaCommand
{
    private readonly DatabaseConfig _databaseConfig;

    public IdempotenciaCommand(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }
    
    public async Task CriarIdempotencia(IdempotenciaEntity idempotencia)
    {
        try
        {
            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("Id", idempotencia.Id);
            parameters.Add("Requisicao", idempotencia.Requisicao);
            parameters.Add("Resultado", idempotencia.Resultado);
            
            using (var connection = new SqliteConnection(_databaseConfig.Name))
            {
               await connection.ExecuteAsync(Insert, parameters);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao criar idempotencia", ex);
        }
    }
    
    public async Task AtualizarIdempotencia(IdempotenciaEntity idempotencia)
    {
        try
        {
            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("Id", idempotencia.Id);
            parameters.Add("Requisicao", idempotencia.Requisicao);
            parameters.Add("Resultado", idempotencia.Resultado);
            
            using (var connection = new SqliteConnection(_databaseConfig.Name))
            {
               await connection.ExecuteAsync(Update, parameters);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao atualizar idempotencia", ex);
        }
    }
    


    #region Sql

    private static string Insert = @"
        INSERT INTO idempotencia
        (
            chave_idempotencia,
            Requisicao,
            Resultado
        )
        VALUES
        (
            @Id,
            @Requisicao,
            @Resultado
        )
    ";

    private static string Update = @"
        UPDATE idempotencia
        SET
            Requisicao = @Requisicao,
            Resultado = @Resultado
        WHERE
            chave_idempotencia = @Id
    ";

    #endregion
}