using CleanArchitecture.Validation;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Application
{
    public class RequestPipelineMediator : IRequestPipelineMediator
    {
        private readonly IServiceProvider serviceProvider;

        public RequestPipelineMediator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<OperationResult> Execute<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : OperationResult
        {

            var pipeline = this.ResolveType<IRequestPipeline<TRequest, TResponse>>();
            return await pipeline.Execute(request);

            /* TODO This will become an inflection point for all requests,
            // so we can extend the interactor interface to return an AuditLogModel
            */
        }

        private TType ResolveType<TType>()
        {
            return (TType)serviceProvider.GetService(typeof(TType));
        }
    }
}