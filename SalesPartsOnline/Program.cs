using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SalesPartsOnline;
using Serilog;
using System.Text;
using Asp.Versioning;
using Microsoft.OpenApi.Models;
using SalesPartsOnline.DAL;
using Microsoft.EntityFrameworkCore;
using SalesPartsOnline.Data;
//Server=localhost;Database=master;Trusted_Connection=True;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(@"C:\Logs\productinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
.AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
});
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SPODbContext>(options =>

    options.UseSqlServer(connectionString)
);


builder.Services.AddSingleton<BasicAuthenticationHandler>();
builder.Services.AddScoped<ICrudOperationsDL, CRUDOperationsDL>();
builder.Services.AddProblemDetails();


// JWT Authentication Configuration
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null)
    .AddJwtBearer(options =>
    {
        var key = Encoding.ASCII.GetBytes("YourSuperSecretKeyHere");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourissuer",
            ValidAudience = "youraudience",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddApiVersioning( setupAction =>
{
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1.0);
}).AddMvc();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<SPODbContext>();

    dbContext.Database.EnsureCreated(); // Ensures the database is created (if it doesn't exist)
    dbContext.SeedData(); // Call the SeedData method to seed the database
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
