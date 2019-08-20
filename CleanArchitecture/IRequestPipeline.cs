using System.Threading.Tasks;
using CleanArchitecture.Validation;

namespace CleanArchitecture.Application
{
    public interface IRequestPipeline<in TRequest, TResponse>
        where TRequest : class
        where TResponse : OperationResult
    {
        Task<OperationResult> Execute(TRequest request);
    }
}