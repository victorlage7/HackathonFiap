using Dapper;
using System.Data;
using Health_Med.Model;

namespace Health_Med.Repository
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly IDbConnection _dbConnection;

        public PacienteRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Paciente>> ObterTodosAsync()
        {
            var query = "SELECT * FROM Pacientes";
            return await _dbConnection.QueryAsync<Paciente>(query);
        }

        public async Task<Paciente> ObterPorIdAsync(int id)
        {
            var query = "SELECT * FROM Pacientes WHERE Id = @Id";
            return await _dbConnection.QueryFirstOrDefaultAsync<Paciente>(query, new { Id = id });
        }

        public async Task<int> AdicionarAsync(Paciente paciente)
        {
            var query = @"INSERT INTO Pacientes (Nome, CPF, Email,Senha) 
                          VALUES (@Nome, @CPF,@Email,@Senha);
                          SELECT CAST(SCOPE_IDENTITY() as int)";
            return await _dbConnection.ExecuteScalarAsync<int>(query, paciente);
        }

        public async Task<bool> AtualizarAsync(Paciente paciente)
        {
            var query = @"UPDATE Pacientes SET 
                          Nome = @Nome, 
                          CPF = @CPF, 
                          Email = @Email,
                          Senha = @Senha
                          WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(query, paciente);
            return result > 0;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var query = "DELETE FROM Pacientes WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(query, new { Id = id });
            return result > 0;
        }
    }
}
