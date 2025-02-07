using Microsoft.AspNetCore.Mvc;
using Health_Med.Repository.Interface;
using Health_Med.Model;

[ApiController]
[Route("api/[controller]")]
public class ConsultaController : ControllerBase
{
    private readonly IConsultaRepository _consultaRepository;

    public ConsultaController(IConsultaRepository consultaRepository)
    {
        _consultaRepository = consultaRepository;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        var consultas = await _consultaRepository.ObterTodasAsync();
        return Ok(consultas);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var consulta = await _consultaRepository.ObterPorIdAsync(id);
        if (consulta == null)
            return NotFound();

        return Ok(consulta);
    }

    [HttpPost]
    public async Task<IActionResult> Agendar([FromBody] Consulta consulta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var novoId = await _consultaRepository.AgendarAsync(consulta);
        return CreatedAtAction(nameof(ObterPorId), new { id = novoId }, consulta);
    }

    [HttpPut("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id, [FromBody] string justificativa)
    {
        var consulta = await _consultaRepository.ObterPorIdAsync(id);
        if (consulta == null)
            return NotFound();

        var cancelado = await _consultaRepository.CancelarAsync(id, justificativa);
        if (!cancelado)
            return StatusCode(500, "Erro ao cancelar a consulta.");

        return NoContent();
    }
}
