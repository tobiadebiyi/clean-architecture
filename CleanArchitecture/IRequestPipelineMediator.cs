using System.Threading.Tasks;
using CleanArchitecture.Validation;

namespace CleanArchitecture.Application
{
    public interface IRequestPipelineMediator
    {
        Task<OperationResult> Execute<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : OperationResult;
    }
}