using System.Threading.Tasks;
using CleanArchitecture.Validation;

namespace CleanArchitecture.Application
{
    public abstract class BaseInteractor<TRequest, TResponse> : IInteractor<TRequest, TResponse>
        where TRequest : class
        where TResponse : OperationResult
    {
        protected BaseInteractor(bool hasAuthorizer = true, bool hasValidator = true)
        {
            this.HasAuthorizer = hasAuthorizer;
            this.HasValidator = hasValidator;
        }
        public bool HasAuthorizer { get; }

        public bool HasValidator { get; }

        public abstract Task<TResponse> Handle(TRequest request);
    }
}