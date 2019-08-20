using System.Threading.Tasks;
using CleanArchitecture.Validation;

namespace CleanArchitecture.Application
{
    public interface IInteractor<in TRequest, TResponse>
        where TRequest : class
        where TResponse : OperationResult
    {
        Task<TResponse> Handle(TRequest request);
        bool HasAuthorizer { get; }
        bool HasValidator { get; }
    }
}
