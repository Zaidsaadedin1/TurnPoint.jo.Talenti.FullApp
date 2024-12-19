using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TurnPoint.Jo.APIs.Entities;
using TurnPoint.Jo.Talenti.SharedModels.Entities;

public class ApplicationDbContext : IdentityDbContext<User, Role, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<InterestsLookup> Interests { get; set; }
    public DbSet<InterestsLookupUser> InterestsLookupUsers { get; set; }  

}
