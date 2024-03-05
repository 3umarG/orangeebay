using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Orange_Bay.DTOs;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.Auth;
using Orange_Bay.Models.Security;
using Orange.EF;
using Orange.EF.Services;
using Serilog;

// Logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
try
{
    Log.Information("Starting the Web Application");
    var builder = WebApplication.CreateBuilder(args);

    //Read Configuration from appSettings
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    //Initialize Logger
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(config)
        .CreateLogger();

    builder.Host.UseSerilog();

    builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

// Configure Swagger 
    builder.Services.AddSwaggerGen(options =>
    {
        // Change the main schema for the Swagger
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Orange Bay API",
            Description = "Orange Bay Web API for Orange Bay Island",
        });

        // Add Global Authorization for all controllers with their end point
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
        });

        // Add Specific Authorization option for every end point
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
    });


    builder.Services.AddDbContext<ApplicationDbContext>(
        options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("SmarterConnection")!,
                b =>
                {
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly
                        .FullName);
                    b.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
        });

// For Identity Setup 
    builder.Services.AddAuthentication();
    builder.Services
        .AddIdentity<ApplicationUser, IdentityRole<int>>(options => { options.User.RequireUniqueEmail = false; })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    // Configure MailSettings for using SMTP google server
    builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

    // Add our Service(s) 
    builder.Services.AddTransient<IEmailService, EmailService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IContactUsService, ContactUsService>();
    builder.Services.AddScoped<IDashboardService, DashboardService>();
    builder.Services.AddScoped<IDiningService, DiningService>();
    builder.Services.AddScoped<IGalleryImagesService, GalleryImagesService>();
    builder.Services.AddScoped<IProgramsService, ProgramsService>();
    builder.Services.AddScoped<ISliderImagesService, SliderImagesService>();
    builder.Services.AddScoped<ICompaniesImagesService, CompaniesImagesService>();
    builder.Services.AddScoped<ITicketsService, TicketsService>();
    builder.Services.AddScoped<ImageSaver>();
    builder.Services.AddHostedService<ReservationCleanupService>();

// Mapping JWT values from appsettings.json to object
    builder.Services.Configure<Jwt>(builder.Configuration.GetSection("JWT"));
    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddScoped<ITokenService, TokenService>();


// Configure our Authentication Shared Schema 
    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;
            o.SaveToken = false;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                ClockSkew = TimeSpan.Zero
            };
        });

// Add Policy for being Admin and Manager
    builder.Services.AddAuthorization();

    var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
    app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.UseStaticFiles();

    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}