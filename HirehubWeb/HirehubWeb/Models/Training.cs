using System.ComponentModel.DataAnnotations;

public class Training
{
    [Key]
    public int TrainingID { get; set; } // IDENTITY(1,1) NOT NULL
    public string Description { get; set; } // varchar(255) NOT NULL
    public string Level { get; set; } // varchar(50) NOT NULL
    public DateTime? StartDate { get; set; } // date NULL
    public DateTime? EndDate { get; set; } // date NULL
    public string Institution { get; set; } // varchar(255) NULL

    // Navigation properties
    public ICollection<CandidateTraining> CandidateTrainings { get; set; }
}
