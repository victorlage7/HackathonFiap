namespace Health_Med.Model
{
    public class Medico
    {
        public Medico(){}

        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public int CRM { get; set; }

        public Especilidade Especilidade { get; set; }
    }

    public enum Especilidade
    {
        Cardiologia=0,
        Dermatologia=1,
        Endocrinologia=2,
        Ginecologia=3,
        Neurologia=4,
        Oftalmologia=5,
        Ortopedia=6,
        Pediatria=7,
        Psiquiatria=8,
        Reumatologia=9,
        Urologia=10
    }
}