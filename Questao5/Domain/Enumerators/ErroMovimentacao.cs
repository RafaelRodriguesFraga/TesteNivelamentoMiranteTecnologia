using System.ComponentModel;

namespace Questao5.Domain.Enumerators
{
    public enum ErroMovimentacao
    {
        [Description("Conta inválida")]
        INVALID_ACCOUNT = 1,

        [Description("Conta inativa")]
        INACTIVE_ACCOUNT,

        [Description("Valor inválido")]
        INVALID_VALUE,

        [Description("Tipo invalido")]
        INVALID_TYPE
    }
}
