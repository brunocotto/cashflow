using CashFlow.Api.Filters;
using CashFlow.Api.Middlewares;
using CashFlow.Application;
using CashFlow.Infrastructure;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(optins => optins.Filters.Add(typeof(ExceptionFilter)));

//Acessado atrav�s do m�todo de extens�o criado em DependecyInjectionExtension
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleaware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await MigrateDatabase();

app.Run();

// fun��o para automatizar o processo de inser��o das migrations no banco

async Task MigrateDatabase()
{
    await using var scope = app.Services.CreateAsyncScope();
    // passo o provedor de services para que seja poss�vel acessar CashFlowDbContext
    await DatabaseMigration.MigrationDatabase(scope.ServiceProvider); 
}
