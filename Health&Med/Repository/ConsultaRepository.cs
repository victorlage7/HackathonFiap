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
            var query = @"INSERT INTO Consultas (MedicoId, PacienteId, HorarioDisponivelid,Status) 
                          VALUES (@MedicoId, @PacienteId, @HorarioDisponivelid, 0);
                          SELECT CAST(SCOPE_IDENTITY() as int)";
            return await _dbConnection.ExecuteScalarAsync<int>(query, consulta);
        }

        public async Task<bool> CancelarAsync(Consulta consulta)
        {
            var query = @"UPDATE Consultas 
                          SET Status = @Status,
                          MedicoId = @MedicoId,
                          PacienteId = @PacienteId,
                          DataHora = @DataHora,
                          Valor = @Valor
                          WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(query, consulta);
            return result > 0;
        }

        public async Task<bool> AceitarAsync(Consulta consulta)
        {
            var query = @"UPDATE Consultas 
                          SET Status = @Status,
                          MedicoId = @MedicoId,
                          PacienteId = @PacienteId,
                          DataHora = @DataHora,
                          Valor = @Valor
                          WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(query, consulta);

            return result > 0;
        }

        public async Task<IEnumerable<Consulta>> ObterConsultasPorEspecialidadeAsync(Especialidade especialidade)
        {
            var query = @"
            SELECT c.*,
                   m.Nome AS NomeMedico, m.Especilidade, p.Nome AS NomePaciente
            FROM Consultas c
            INNER JOIN Medicos m ON c.MedicoId = m.Id
            INNER JOIN Pacientes p ON c.PacienteId = p.Id
            WHERE m.Especilidade = @Especialidade";

            return await _dbConnection.QueryAsync<Consulta, Medico, Paciente, Consulta>(
                query,
                (consulta, medico, paciente) =>
                {
                    consulta.Medico = medico;
                    consulta.Paciente = paciente;
                    return consulta;
                },
                new { Especialidade = especialidade }, // Converte enum para string antes de enviar para SQL
                splitOn: "NomeMedico,NomePaciente"
            );
        }
    }
}
