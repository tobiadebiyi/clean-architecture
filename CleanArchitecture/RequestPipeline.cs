using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using CleanArchitecture.Validation;

namespace CleanArchitecture.Application
{
    public class RequestPipeline<TRequest, TResponse> : IRequestPipeline<TRequest, TResponse>
        where TRequest : class
        where TResponse : OperationResult
    {
        private readonly IAuthorizer<TRequest> authorizer;
        private readonly IInteractor<TRequest, TResponse> interactor;
        private readonly ILogger<RequestPipeline<TRequest, TResponse>> logger;
        private readonly IValidator<TRequest> validator;

        public RequestPipeline(
            IInteractor<TRequest, TResponse> interactor,
            ILogger<RequestPipeline<TRequest, TResponse>> logger,
            IAuthorizer<TRequest> authorizer = null,
            IValidator<TRequest> validator = null)
        {
            this.interactor = interactor;
            this.logger = logger;
            this.authorizer = authorizer;
            this.validator = validator;
        }

        public async Task<OperationResult> Execute(TRequest request)
        {
            var requestName = typeof(TRequest).Name;

            if (this.interactor == null)
                throw new InvalidOperationException($"There is no interactor for [{requestName}]");

            if (this.authorizer == null && interactor.HasAuthorizer)
                throw new InvalidOperationException($"There is no authorizer for [{requestName}]");

            if (this.validator == null && interactor.HasValidator)
                throw new InvalidOperationException($"There is no validator for [{requestName}]");

            if (interactor.HasAuthorizer)
            {
                logger.LogInformation($"Authorizing [{requestName}]");

                var authorizationResult = await this.authorizer.Authorize(request);
                if (!authorizationResult.Succeeded)
                {
                    return OperationResultCreator.Forbidden(authorizationResult.Errors);
                }
            }

            if (interactor.HasValidator)
            {
                logger.LogInformation($"Validating [{requestName}]");

                var validationResult = await this.validator.Validate(request);
                if (!validationResult.Succeeded)
                {
                    return validationResult;
                }
            }

            logger.LogInformation($"Handling [{requestName}]");
            return await this.interactor.Handle(request);
        }
    }
}