using System.Text;
using System.Text.Json.Serialization;
using Google.Cloud.Storage.V1;
using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Repositories;
using kit_stem_api.Repositories.IRepositories;
using kit_stem_api.Services;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace kit_stem_api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"./googleCloudStorage.json");

        // Add repositories
        builder.Services.AddScoped<ITokenRepository, TokenRepository>();
        builder.Services.AddScoped<ILabRepository, LabRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IComponentTypeRepository, ComponentTypeRepository>();

        // Add services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ILabService, LabService>();
        builder.Services.AddSingleton<IFirebaseService>(s => new FirebaseService(StorageClient.Create()));

        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IComponentTypeService, ComponentTypeService>();

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
        builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles); ;
        builder.Services.AddDbContext<KitStemDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("KitStemHubDb"));
        });
        builder.Services.AddCors(options =>
            {
                options.AddPolicy("testCorsApp", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                });
            });
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                        {
                            options.Password.RequireDigit = true;
                            options.Password.RequiredLength = 8;
                            options.Password.RequireNonAlphanumeric = true;
                            options.Password.RequireUppercase = true;
                            options.Password.RequireLowercase = true;
                            options.Password.RequiredUniqueChars = 1;
                        })
                        .AddDefaultTokenProviders()
                        .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("KitStemHub")
                        .AddEntityFrameworkStores<KitStemDbContext>();
        builder.Services.AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(options =>
                        {
                            var _config = builder.Configuration;
                            options.RequireHttpsMetadata = false;
                            options.TokenValidationParameters = new TokenValidationParameters()
                            {
                                ClockSkew = TimeSpan.Zero,
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                RequireAudience = true,
                                RequireExpirationTime = true,
                                RequireSignedTokens = true,
                                ValidAudience = _config["Jwt:Audience"],
                                ValidIssuer = _config["Jwt:Issuer"],
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new ArgumentException()))
                            };
                        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("testCorsApp");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
