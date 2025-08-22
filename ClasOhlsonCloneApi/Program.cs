using Microsoft.AspNetCore.Authentication.Negotiate;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
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
            policy.WithOrigins("http://localhost:5173") // din Vue-app
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
// app.UseHttpsRedirection();
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
