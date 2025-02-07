using Health_Med.Repository.Interface;
using System.Data;
using System.Data.SqlClient;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Adiciona o repositório MedicoRepository
        builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();

        // Registrar a conexão com o banco de dados
        builder.Services.AddScoped<IDbConnection>(sp =>
            new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    }
}