using CashFlow.Domain.Repositories;

namespace CashFlow.Infrastructure.DataAccess;

internal class UnitOfWork(CashFlowDbContext dbContext) : IUnitOfWork
{
    private readonly CashFlowDbContext _dbcontext = dbContext;
    public async Task Commit() => await _dbcontext.SaveChangesAsync();
}
