using ApiEcommerce_VS.Constants;
using ApiEcommerce_VS.Data;
using ApiEcommerce_VS.Repository;
using ApiEcommerce_VS.Repository.IRepository;
using Asp.Versioning;
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
            options.MaximumBodySize = 1024 * 1024; // los primeros 1024 hacen referencia a Bites por eso los multiplicamos por 1024 para tener 1 MB de cache
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

        //Configuración de perfiles de Caché
        // En este caso configuramos 2 perfiles uno a 10 segundos y otro a 20 segundos
        // Importante tomar en cuenta que cuando vayamos a utilizar uno de los perfiles lo haremos con el nombre que colocamos
        builder.Services.AddControllers(options =>
        {
            options.CacheProfiles.Add(CacheProfiles.Default10, CacheProfiles.Profile10);
            options.CacheProfiles.Add(CacheProfiles.Default20, CacheProfiles.Profile20);
        }
            
        );
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

        var apiVersionenBuilder = builder.Services.AddApiVersioning(option =>
        {
            option.AssumeDefaultVersionWhenUnspecified = true;
            option.DefaultApiVersion = new ApiVersion(1, 0);
            option.ReportApiVersions = true;
            option.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version"));
        });

        apiVersionenBuilder.AddApiExplorer(option =>
        {
            option.GroupNameFormat = "'v'VVV";
            option.SubstituteApiVersionInUrl = true;
        });

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