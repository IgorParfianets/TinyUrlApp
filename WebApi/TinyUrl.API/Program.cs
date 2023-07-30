using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Reflection;
using System.Text;
using TinyUrl.API.Utils;
using TinyUrl.Business.Services;
using TinyUrl.Core.Abstractions;
using TinyUrl.CQS.Handlers.CommandHandlers;
using TinyUrl.CQS.Handlers.QueryHandlers;
using TinyUrl.CQS.Queries;
using TinyUrl.Database;

namespace TinyUrl.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .WriteTo.File(GetPathToLogFile(),
                    LogEventLevel.Information));

            string myCorsPolicyName = "ReactApp";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(myCorsPolicyName, policyBuilder =>
                {
                    policyBuilder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<TinyUrlContext>(
                optionBuilder => optionBuilder.UseSqlServer(connectionString));
            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = builder.Configuration["Token:Issuer"],
                        ValidAudience = builder.Configuration["Token:Issuer"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:JwtSecret"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Add business services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUrlService, UrlService>();
            builder.Services.AddScoped<IJwtUtil, JwtUtil>();

            // Add mediator handlers 
            builder.Services.AddMediatR(typeof(AddUserCommandHandler).Assembly);
            builder.Services.AddMediatR(typeof(AddRefreshTokenCommandHandler).Assembly);
            builder.Services.AddMediatR(typeof(DeleteRefreshTokenCommandHandler).Assembly);
            builder.Services.AddMediatR(typeof(GetUserByEmailQueryHandler).Assembly);
            builder.Services.AddMediatR(typeof(GetUserByRefreshTokenQueryHandler).Assembly);
            builder.Services.AddMediatR(typeof(AddUrlCommandHandler).Assembly);
            builder.Services.AddMediatR(typeof(GetUrlByAliasQueryHandler).Assembly);
            builder.Services.AddMediatR(typeof(DeleteUrlCommandByAliasHandler).Assembly); 
            builder.Services.AddMediatR(typeof(GetAllUrlsByUserIdQueryHandler).Assembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        /// <summary>
        ///     Returns the path for log file recording.
        /// </summary>
        /// <returns>A string whose value contains a path to the log file</returns>
        private static string GetPathToLogFile()
        {
            var sb = new StringBuilder();
            sb.Append(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            sb.Append(@"\logs\");
            sb.Append($"{DateTime.Now:yyyyMMddhhmmss}");
            sb.Append("data.log");
            return sb.ToString();
        }
    }
}