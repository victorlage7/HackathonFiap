using Health_Med.Repository;
using Health_Med.Repository.Interface;
using Health_Med.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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

        // Adiciona o repositórios
        builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
        builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
        builder.Services.AddScoped<IHorarioDisponivelRepository, HorarioDisponivelRepository>();


        // Configurar JWT
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        }
        );

        //Adicionar Serviços
        builder.Services.AddScoped<JwtService>();

        // Adicionar configuração do Swagger para autenticação via JWT
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HealthMed API",
                Version = "v1",
                Description = "API para gerenciamento de consultas médicas",
                Contact = new OpenApiContact
                {
                    Name = "Hackathon FIAP",
                    Email = "contato@hackathonfiap.com"
                }
            });

            // Configuração para suporte a JWT no Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira seu token JWT no campo abaixo (sem 'Bearer ')"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                 {
                    new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
            },
            new string[] {}
        }
    });
        });



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