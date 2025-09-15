using LastLink.Anticipation.Application.UseCases;
using LastLink.Anticipation.Domain.Repositories;
using LastLink.Anticipation.Infrastructure.Data;
using LastLink.Anticipation.Infrastructure.Repositories;
using LastLink.Anticipation.Api.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AnticipationDbContext>(o => o.UseInMemoryDatabase("anticipation-db"));

builder.Services.AddScoped<IAnticipationRepository, AnticipationRepository>();
builder.Services.AddScoped<CreateAnticipationHandler>();
builder.Services.AddScoped<ListByCreatorHandler>();
builder.Services.AddScoped<ApproveRejectHandler>();

// CORS para o front local (Vite)
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy => policy
        .WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "LastLink Anticipation API v1"); });
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("Frontend");

app.MapControllers();
app.Run();
