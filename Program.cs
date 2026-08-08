using ApiEcommerce_VS.Constants;
using ApiEcommerce_VS.Data;
using ApiEcommerce_VS.Repository;
using ApiEcommerce_VS.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var dbConnectionString = builder.Configuration.GetConnectionString("ConexionSql");
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(dbConnectionString));

        builder.Services.AddResponseCaching(options =>
        {
            options.MaximumBodySize = 1024;
            options.UseCaseSensitivePaths = true;
        });

        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddAutoMapper(cfg =>
        {

            // cfg.AddProfile<CategoryProfile>();

            cfg.AddMaps(typeof(Program).Assembly);

        });
        var secretKey = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("SecretKey no esta configurada");
        }
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen(
          options =>
          {
              options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
              {
                  Description = "Nuestra API utiliza la Autenticación JWT usando el esquema Bearer. \n\r\n\r" +
                              "Ingresa la palabra a continuación el token generado en login.\n\r\n\r" +
                              "Ejemplo: \"12345abcdef\"",
                  Name = "Authorization",
                  In = ParameterLocation.Header,
                  Type = SecuritySchemeType.Http,
                  Scheme = "Bearer"
              });

              options.AddSecurityRequirement(document => new OpenApiSecurityRequirement()
              {
                  [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<String>()
              });
            //  options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            //    {
            //      {
            //        new OpenApiSecurityScheme
            //        {
            //          Reference = new OpenApiReference
            //          {
            //            Type = ReferenceType.SecurityScheme,
            //            Id = "Bearer"
            //          },
            //          Scheme = "oauth2",
            //          Name = "Bearer",
            //          In = ParameterLocation.Header
            //        },
            //      new List<string>()
            //    }
            //});
          }
        );

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }
            );
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(PolicyNames.AllowSpecificOrigin);
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}