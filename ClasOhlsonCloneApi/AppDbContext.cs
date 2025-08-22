namespace ClasOhlsonCloneApi;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Her definerer du tabellene i databasen.
    // Start med Users (matcher tabellen din i MySQL).
    public DbSet<PostData> Users { get; set; }
}