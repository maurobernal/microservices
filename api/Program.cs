using api.Interfaces;
using api.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using StackExchange.Redis;

/* ========= Servicios e Inyecciones  ==================== */
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
Console.WriteLine("******************Configurando Servicios *******************");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//MS SQL
builder.Services.AddTransient<IAppDBContext, AppDBContext>();
builder.Services.AddDbContext<AppDBContext>(o => o.UseSqlServer("Server=172.0.0.161,14334;Database=API;user id=sa;password=M1sterPassw0rd!;Encrypt=true;TrustServerCertificate=true"));

//Redis

string endpoint = configuration.GetSection("Cache")!.GetValue("Endpoint",string.Empty)!;
builder.Services.AddSingleton<IConnectionMultiplexer>(o => ConnectionMultiplexer.Connect(new ConfigurationOptions() { 
 EndPoints = { endpoint }
}));
builder.Services.AddSingleton<ICacheService, RedisContext>();



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Host.UseSerilog((ctx, lc) =>
lc
    .WriteTo.Console()
    .WriteTo.Seq("http://127.0.0.1:5341")


    ); ;


//Rate limiting
builder.Services.AddRateLimiter(delegate(RateLimiterOptions options){
    options.AddFixedWindowLimiter("FixedWindows", delegate (FixedWindowRateLimiterOptions options)
    {
        options.QueueLimit = 2;
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(30);


    });

    options.AddSlidingWindowLimiter("SliderWindows", delegate( SlidingWindowRateLimiterOptions options) 
    { 
        options.QueueLimit = 2;
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(30);
        options.SegmentsPerWindow = 2;
    });

    options.AddConcurrencyLimiter("Concurrency", delegate (ConcurrencyLimiterOptions options) 
    {
        options.PermitLimit = 1;
        options.QueueLimit = 1;
    });

    options.OnRejected = delegate (OnRejectedContext context, CancellationToken cancellation)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        return new ValueTask();
    };
});

Console.WriteLine("******************Finalizado la configuración de servicios *******************");
/* ========= Construir el sitio ======================== */
var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseRateLimiter();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();
await Migrations(scope.ServiceProvider);



/* ========= Lanzo la aplicación ======================== */
app.Run();




async Task Migrations(IServiceProvider serviceProvider)
{
    var context= serviceProvider.GetService<AppDBContext>();
    var conn_appdb = context.Database.GetDbConnection();

    Console.WriteLine($"Conexión Actual AppDB: {conn_appdb.ToString()}  {Environment.NewLine}  {conn_appdb.ConnectionString}");
    Console.WriteLine("****************** Probando acceso  *******************");


    try
    {
        Console.WriteLine("Base Disponible de API:" + context.Database.CanConnect());
        context.Database.Migrate();

    }
    catch (Exception ex)
    {
        Console.WriteLine($"------ !!! ERROR connectando: {ex.Message}");
    }
   


}