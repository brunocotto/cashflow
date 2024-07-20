using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpenseWriteOnlyRepository _expenseWriteOnly;
    private readonly IExpenseReadOnlyRepository _expenseReadOnly;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteExpenseUseCase(
        IExpenseWriteOnlyRepository expenseWriteOnly,
        IExpenseReadOnlyRepository expenseReadOnly,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork)
    {
        _expenseWriteOnly = expenseWriteOnly;
        _expenseReadOnly = expenseReadOnly;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var expense = await _expenseReadOnly.GetById(loggedUser, id);

        if (expense is null)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        await _expenseWriteOnly.Delete(id);

        await _unitOfWork.Commit();
    }
}
