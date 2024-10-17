using AJTarefasDomain.Interfaces.Negocio.Projeto;
using AJTarefasDomain.Interfaces.Repositorio.Projeto;
using AJTarefasNegocio.Projeto;
using AJTarefasRecursos.Repositorios.Projeto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProjetoService, ProjetoNegocio>();
builder.Services.AddScoped<IProjetoRepositorio, ProjetoRepositorio>();

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

app.Run();
