using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Health_Med.Model
{
    public class Consulta
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataHora { get; set; }
        [EnumDataType(typeof(CosultaStatus))]
        public CosultaStatus Status { get; set; }
        public double Valor { get; set; }
        public string MotivoCancelamento { get; set; }
    }

    public enum CosultaStatus {
        [Description("Agendada")] Agendada = 0
       , [Description("Confirmada")] Confirmada = 1
       , [Description("Cancelada")] Cancelada = 2
    }
}
