using Microsoft.EntityFrameworkCore;
using Uttt.Micro.Libro.Extenciones;
using Uttt.Micro.Libro.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// Agregar controladores
builder.Services.AddControllers();

// Configurar CORS para React (localhost:3009)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins() // puerto de React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configurar Swagger y OpenAPI
builder.Services.AddOpenApi();     // Extensión personalizada
builder.Services.AddSwaggerGen();  // Swagger oficial

// Configurar DbContext con cadena de conexión del appsettings.json
builder.Services.AddDbContext<ContextoLibreria>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));


// Servicios personalizados (validadores, etc.)
builder.Services.AddCustomServices(builder.Configuration);

// Construir app
var app = builder.Build();

// Activar Swagger
app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = "swagger"; // importante si se accede en /swagger
});

app.MapOpenApi(); // Si usas extensión OpenApi personalizada

// Habilitar CORS
app.UseCors("PermitirFrontend");

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
