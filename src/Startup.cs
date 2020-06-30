using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Consul;
using MedPark.Common;
using MedPark.Common.Consul;
using MedPark.Common.Handlers;
using MedPark.Common.Messages;
using MedPark.Common.RabbitMq;
using MedPark.CustomersService.Config;
using MedPark.CustomersService.Domain;
using MedPark.CustomersService.Dto;
using MedPark.CustomersService.Handlers.Customers;
using MedPark.CustomersService.Handlers.Gateway;
using MedPark.CustomersService.Messages.Commands;
using MedPark.CustomersService.Messages.Events;
using MedPark.CustomersService.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MedPark.CustomersService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IContainer Container { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {            
            //Auto Mapper
            services.AddAutoMapper(typeof(Startup));
            services.AddHealthChecks();
            services.AddConsul();

            //Add DBContext
            services.AddDbContext<CustomersDbContext>(options => options.UseSqlServer(Configuration["Database:ConnectionString"]));
            services.AddMvc(mvc => mvc.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<CustomersDbContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsImplementedInterfaces();
            builder.AddDispatchers();
            builder.AddRabbitMq();
            builder.AddRepository<Customer>();
            builder.AddRepository<Address>();
            builder.AddRepository<MedicalScheme>();
            builder.AddRepository<CustomerMedicalScheme>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, IServiceProvider serviceProvider, IHostApplicationLifetime lifetime, IConsulClient consulClient)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedData.EnsureSeedData(serviceProvider);
            }
            else
            {
                app.UseHsts(); 
            }           

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoit =>
            {
                endpoit.MapHealthChecks("/health");
            });
            app.UseHttpsRedirection();

            app.UseRabbitMq()
                .SubscribeCommand<CreateAddress>()
                .SubscribeCommand<UpdateCustomerDetails>()
                .SubscribeCommand<AddCustomerMedicalScheme>()
                .SubscribeEvent<SignedUp>(@namespace: "identity")
                .SubscribeEvent<AddressCreated>(@namespace: "gateway");

            var serviceID = app.UseConsul();
            lifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceID);
            });

            app.UseMvcWithDefaultRoute();
        }
    }
    
}
