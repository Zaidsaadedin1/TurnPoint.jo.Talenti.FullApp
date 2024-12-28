using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TurnPoint.Jo.APIs.Entities;
using TurnPoint.Jo.APIs.Interfaceses;
using TurnPoint.Jo.APIs.Services;
using TurnPoint.Jo.APIs.Validators.AuthenticationValidators;
using FluentValidation.AspNetCore;
using TurnPoint.Jo.APIs.Validators;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging to a file
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Register Serilog as the logging provider
builder.Host.UseSerilog();

// Register the identity services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.MigrationsAssembly("TernPoint.Jo.Talenti.DatabaseManager")
    ),
    ServiceLifetime.Scoped
);


// Register Identity services with a custom User and Role class
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Register other services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOtpService, OtpService>();

builder.Services.AddScoped<IInterestsLookupUserService, InterestsLookupUserService>();
builder.Services.AddScoped<IInterestsService, InterestsService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISmsService, SmsService>();

builder.Services.AddScoped<LoginDtoValidator>();
builder.Services.AddScoped<RegisterUserDtoValidator>();
builder.Services.AddScoped<ResetPasswordDtoValidator>();
builder.Services.AddScoped<VerifyOtpDtoValidator>();

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterUserDtoValidator>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginDtoValidator>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ResetPasswordDtoValidator>());
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<VerifyOtpDtoValidator>());

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add controllers
builder.Services.AddControllers();

// Register Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1",
        Description = "API for authentication and user management using JWT."
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] followed by your token.\r\nExample: \"Bearer abc123\""
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("TurnPoinTalentiTypePolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Specify allowed origins
               .AllowAnyHeader()     // Allow any headers
               .AllowAnyMethod()     // Allow any HTTP methods
               .AllowCredentials();  // Allow credentials (cookies, authorization headers, etc.)
    });
});



// Configure authentication
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration["JwtSettings:SecretKey"]; 
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new ArgumentNullException("JwtSettings:SecretKey", "JWT secret key is missing.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"], 
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
            RoleClaimType = "Roles"
        };
    });


var app = builder.Build();
app.Use(async (context, next) =>
{
    if (context.Request.Headers.ContainsKey("Authorization"))
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        try
        {
            var handler = new JwtSecurityTokenHandler();
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var payload = JsonSerializer.Serialize(jwtToken.Payload, new JsonSerializerOptions { WriteIndented = true });

                Console.WriteLine($"JWT Token Payload: {payload}");
            }
            else
            {
                Console.WriteLine("Invalid JWT token.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error decoding JWT: {ex.Message}");
        }
    }
    await next();
});
// Optional: Automatically migrate database
if (app.Configuration.GetValue<bool>("Migrate"))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTToken_Auth_API V1"));

// HTTPS redirection
app.UseHttpsRedirection();

// Routing
app.UseRouting();

// CORS must be placed before authentication and authorization
app.UseCors("TurnPoinTalentiTypePolicy");

// Authentication and Authorization
app.UseAuthentication(); // Adds the Authentication middleware
app.UseAuthorization(); // Adds the Authorization middleware

// Map controllers
app.MapControllers();

// Ensure logs are flushed when the application stops
app.Lifetime.ApplicationStopping.Register(() => Log.CloseAndFlush());

// Run the application
app.Run();
