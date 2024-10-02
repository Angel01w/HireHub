using System.ComponentModel.DataAnnotations;

namespace HirehubWeb.Models
{
    public class CandidateTraining
    {
        [Key]
        // Propiedades básicas
        public int CandidateTrainingID { get; set; } // ID único de la formación
        public int CandidateID { get; set; } // ID del candidato
        public int TrainingID { get; set; } // ID del entrenamiento

        // Información adicional
        public string TrainingName { get; set; } // Nombre del entrenamiento
        public DateTime StartDate { get; set; } // Fecha de inicio del entrenamiento
        public DateTime EndDate { get; set; } // Fecha de finalización del entrenamiento
        public string Certificate { get; set; } // Certificado otorgado (si aplica)

        // Constructor vacío
        public CandidateTraining() { }

        // Constructor con parámetros
        public CandidateTraining(int candidateTrainingID, int candidateID, int trainingID, string trainingName, DateTime startDate, DateTime endDate, string certificate)
        {
            CandidateTrainingID = candidateTrainingID;
            CandidateID = candidateID;
            TrainingID = trainingID;
            TrainingName = trainingName;
            StartDate = startDate;
            EndDate = endDate;
            Certificate = certificate;
        }
        // Relación con Candidate
        public Candidate Candidate { get; set; }
        // Relación con Training
        public Training Training { get; set; }
        // Método para obtener detalles del entrenamiento
        public string GetTrainingDetails()
        {
            return $"Entrenamiento: {TrainingName}, CandidatoID: {CandidateID}, Inicio: {StartDate.ToShortDateString()}, Fin: {EndDate.ToShortDateString()}";
        }
    }

}
