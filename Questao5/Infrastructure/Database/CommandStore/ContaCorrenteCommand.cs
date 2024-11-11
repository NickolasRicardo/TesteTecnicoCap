using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces.QueryStore;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore;

public class ContaCorrenteCommand : IContaCorrenteCommand
{
    private readonly DatabaseConfig _databaseConfig;

    public ContaCorrenteCommand(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public async Task CriarContaCorrente(ContaCorrenteEntity contaCorrente)
    {
        try
        {
            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("IdContaCorrente", contaCorrente.Id);
            parameters.Add("Numero", contaCorrente.Numero);
            parameters.Add("Nome", contaCorrente.Nome);
            parameters.Add("Ativo", contaCorrente.Ativo);
            
            using (var connection = new SqliteConnection(_databaseConfig.Name))
            {
               await connection.ExecuteAsync(Insert, parameters);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao criar conta corrente", ex);
        }
    }

    public async Task AtualizarContaCorrente(ContaCorrenteEntity contaCorrente)
    {
        try
        {
            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("IdContaCorrente", contaCorrente.Id);
            parameters.Add("Numero", contaCorrente.Numero);
            parameters.Add("Nome", contaCorrente.Nome);
            parameters.Add("Ativo", contaCorrente.Ativo);
            
            using (var connection = new SqliteConnection(_databaseConfig.Name))
            {
               await connection.ExecuteAsync(Update, parameters);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao atualizar conta corrente", ex);
        }
    }

    public async Task DeletarContaCorrente(ContaCorrenteEntity contaCorrente)
    {
        try
        {
            DynamicParameters parameters = new DynamicParameters();
            
            parameters.Add("IdContaCorrente", contaCorrente.Id);
            parameters.Add("Numero", contaCorrente.Numero);
            parameters.Add("Nome", contaCorrente.Nome);
            parameters.Add("Ativo", false);
            
            using (var connection = new SqliteConnection(_databaseConfig.Name))
            {
                await connection.ExecuteAsync(Update, parameters);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao atualizar conta corrente", ex);
        }
    }

    #region Sql

    private static string Insert = @"
        Insert into
                contacorrente
                ( 
                      idcontacorrente , 
                      numero           , 
                      nome             , 
                      ativo 
                )            
        values 
                (
                    @IdContaCorrente,
                    @Numero,
                    @Nome,
                    @Ativo
                )
    ";
    
    private static string Update = @"
        Update 
                contacorrente
        set 
                numero = @Numero,
                nome = @Nome,
                ativo = @Ativo
        where 
                idcontacorrente = @IdContaCorrente
                ";
    #endregion
}