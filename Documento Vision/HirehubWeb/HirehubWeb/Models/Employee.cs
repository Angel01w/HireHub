using System.ComponentModel.DataAnnotations;

public class Employee
{
    [Key]
    public int EmployeeID { get; set; } // IDENTITY(1,1) NOT NULL
    public string Identification { get; set; } // varchar(20) NOT NULL
    public string Name { get; set; } // varchar(255) NOT NULL
    public DateTime HireDate { get; set; } // date NOT NULL
    public string Department { get; set; } // varchar(100) NULL
    public string Position { get; set; } // varchar(100) NULL
    public decimal? MonthlySalary { get; set; } // decimal(18, 2) NULL
    public string Status { get; set; } // varchar(50) NOT NULL

    // Navigation properties
    public ICollection<User> Users { get; set; }
}
