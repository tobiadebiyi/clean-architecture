using System.Threading.Tasks;
using CleanArchitecture.Validation;

namespace CleanArchitecture.Application
{
    public interface IAuthorizer<in TRequest>
        where TRequest : class
    {
        Task<OperationResult> Authorize(TRequest request);
    }
}
