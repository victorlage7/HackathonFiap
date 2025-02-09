using System.ComponentModel.DataAnnotations;

namespace Health_Med.Model
{
    public class Medico
    {
        public Medico(){}

        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public double? ValorConsulta { get; set; }

        public int CRM { get; set; }
        [EnumDataType(typeof(Especialidade))]
        public Especialidade Especilidade { get; set; }
    }

    public enum Especialidade
    {
        Cardiologia = 0,
        Dermatologia = 1,
        Endocrinologia = 2,
        Ginecologia = 3,
        Neurologia = 4,
        Oftalmologia = 5,
        Ortopedia = 6,
        Pediatria = 7,
        Psiquiatria = 8,
        Reumatologia = 9,
        Urologia = 10
    }
}