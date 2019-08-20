using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application
{
    public static class ApplicationExtensions
    {
        public static void AddCleanArchitecture(this IServiceCollection services, string applicationAssemblyName)
        {
            services.AddTransient(typeof(IRequestPipeline<,>), typeof(RequestPipeline<,>));
            services.AddTransient<IRequestPipelineMediator, RequestPipelineMediator>();

            var applicationAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(assembly => assembly.GetName().Name == applicationAssemblyName);

            services.Scan(scan =>
                scan.FromAssemblies(applicationAssembly)
                    .AddClasses(classes => classes.AssignableTo(typeof(IInteractor<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                    .AddClasses(classes => classes.AssignableTo(typeof(IAuthorizer<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                    .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                );
        }
    }
}