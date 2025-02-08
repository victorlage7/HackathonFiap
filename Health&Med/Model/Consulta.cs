namespace Health_Med.Model
{
    public class Consulta
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataHora { get; set; }
        public CosultaStatus Status { get; set; }
        public double Valor { get; set; }
        public string MotivoCancelamento { get; set; }
    }

    public enum CosultaStatus { 
        Agendada = 0
       ,Confirmada = 1
       ,Cancelada = 2
    }
}
