using HirehubWeb.Models;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int UserID { get; set; } // IDENTITY(1,1) NOT NULL
    public string Username { get; set; } // varchar(50) NOT NULL
    public string PasswordHash { get; set; } // varchar(255) NOT NULL
    public string Email { get; set; } // varchar(100) NOT NULL
    public string Role { get; set; } // varchar(50) NOT NULL
    public DateTime? CreationDate { get; set; } // datetime NULL
    public int? EmployeeID { get; set; } // Foreign Key to Employees
    public int? CandidateID { get; set; } // Foreign Key to Candidates
    public string UserType { get; set; } // varchar(20) NULL ('Candidate' or 'Employee')

    // Navigation properties
    public Candidate Candidate { get; set; } // Navigation to Candidate
    public Employee Employee { get; set; } // Navigation to Employee
}
