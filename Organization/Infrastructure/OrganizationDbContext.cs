using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entity;

namespace Organization.Infrastructure
{
    public class OrganizationDbContext : DbContext
    {

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Office> Office { get; set; }
        public DbSet<Parish> Parish { get; set; }
        public DbSet<Team> Team { get; set; }

        public OrganizationDbContext(DbContextOptions<OrganizationDbContext> dbContextOptions) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("LocalConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
            base.OnConfiguring(optionsBuilder);




        }

        //seeding data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parish>().HasData(
                new Parish("Vermillion")
                {
                    ParishId = 1,
                    ZipCode = 70510
                },
                new Parish("Laffyette")
                {
                    ParishId = 2,
                    ZipCode = 70508
                },
                new Parish("East Baton Rouge")
                {
                    ParishId = 3,
                    ZipCode = 70808
                },
                new Parish("Ascension")
                {
                    ParishId = 4,
                    ZipCode = 70346
                }
                );

            modelBuilder.Entity<Office>().HasData(
                new Office("Diamond Nails")
                {
                    ParishId = 1,
                    OfficeId = 1,

                },
                new Office("Atlas")
                {
                    ParishId = 2,
                    OfficeId = 2,
                },
                new Office("CGI")
                {
                    ParishId = 4,
                    OfficeId = 3
                },
                new Office("OTS/PMO")
                {
                    ParishId = 3,
                    OfficeId = 4
                }
                );
            modelBuilder.Entity<Office>().HasMany(e => e.Employees).WithOne(o => o.Office).OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Employee>().HasData(
                new Employee("Hoa", "Nguyen")
                {
                    EmployeeId = 1,
                    Age = 20,
                    OfficeId = 4
                },
                new Employee("Alex", "Brodsky")
                {
                    EmployeeId = 2,
                    Age = 21,
                    OfficeId = 4
                },
                new Employee("Some", "Guy")
                {
                    EmployeeId = 3,
                    Age = 47,
                    OfficeId = 1
                },
                new Employee("Hans", "Neuenhaus")
                {
                    EmployeeId = 4,
                    Age = 33,
                    OfficeId = 4
                }
                );

            modelBuilder.Entity<Team>().HasData(
                new Team("Cool Team")
                {
                    TeamId = 1,
                    Description = "For cool people only."
                },
                new Team("Lame Team")
                {
                    TeamId = 2,
                    Description = "For lame people >:)"
                },
                new Team("Just Hans")
                {
                    TeamId = 3,
                    Description = "A team just for Hans"
                }
                );

            modelBuilder.Entity<Team>().HasMany(e => e.Employees)
                .WithMany(t => t.Teams).UsingEntity(j => j.HasData(
                    new { EmployeesEmployeeId = 1, TeamsTeamId = 1 },
                    new { EmployeesEmployeeId = 2, TeamsTeamId = 1 },
                    new { EmployeesEmployeeId = 2, TeamsTeamId = 2 },
                    new { EmployeesEmployeeId = 3, TeamsTeamId = 2 },
                    new { EmployeesEmployeeId = 4, TeamsTeamId = 1 },
                    new { EmployeesEmployeeId = 4, TeamsTeamId = 3 }
                    ));
            base.OnModelCreating(modelBuilder);
        }


    }
}
