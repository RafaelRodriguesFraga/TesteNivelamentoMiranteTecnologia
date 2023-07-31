using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Domain.Entities
{
    public class SaldoContaCorrente
    {
        public int Numero { get; set; }
        public string Nome { get; set; }

        [Column("saldoatual")]
        public decimal SaldoAtual { get; set; }

    }
}
