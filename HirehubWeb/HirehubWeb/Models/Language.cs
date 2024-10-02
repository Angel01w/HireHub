using System.ComponentModel.DataAnnotations;

public class Language
{
    [Key]
    public int LanguageID { get; set; } // IDENTITY(1,1) NOT NULL
    public string Name { get; set; } // varchar(100) NOT NULL
    public string Status { get; set; } // varchar(50) NOT NULL
}
