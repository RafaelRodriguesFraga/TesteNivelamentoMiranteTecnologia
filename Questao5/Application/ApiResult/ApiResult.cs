namespace Questao5.Application.ApiResult
{
    public class ApiResult<TResult>
    {
        public ApiResult(bool sucesso, int codigo, TResult dados, string erro)
        {
            Sucesso = sucesso;
            Codigo = codigo;
            Dados = dados;
            Erro = erro;
        }

        public bool Sucesso { get; set; }
        public int Codigo { get; set; }
        public TResult Dados { get; set; }
        public string Erro { get; set; }


    }
}

