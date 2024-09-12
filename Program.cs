using System.Text;
using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Repositories;
using kit_stem_api.Repositories.IRepositories;
using kit_stem_api.Services;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace kit_stem_api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add repositories
        builder.Services.AddScoped<ITokenRepository, TokenRepository>();

        // Add services
        builder.Services.AddScoped<IUserService, UserService>();

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
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

                            options.Events = new JwtBearerEvents()
                            {
                                OnMessageReceived = context =>
                                {
                                    context.Token = context.Request.Cookies["accessToken"];
                                    return Task.CompletedTask;
                                }
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
