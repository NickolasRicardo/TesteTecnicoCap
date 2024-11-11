using MediatR;
using Questao5.Infrastructure.Sqlite;
using System.Reflection;
using Questao5.Application.Services;
using Questao5.Domain.Interfaces.QueryStore;
using Questao5.Domain.Interfaces.Services;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();


builder.Services.AddScoped<IMovimentoQuery, MovimentoQuery>();
builder.Services.AddScoped<IMovimentoCommand, MovimentoCommand>();

builder.Services.AddScoped<IIdempotenciaQuery, IdempotenciaQuery>();
builder.Services.AddScoped<IIdempotenciaCommand, IdempotenciaCommand>();

builder.Services.AddScoped<IContaCorrenteQuery, ContaCorrenteQuery>();
builder.Services.AddScoped<IContaCorrenteCommand, ContaCorrenteCommand>();

builder.Services.AddScoped<IMovimentoService, MovimentoService>();
builder.Services.AddScoped<IContaCorrenteService, ContaCorrenteService>();
builder.Services.AddScoped<IIdempotenciaService, IdempotenciaService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();

