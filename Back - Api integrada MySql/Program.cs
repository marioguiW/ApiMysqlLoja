using System.Security.Cryptography.X509Certificates;
using Back___Api_integrada_MySql.Data;

namespace Back___Api_integrada_MySql;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<LojaContexto>();

        builder.Services.AddControllers();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder => builder
                .AllowAnyOrigin() // Permitir qualquer origem
                .AllowAnyMethod() // Permitir qualquer método HTTP
                .AllowAnyHeader() // Permitir qualquer cabeçalho
            );
        });


        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors();

        app.MapControllers();

        app.Run();


        Insere();
    }

    public static void Insere()
    {

    }
}