var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
app.MapGet("/", () => "LastLink Anticipation API - setup");
app.MapControllers();
app.Run();
