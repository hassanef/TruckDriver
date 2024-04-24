using TruckDriver.Api.GlobalException;
using TruckDriver.Application.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAzureKeyVault(builder.Configuration);
builder.Services.AddCosmosDB(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.ConfigureApiVersioning();
builder.Services.AddTruckDriverServices();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.MapControllers();
app.Run();
public partial class Program { }