namespace Health_Med.Model
{
    public class Consulta
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public int PacienteId { get; set; }
        public DateTime DataHora { get; set; }
        public string Status { get; set; }
    }
}
