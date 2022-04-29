using api.Interfaces;
using api.Context;
using Microsoft.EntityFrameworkCore;

/* ========= Servicios e Inyecciones  ==================== */
var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("******************Configurando Servicios *******************");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IAppDBContext, AppDBContext>();
builder.Services.AddDbContext<AppDBContext>(o => o.UseSqlServer("Server=mssql;Database=API;user id=sa;password=M1st2rPassw0rd!;"));




Console.WriteLine("******************Finalizado la configuración de servicios *******************");
/* ========= Construir el sitio ======================== */
var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


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