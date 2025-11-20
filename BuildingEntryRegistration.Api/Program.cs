using BuildingEntryRegistration.Api.Services;
using Microsoft.AspNetCore.DataProtection;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var keyRingPath = configuration["DataProtection:KeyRingPath"];
if (string.IsNullOrEmpty(keyRingPath))
{
    keyRingPath = Path.Combine(AppContext.BaseDirectory, "keys");
}
Directory.CreateDirectory(keyRingPath);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keyRingPath))
    .SetApplicationName("BuildingEntryRegistrationApi");


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


builder.Services.AddSingleton<IStorageService, InMemoryStorageService>();
builder.Services.AddScoped<ICheckInService, CheckInService>();

var app = builder.Build();

var dataProtectionProvider = app.Services.GetRequiredService<IDataProtectionProvider>();
var protector = dataProtectionProvider.CreateProtector("ConfigProtector.v1");

var protectedToken = configuration["ProtectedSettings:ProtectedToken"];
string unprotectedToken = string.Empty;
if (!string.IsNullOrEmpty(protectedToken))
{
    try
    {
        unprotectedToken = protector.Unprotect(protectedToken);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Unable to unprotect ProtectedToken. Make sure key ring used to protect value is the same as current key ring.");
    }
}

if (!string.IsNullOrEmpty(unprotectedToken))
{
    configuration["RuntimeSettings:UnprotectedToken"] = unprotectedToken;
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.InjectJavascript("/swagger/custom-auth.js");
    });
}

app.UseCors("AllowAngular");


app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? string.Empty;

//authorization to the paths starting with /api
    if (!path.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    var expected = app.Configuration["RuntimeSettings:UnprotectedToken"] ?? string.Empty;
    if (string.IsNullOrEmpty(expected))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new { error = "No runtime token configured" });
        return;
    }

    if (!context.Request.Headers.TryGetValue("Authorization", out var header))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new { error = "Missing Authorization header" });
        return;
    }

    var auth = header.ToString();
    const string bearerPrefix = "Bearer ";
    if (!auth.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new { error = "Invalid Authorization header format" });
        return;
    }

    var token = auth.Substring(bearerPrefix.Length).Trim();
    if (token != expected)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new { error = "Invalid token" });
        return;
    }

    await next();
});

app.MapControllers();

app.Run();