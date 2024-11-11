namespace Questao5.Domain.Entities;

public class ContaCorrenteEntity : BaseEntity
{
    public long Numero { get; set; }
    public string Nome { get; set; }
    public int Ativo { get; set; }
}