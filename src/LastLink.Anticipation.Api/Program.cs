using LastLink.Anticipation.Application.UseCases;
using LastLink.Anticipation.Domain.Repositories;
using LastLink.Anticipation.Infrastructure.Data;
using LastLink.Anticipation.Infrastructure.Repositories;
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

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "LastLink Anticipation API v1"); });
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapControllers();
app.Run();
