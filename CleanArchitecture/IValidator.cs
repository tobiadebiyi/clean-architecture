using System.Threading.Tasks;
using CleanArchitecture.Validation;

namespace CleanArchitecture.Application
{
    public interface IValidator<in TRequest>
    {
        Task<OperationResult> Validate(TRequest request);
    }
}
