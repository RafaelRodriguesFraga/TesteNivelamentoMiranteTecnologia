using Dapper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Repositories;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Repositories
{
    public class IdempotenciaWriteRepository : IIdempotenciaWriteRepository
    {
        private readonly DatabaseConfig databaseConfig;
        SqliteConnection connection;

        public IdempotenciaWriteRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
            connection = new SqliteConnection(databaseConfig.Name);
            connection.Open();
        }

        public async Task InserirAsync(object request, string resultado)
        {
            try
            {
                var requisicao = JsonConvert.SerializeObject(request);
                Idempotencia idempotencia = new Idempotencia(requisicao, resultado);

                var insertSqlIdempotencia =
                    @"INSERT INTO Idempotencia (chave_idempotencia, requisicao, resultado) 
                      VALUES (@ChaveIdempotencia, @Requisicao, @Resultado);";

                await connection.ExecuteAsync(insertSqlIdempotencia, idempotencia);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                connection.Close();

            }
        }
    }
}
