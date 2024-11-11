using Questao5.Domain.Enumerators;
using Questao5.Domain.Extensions;

namespace Questao5.Domain.Entities
{
    public class MovimentoEntity : BaseEntity
    {
        public string IdContaCorrente { get; set; }
        public DateTime DataMovimento { get; set; }
        public string TipoMovimento { get; set; }
        public double Valor { get; set; }
    }
}
