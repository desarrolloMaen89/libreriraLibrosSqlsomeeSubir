﻿using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Uttt.Micro.Libro.Aplicacion;
using Uttt.Micro.Libro.Persistencia;
using Microsoft.EntityFrameworkCore;



namespace Uttt.Micro.Libro.Extenciones
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            services.AddDbContext<ContextoLibreria>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            ////Agregar servicios de CORS
            //services.AddCors(options =>
            //{

            //})

            //agregamos MediaR como servicio
            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);
            services.AddAutoMapper(typeof(Consulta.Manejador));

            return services;
        }
    }
}
