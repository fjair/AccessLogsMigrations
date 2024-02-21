using GD.RRHH.CheckList.AccessMigrations.Models;
using Microsoft.EntityFrameworkCore;

namespace GD.RRHH.CheckList.AccessMigrations.EF;

public class AccessDbContext: DbContext
{
    static readonly string _source = "\\\\192.168.10.70\\C$\\Access Control\\ASManager\\ASRes\\AsLog.mdb";
    static readonly string _provider = "Microsoft.ACE.OLEDB.12.0";
    static readonly string conn = $"Provider={_provider}; Data Source={_source}; Persist Security Info=true;";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseJet(conn);
    }

    public DbSet<AccessLog> AccessLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessLog>().ToTable("AccessLog");
    }
}
