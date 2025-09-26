using ClasOhlsonCloneApi;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.Negotiate;
// Console.WriteLine("MYSQL_PASSWORD fra launchSettings: " + Environment.GetEnvironmentVariable("MYSQL_PASSWORD"));

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Configuration.AddUserSecrets<Program>();

// Hent passord fra user-secrets eller env
var mysqlPassword = builder.Configuration["MYSQL_PASSWORD"];

// Sett opp connection string manuelt
var connectionString = $"server=localhost;database=test_schema;user=apiuser;password={mysqlPassword};";

// Legg til DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();
app.Run();



// // Denne legger til Swagger
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
//
// // Legg til Controller-støtte
// builder.Services.AddControllers();
// builder.Services.AddCors(); // 👈 legger til CORS
//
// var app = builder.Build();
// Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
// // Bruk Swagger i utviklingsmodus
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
//     //Haha denne fikset det slik at front og api kan snakke sammen!!!!!
//     app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
// }
//
// // Map alle Controller-ruter
// app.MapControllers();
// app.Lifetime.ApplicationStarted.Register(() =>
// {
// foreach (var address in app.Urls)
// {
//     Console.WriteLine($"Swagger kjører på adresse: {address}/swagger");
// }
// });
//
// // Start webserver
// app.Run();
