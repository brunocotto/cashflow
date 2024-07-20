using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetAllExpenseUseCase : IGetAllExpenseUseCase
{
    private readonly IExpenseReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;
    public GetAllExpenseUseCase(
        IExpenseReadOnlyRepository repository,
        ILoggedUser loggedUser,
        IMapper mapper)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _mapper = mapper;
    }
    public async Task<ResponseExpensesJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();   

        var result = await _repository.GetAll(loggedUser);

        return new ResponseExpensesJson 
        {
            Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
        };
    }
}
