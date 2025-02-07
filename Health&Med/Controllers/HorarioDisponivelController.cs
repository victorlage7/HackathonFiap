using Microsoft.AspNetCore.Mvc;
using Health_Med.Repository.Interface;
using Health_Med.Model;

[ApiController]
[Route("api/[controller]")]
public class HorarioDisponivelController : ControllerBase
{
    private readonly IHorarioDisponivelRepository _horarioRepository;

    public HorarioDisponivelController(IHorarioDisponivelRepository horarioRepository)
    {
        _horarioRepository = horarioRepository;
    }

    [HttpGet("medico/{medicoId}")]
    public async Task<IActionResult> ObterPorMedico(int medicoId)
    {
        var horarios = await _horarioRepository.ObterPorMedicoAsync(medicoId);
        return Ok(horarios);
    }

    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] HorarioDisponivel horario)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var novoId = await _horarioRepository.AdicionarAsync(horario);
        return CreatedAtAction(nameof(ObterPorMedico), new { medicoId = horario.MedicoId }, horario);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        var removido = await _horarioRepository.RemoverAsync(id);
        if (!removido)
            return NotFound();

        return NoContent();
    }
}
