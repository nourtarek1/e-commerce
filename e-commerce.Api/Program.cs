
using Domain.Contracts;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Data;
using Persistence.Repositories;
using Persistence;
using Services.Abstractions;
using Services.Mapping_Profiles;
using Services.MappingProfiles;
using Services;
using Sherd;
using System.Security.Claims;
using System.Text;
using Domin.Contercts;

namespace e_commerce.Api
{
  public class Program
  {
    public static async Task Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.

      builder.Services.AddControllers();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();
      builder.Services.AddScoped<IServiceManager, ServiceManager>();
      builder.Services.AddScoped<IDbInitializer, DbInitializer>();
      builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
      builder.Services.AddScoped<IBasketRepository, BasketRepository>();
      builder.Services.AddScoped<PictureUrlResolver>();
      builder.Services.AddIdentity<AppUser, IdentityRole>()
      .AddEntityFrameworkStores<AuthDbContext>()
      .AddDefaultTokenProviders();


      builder.Services.AddAutoMapper(cfg => {
        cfg.AddProfile(new CategoryProfile());
        cfg.AddProfile(new ProductProfiles());
        cfg.AddProfile(new BasketProfile());
        cfg.AddProfile(new OrderProfiles());
      });




      #region ConnectionString
      builder.Services.AddDbContext<AuthDbContext>(options =>
      {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
      });

      builder.Services.AddDbContext<StoreDbContext>(options =>
      {
        options.UseSqlServer(builder.Configuration.GetConnectionString("StoreConnection"));
      });
      #endregion


      #region JwtOptions
      builder.Services.Configure<JwtOptions>(
      builder.Configuration.GetSection("JwtOptions"));

      var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();
      builder.Services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
          .AddJwtBearer(options =>
          {
            options.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuer = true,
              ValidateAudience = true,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,

              ValidIssuer = jwtOptions.Issuer,
              ValidAudience = jwtOptions.Audience,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),


            };
          });
      #endregion

      builder.Services.AddCors(options =>
      {
        options.AddPolicy("AllowFrontend",
            policy =>
            {
              policy.WithOrigins("http://localhost:3000") // رابط React
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
      });


      builder.Services.AddHttpContextAccessor();

      var app = builder.Build();

      #region Seeding
      using var scope = app.Services.CreateScope();
      var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
      await dbInitializer.InitializeAsync();
      await dbInitializer.InitializeIdentityAsync();
      #endregion


      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseCors("AllowFrontend");
      app.UseAuthentication(); 
      app.UseAuthorization();

      app.MapControllers();


      app.Run();
    }
  }
}
