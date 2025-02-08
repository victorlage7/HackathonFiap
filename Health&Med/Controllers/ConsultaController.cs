﻿using Microsoft.AspNetCore.Mvc;
using Health_Med.Repository.Interface;
using Health_Med.Model;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class ConsultaController : ControllerBase
{
    private readonly IConsultaRepository _consultaRepository;
    private readonly IMedicoRepository _medicoRepository;
    private readonly IPacienteRepository _pacienteRepository;
    private readonly IHorarioDisponivelRepository _horarioDisponivelRepository;

    public ConsultaController(IConsultaRepository consultaRepository, IMedicoRepository medicoRepository, IPacienteRepository pacienteRepository, IHorarioDisponivelRepository horarioDisponivelRepository)
    {
        _consultaRepository = consultaRepository;
        _medicoRepository = medicoRepository;
        _pacienteRepository = pacienteRepository;
        _horarioDisponivelRepository = horarioDisponivelRepository;
    }

    [Authorize(Roles = "Medico,Paciente")]
    [HttpGet]
    public async Task<IActionResult> ObterTodas()
    {
        var consultas = await _consultaRepository.ObterTodasAsync();
        return Ok(consultas);
    }

    [Authorize(Roles = "Medico,Paciente")]
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var consulta = await _consultaRepository.ObterPorIdAsync(id);
        if (consulta == null)
            return NotFound();
       
        Consulta consulta1 = new Consulta();
        Medico medico = new Medico();
        Paciente paciente = new Paciente();
        HorarioDisponivel horario = new HorarioDisponivel();

        medico = await _medicoRepository.ObterPorIdAsync(consulta.MedicoId);
        if (medico == null)
            return BadRequest("Médico não exisate.");

        paciente = await _pacienteRepository.ObterPorIdAsync(consulta.PacienteId);
        if (paciente == null)
            return BadRequest("Paciente não exisate.");

        horario = await _horarioDisponivelRepository.ObterPorIdAsync(consulta.HorarioDisponivelid);
        if (horario == null)
            return BadRequest("Horário não exisate.");

        consulta1.Medico = medico;
        consulta1.Paciente = paciente;
        consulta1.HorarioDisponivel = horario;
        consulta1.Status = consulta.Status;
        consulta1.Valor = medico.ValorConsulta;
        consulta1.MotivoCancelamento = consulta.MotivoCancelamento;

        return Ok(consulta1);
    }

    [Authorize(Roles = "Paciente")]
    [HttpPost]
    public async Task<IActionResult> Agendar([FromBody] Consulta consulta)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var medico = await _medicoRepository.ObterPorIdAsync(consulta.MedicoId);
        if (medico == null)
            return BadRequest("Médico não exisate.");

        var paciente = await _pacienteRepository.ObterPorIdAsync(consulta.PacienteId);
        if (paciente == null)
            return BadRequest("Paciente não exisate.");

        var horario = await _horarioDisponivelRepository.ObterPorIdAsync(consulta.HorarioDisponivelid);
        if (horario == null)
            return BadRequest("Horário não exisate.");

        var novoId = await _consultaRepository.AgendarAsync(consulta);

        Consulta consulta1 = await _consultaRepository.ObterPorIdAsync(novoId);

        consulta1.Medico = medico;
        consulta1.Paciente = paciente;
        consulta1.HorarioDisponivel = horario;

        return Ok(consulta1);
    }

    [Authorize(Roles = "Medico,Paciente")]
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
        cancelarConsulta.HorarioDisponivelid = consulta.HorarioDisponivelid;
        cancelarConsulta.Status = CosultaStatus.Cancelada;
        cancelarConsulta.MotivoCancelamento = justificativa;

        var cancelado = await _consultaRepository.CancelarAsync(cancelarConsulta);
        if (!cancelado)
            return StatusCode(500, "Erro ao cancelar a consulta.");

        return NoContent();
    }

    [Authorize(Roles = "Medico")]
    [HttpPut("{id}/aceitar")]
    public async Task<IActionResult> Aceitar(int id, double valor)
    {
        var consulta = await _consultaRepository.ObterPorIdAsync(id);
        if (consulta == null)
            return NotFound();

        var medico = await _medicoRepository.ObterPorIdAsync(consulta.MedicoId);
        if (medico == null)
            return BadRequest("Médico não exisate.");

        Consulta aceitarConsulta = new Consulta();
        aceitarConsulta.Id = consulta.Id;
        aceitarConsulta.MedicoId = consulta.MedicoId;
        aceitarConsulta.PacienteId = consulta.PacienteId;
        aceitarConsulta.HorarioDisponivelid = consulta.HorarioDisponivelid;
        aceitarConsulta.Status = CosultaStatus.Confirmada;
        aceitarConsulta.Valor = medico.ValorConsulta;


        var aceito = await _consultaRepository.AceitarAsync(aceitarConsulta);
        if (!aceito)
            return StatusCode(500, "Erro ao cancelar a consulta.");

        return NoContent();
    }
}
