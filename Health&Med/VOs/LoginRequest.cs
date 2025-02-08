namespace Health_Med.VOs
{
    public class LoginRequest
    {
        public string Login { get; set; } // CRM para médicos, cpf para pacientes
        public string Senha { get; set; }
        public TipoUsuario TipoUsuario { get; set; } // "medico" ou "paciente"
    }

    public enum TipoUsuario { 
    Medico = 0
    ,Paciente = 1
    }
}