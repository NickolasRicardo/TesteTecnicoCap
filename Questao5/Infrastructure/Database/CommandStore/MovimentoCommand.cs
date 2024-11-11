using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Extensions;
using Questao5.Domain.Interfaces.QueryStore;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore;

public class MovimentoCommand : IMovimentoCommand
{
    private readonly DatabaseConfig _databaseConfig;

    public MovimentoCommand(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public async Task CriarMovimento(MovimentoEntity movimento)
    {
        try
        {
            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("IdMovimento", movimento.Id);
            parameters.Add("IdContaCorrente", movimento.IdContaCorrente);
            parameters.Add("DataMovimento", movimento.DataMovimento.ToString("dd-MM-yyyy"));
            parameters.Add("TipoMovimento", movimento.TipoMovimento);
            parameters.Add("Valor", movimento.Valor);
            
            using (var connection = new SqliteConnection(_databaseConfig.Name))
            {
               await connection.ExecuteAsync(Insert, parameters);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao criar movimento", ex);
        }
    }
    
    public async Task AtualizarMovimento(MovimentoEntity movimento)
    {
        try
        {
            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("IdMovimento", movimento.Id);
            parameters.Add("IdContaCorrente", movimento.IdContaCorrente);
            parameters.Add("DataMovimento", movimento.DataMovimento);
            parameters.Add("TipoMovimento", movimento.TipoMovimento);
            parameters.Add("Valor", movimento.Valor);
            
            using (var connection = new SqliteConnection(_databaseConfig.Name))
            {
               await connection.ExecuteAsync(Update, parameters);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao atualizar movimento", ex);
        }
    }


    #region Sql

    private static string Insert = @"
        INSERT INTO Movimento
        (
            idmovimento,
            idcontacorrente,
            datamovimento,
            tipomovimento,
            valor
        )
        VALUES
        (
            @IdMovimento,
            @IdContaCorrente,
            @DataMovimento,
            @TipoMovimento,
            @Valor
        )" ;

    private static string Update = @"
        UPDATE Movimento
        SET
            idcontacorrente = @IdContaCorrente,
            datamovimento = @DataMovimento,
            tipomovimento = @TipoMovimento,
            valor = @Valor
        WHERE
            idmovimento = @IdMovimento";

    #endregion
}