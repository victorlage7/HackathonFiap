using Health_Med.Model;

public interface IPacienteRepository
{
    Task<IEnumerable<Paciente>> ObterTodosAsync();
    Task<Paciente> ObterPorIdAsync(int id);
    Task<int> AdicionarAsync(Paciente paciente);
    Task<bool> AtualizarAsync(Paciente paciente);
    Task<bool> RemoverAsync(int id);
}