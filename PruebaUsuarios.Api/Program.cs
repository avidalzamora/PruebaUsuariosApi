using PruebaUsuarios.Data;
using PruebaUsuarios.Data.Repositorios;
using PruebaUsuarios.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mySqlConfig = new MySqlConfig(builder.Configuration.GetConnectionString("MySqlConnection"));
builder.Services.AddSingleton(mySqlConfig);

builder.Services.Configure<Configuraciones>(builder.Configuration.GetSection("Configuraciones"));
builder.Services.AddScoped<IUsuario, UsuarioRepository>();
builder.Services.AddScoped<ICorreo, CorreoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
