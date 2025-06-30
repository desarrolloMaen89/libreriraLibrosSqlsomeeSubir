using Microsoft.EntityFrameworkCore;
using Uttt.Micro.Libro.Extenciones;
using Uttt.Micro.Libro.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores
builder.Services.AddControllers();

// Configurar CORS global para permitir todo (¡solo para desarrollo!)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configurar Swagger y OpenAPI
builder.Services.AddOpenApi();     // Extensión personalizada (si aplica)
builder.Services.AddSwaggerGen();  // Swagger oficial

// Configurar DbContext con cadena de conexión del appsettings.json
builder.Services.AddDbContext<ContextoLibreria>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));

// Servicios personalizados (validadores, etc.)
builder.Services.AddCustomServices(builder.Configuration);

var app = builder.Build();

// Activar Swagger
app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = "swagger";
});

app.MapOpenApi(); // Solo si usas esa extensión

// Usar CORS globalmente
app.UseCors("PermitirTodo");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
