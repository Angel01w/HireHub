using HirehubWeb.Models;
using System.ComponentModel.DataAnnotations;

public class CandidateTraining
{
    [Key]
    public int CandidateTrainingID { get; set; } // IDENTITY(1,1) NOT NULL
    public int CandidateID { get; set; } // Foreign Key to Candidates
    public int TrainingID { get; set; } // Foreign Key to Trainings

    // Navigation properties
    public Candidate Candidate { get; set; } // Navigation to Candidate
    public Training Training { get; set; } // Navigation to Training
}
