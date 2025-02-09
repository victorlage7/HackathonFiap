﻿using Health_Med.Model;

namespace Health_Med.VOs
{
    public class HorarioDisponivelVO
    {
        public HorarioDisponivelVO(HorarioDisponivel horarioDisponivel, Medico Medico)
        {
            Disponivel = horarioDisponivel.Disponivel;
            DataHora = horarioDisponivel.DataHora;
            NomeMedico = Medico.Nome;
            EmailMedico = Medico.Email;
            CRM =   Medico.CRM;
            EspecilidadeMedico = Medico.Especilidade;
            HorarioDisponivelid = horarioDisponivel.Id;
        }

        public int HorarioDisponivelid { get; set; }
        public bool Disponivel  { get; set; }
        public DateTime DataHora { get; set; }
        public string NomeMedico { get; set; }
        public string EmailMedico { get; set; }
        public int CRM { get; set; }
        public Especialidade EspecilidadeMedico { get; set; }
    }
}
