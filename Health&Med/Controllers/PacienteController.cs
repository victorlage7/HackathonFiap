using Microsoft.AspNetCore.Mvc;
using Health_Med.Model;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class PacienteController : ControllerBase
{
    private readonly IPacienteRepository _pacienteRepository;

    public PacienteController(IPacienteRepository pacienteRepository)
    {
        _pacienteRepository = pacienteRepository;
    }

    [Authorize(Roles = "Medico,Paciente")]
    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var pacientes = await _pacienteRepository.ObterTodosAsync();
        return Ok(pacientes);
    }

    [Authorize(Roles = "Medico,Paciente")]
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var paciente = await _pacienteRepository.ObterPorIdAsync(id);
        if (paciente == null)
            return NotFound();

        return Ok(paciente);
    }

    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] Paciente paciente)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var novoId = await _pacienteRepository.AdicionarAsync(paciente);
        return CreatedAtAction(nameof(ObterPorId), new { id = novoId }, paciente);
    }

    [Authorize(Roles = "Paciente")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] Paciente paciente)
    {
        if (id != paciente.Id)
            return BadRequest();

        var existe = await _pacienteRepository.ObterPorIdAsync(id);
        if (existe == null)
            return NotFound();

        var atualizado = await _pacienteRepository.AtualizarAsync(paciente);
        if (!atualizado)
            return StatusCode(500, "Erro ao atualizar o paciente.");

        return NoContent();
    }

    [Authorize(Roles = "Paciente")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        var existe = await _pacienteRepository.ObterPorIdAsync(id);
        if (existe == null)
            return NotFound();

        var removido = await _pacienteRepository.RemoverAsync(id);
        if (!removido)
            return StatusCode(500, "Erro ao remover o paciente.");

        return NoContent();
    }
}
