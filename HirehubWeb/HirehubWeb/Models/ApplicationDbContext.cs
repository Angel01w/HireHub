using Microsoft.EntityFrameworkCore;


namespace HirehubWeb.Models
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet for each table in your database
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateCompetency> CandidateCompetencies { get; set; }
        public DbSet<CandidateTraining> CandidateTrainings { get; set; }
        public DbSet<Competency> Competencies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<WorkExperience> WorkExperience { get; set; }

      
    }
}
