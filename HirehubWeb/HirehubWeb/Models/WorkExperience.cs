using System.ComponentModel.DataAnnotations;

public class WorkExperience
{
    [Key]
    public int ExperienceID { get; set; } // IDENTITY(1,1) NOT NULL
    public string Company { get; set; } // varchar(255) NOT NULL
    public string PositionHeld { get; set; } // varchar(100) NOT NULL
    public DateTime? StartDate { get; set; } // date NULL
    public DateTime? EndDate { get; set; } // date NULL
    public decimal? Salary { get; set; } // decimal(18, 2) NULL
}
