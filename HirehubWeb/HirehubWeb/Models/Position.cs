using System.ComponentModel.DataAnnotations;

public class Position
{
    [Key]
    public int PositionID { get; set; } // IDENTITY(1,1) NOT NULL
    public string Name { get; set; } // varchar(100) NOT NULL
    public string RiskLevel { get; set; } // varchar(50) NOT NULL
    public decimal? MinimumSalary { get; set; } // decimal(18, 2) NULL
    public decimal? MaximumSalary { get; set; } // decimal(18, 2) NULL
    public string Status { get; set; } // varchar(50) NOT NULL
}
