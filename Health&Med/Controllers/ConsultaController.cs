using Microsoft.AspNetCore.Mvc;
using Health_Med.Repository.Interface;
using Health_Med.Model;
using System.Drawing;

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

    [HttpGet("medico/especialidade/{especialidade}")]
    public async Task<IActionResult> ObterPorEspecialidade(Especialidade especialidade)
    {
        var consultas = await _consultaRepository.ObterConsultasPorEspecialidadeAsync(especialidade);
        if (consultas == null)
            return NotFound();

        return Ok(consultas);
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

        Consulta cancelarConsulta = new Consulta();
        cancelarConsulta.Id = consulta.Id;
        cancelarConsulta.MedicoId = consulta.MedicoId;
        cancelarConsulta.PacienteId = consulta.PacienteId;
        cancelarConsulta.DataHora = consulta.DataHora;
        cancelarConsulta.Status = CosultaStatus.Confirmada;

        var cancelado = await _consultaRepository.CancelarAsync(cancelarConsulta);
        if (!cancelado)
            return StatusCode(500, "Erro ao cancelar a consulta.");

        return NoContent();
    }

    [HttpPut("{id}/aceitar")]
    public async Task<IActionResult> Aceitar(int id, double valor)
    {
        var consulta = await _consultaRepository.ObterPorIdAsync(id);
        if (consulta == null)
            return NotFound();

        Consulta aceitarConsulta = new Consulta();
        aceitarConsulta.Id = consulta.Id ;
        aceitarConsulta.MedicoId = consulta.MedicoId;
        aceitarConsulta.PacienteId = consulta.PacienteId;
        aceitarConsulta.DataHora = consulta.DataHora;
        aceitarConsulta.Status = CosultaStatus.Confirmada;
        aceitarConsulta.Valor = valor;


        var aceito = await _consultaRepository.AceitarAsync(aceitarConsulta);
        if (!aceito)
            return StatusCode(500, "Erro ao cancelar a consulta.");

        return NoContent();
    }
}
