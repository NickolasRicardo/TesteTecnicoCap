namespace Questao5.Domain.Entities;

public class IdempotenciaEntity : BaseEntity
{
    public string Requisicao { get; set; }
    public string Resultado { get; set; }
}