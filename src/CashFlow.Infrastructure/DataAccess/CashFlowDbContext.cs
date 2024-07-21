using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;

// internal para garantir que CashFlowDbContext só sera executado dentro do prj de infra
internal class CashFlowDbContext: DbContext
{
    public CashFlowDbContext(DbContextOptions options) : base(options) { }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tag>().ToTable("Tags");
    }

}
