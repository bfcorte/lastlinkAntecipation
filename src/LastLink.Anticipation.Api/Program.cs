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
var app = builder.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
