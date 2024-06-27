using Core.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Core
{
    public static class ModuleICoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            //Configuration Of Mediator
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            //Configuration Of Auto Mapper 
            services.AddAutoMapper(Assembly.GetExecutingAssembly());



            // Get Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            // services.AddValidatorsFromAssembly();


            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));




            return services;
        }

    }
}
