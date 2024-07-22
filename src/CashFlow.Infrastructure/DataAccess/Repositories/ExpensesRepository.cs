using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

// internal para garantir que ExpensesRepository só sera executado dentro do prj de infra
internal class ExpensesRepository(CashFlowDbContext dbContext) : IExpenseWriteOnlyRepository, IExpenseReadOnlyRepository, IExpenseUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext = dbContext;

    public async Task Add(Expense expense)
    {
       await _dbContext.AddAsync(expense);
    }

    public async Task Delete(long id)
    {
        var result = await _dbContext.Expenses.FirstAsync(expense => expense.Id == id);

        _dbContext.Expenses.Remove(result);
    }

    public async Task<List<Expense>> GetAll(User user)
    {
        // AsNoTracking para o entity framework não armazenas as entidades em cache, melhorando a performance
       return await _dbContext.Expenses
            .Include(expense => expense.Tags)
            .AsNoTracking()
            .Where(expense => expense.UserId ==  user.Id)
            .ToListAsync();
    }

    async Task<Expense?> IExpenseReadOnlyRepository.GetById(User user, long id)
    {
        return await _dbContext.Expenses
            .Include(expense => expense.Tags)
            .AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }

    async Task<Expense?> IExpenseUpdateOnlyRepository.GetById(User user, long id)
    {
        return await _dbContext.Expenses
            .Include(expense => expense.Tags)
            .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);  
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByMonth(User user, DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext.Expenses
            .AsNoTracking()
            .Where(expense => expense.UserId == user.Id && expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }
}
