using TruckDriver.Api.GlobalException;
using TruckDriver.Application.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();


//var configuration = builder.Configuration.AddAzureKeyVault(
//    vaultUri: new Uri(builder.Configuration[AzureKeys.KeyVaultUri]),
//    credential: new DefaultAzureCredential()
//).Build();

builder.Services.ConfigureAzureKeyVault(builder.Configuration);
builder.Services.ConfigureCosmosDB(builder);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureAuthentication(builder);
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