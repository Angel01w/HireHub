using System.ComponentModel.DataAnnotations;

namespace HirehubWeb.Models
{
    public class Competency
    {
        [Key]
        public int CompetencyID { get; set; } // IDENTITY(1,1) NOT NULL
        public string Description { get; set; } // varchar(255) NOT NULL
        public string Status { get; set; } // varchar(50) NOT NULL

        // Navigation properties
        public ICollection<CandidateCompetency> CandidateCompetencies { get; set; }
    }
}
