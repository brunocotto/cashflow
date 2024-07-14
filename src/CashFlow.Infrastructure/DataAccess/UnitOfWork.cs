using CashFlow.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;

internal class UnitOfWork(CashFlowDbContext dbContext) : IUnitOfWork
{
    private readonly CashFlowDbContext _dbcontext = dbContext;
    public async Task Commit() => await _dbcontext.SaveChangesAsync();
}
