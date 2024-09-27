using System.Text;
using System.Text.Json.Serialization;
using Google.Cloud.Storage.V1;
using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Repositories;
using kit_stem_api.Repositories.IRepositories;
using kit_stem_api.Services;
using kit_stem_api.Services.IServices;
using kit_stem_api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        builder.Services.AddScoped<UnitOfWork>();
        builder.Services.AddScoped<ITokenRepository, TokenRepository>();

        // Add services
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ILabService, LabService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IComponentTypeService, ComponentTypeService>();
        builder.Services.AddScoped<IComponentService, ComponentService>();
        builder.Services.AddScoped<IPackageService, PackageService>();
        builder.Services.AddSingleton<IEmailService>(s => new GmailService(builder.Configuration));
        builder.Services.AddSingleton<IGoogleService>(s => new GoogleService(builder.Configuration));
        builder.Services.AddSingleton<IFirebaseService>(s => new FirebaseService(StorageClient.Create()));

        // Add services to the container.
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
        builder.Services.AddAuthorization();
        builder.Services.AddMvc().ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                // Extract validation errors and customize response structure
                var errors = context.ModelState
                    .Where(m => m.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key.ToLower(),  // Convert key (property name) to lowercase
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()  // Get the first error message
                    );

                // Define the custom error response structure
                var errorResponse = new
                {
                    status = "fail",
                    details = new
                    {
                        message = "Thông tin yêu cầu không chính xác!",
                        errors
                    }
                };

                // Return a BadRequest with the custom response
                return new BadRequestObjectResult(errorResponse);
            };
        });
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
                options.AddPolicy("ClientCors", policy =>
                {
                    policy.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
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

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors("ClientCors");
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
