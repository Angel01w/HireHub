using System.ComponentModel.DataAnnotations;

namespace HirehubWeb.Models
{
    public class CandidateCompetency
    {
        [Key]
        public int CandidateCompetencyID { get; set; } // IDENTITY(1,1) NOT NULL
        public int CandidateID { get; set; } // Foreign Key to Candidates
        public int CompetencyID { get; set; } // Foreign Key to Competencies

        // Navigation properties
        public Candidate Candidate { get; set; } // Navigation to Candidate
        public Competency Competency { get; set; } // Navigation to Competency
    }
}
