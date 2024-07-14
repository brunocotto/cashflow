using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

// internal para garantir que ExpensesRepository s� sera executado dentro do prj de infra
internal class ExpensesRepository(CashFlowDbContext dbContext) : IExpenseWriteOnlyRepository, IExpenseReadOnlyRepository, IExpenseUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbcontext = dbContext;

    public async Task Add(Expense expense)
    {
       await _dbcontext.AddAsync(expense);
    }

    public async Task<bool> Delete(long id)
    {
        var result = await _dbcontext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);

        if (result is null) {
            return false;
        }

        _dbcontext.Expenses.Remove(result);

        return true;
    }

    public async Task<List<Expense>> GetAll()
    {
        // AsNoTracking para o entity framework n�o armazenas as entidades em cache, melhorando a performance
       return await _dbcontext.Expenses.AsNoTracking().ToListAsync();
    }

    async Task<Expense?> IExpenseReadOnlyRepository.GetById(long id)
    {
        return await _dbcontext.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);
    }

    async Task<Expense?> IExpenseUpdateOnlyRepository.GetById(long id)
    {
        return await _dbcontext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
    }

    public void Update(Expense expense)
    {
         _dbcontext.Expenses.Update(expense);
    }
}