using Marboket.Presentation.Extensions.ConfigureServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddPresentationServices();

var app = builder.Build();

app.UsePresentationServices();
app.UseInfrastructureServices();

app.Run();