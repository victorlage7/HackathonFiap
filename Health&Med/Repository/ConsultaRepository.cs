using Dapper;
using System.Data;
using Health_Med.Repository.Interface;
using Health_Med.Model;

namespace Health_Med.Repository
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly IDbConnection _dbConnection;

        public ConsultaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Consulta>> ObterTodasAsync()
        {
            var query = "SELECT * FROM Consultas";
            return await _dbConnection.QueryAsync<Consulta>(query);
        }

        public async Task<Consulta> ObterPorIdAsync(int id)
        {
            var query = "SELECT * FROM Consultas WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Consulta>(query, new { Id = id });
        }

        public async Task<int> AgendarAsync(Consulta consulta)
        {
            var query = @"INSERT INTO Consultas (MedicoId, PacienteId, DataHora) 
                          VALUES (@MedicoId, @PacienteId, @DataHora, 'Agendada');
                          SELECT CAST(SCOPE_IDENTITY() as int)";
            return await _dbConnection.ExecuteScalarAsync<int>(query, consulta);
        }

        public async Task<bool> CancelarAsync(int id, string justificativa)
        {
            var query = @"UPDATE Consultas 
                          SET Status = 'Cancelada'
                          WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(query, new { Id = id, Justificativa = justificativa });
            return result > 0;
        }
    }
}
