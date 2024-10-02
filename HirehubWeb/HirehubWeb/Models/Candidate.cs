using System.ComponentModel.DataAnnotations;

namespace HirehubWeb.Models
{
    public class Candidate
    {
        public int CandidateID { get; set; } // Identificador único para cada candidato
        public string Identification { get; set; } // Documento de identificación (por ejemplo, cédula o pasaporte)
        public string Name { get; set; } // Nombre del candidato
        public string DesiredPosition { get; set; } // Posición deseada (por ejemplo, Ingeniero de Software)
        public string Department { get; set; } // Departamento relacionado con la posición deseada
        public decimal DesiredSalary { get; set; } // Salario deseado
        public string KeyCompetencies { get; set; } // Competencias clave del candidato
        public string KeyTrainings { get; set; } // Capacitación clave del candidato
        public string WorkExperience { get; set; } // Experiencia laboral del candidato
        public string RecommendedBy { get; set; } // Persona que recomendó al candidato (opcional)
    }

}
