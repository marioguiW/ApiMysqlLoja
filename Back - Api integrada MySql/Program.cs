using System.Security.Cryptography.X509Certificates;
using System.Text;
using Back___Api_integrada_MySql.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Back___Api_integrada_MySql;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<LojaContexto>();

        builder.Services.AddControllers();

        var key = Encoding.ASCII.GetBytes(Settings.Secret);

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

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

        app.UseAuthentication();

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