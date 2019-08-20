using CleanArchitecture.Application;
using CleanArchitecture.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.Controllers
{
    public abstract class BaseRequestController : Controller
    {
        private readonly IRequestPipelineMediator requestPipelineMediator;

        protected BaseRequestController(IRequestPipelineMediator requestHandler)
        {
            this.requestPipelineMediator = requestHandler;
        }

        protected async Task<IActionResult> HandleRequest<TRequest, TResponse>(TRequest request) where TRequest : class where TResponse : OperationResult
        {
            var result = await this.requestPipelineMediator.Execute<TRequest, TResponse>(request);

            if (result.IsForbidden())
            {
                return Forbid();
            }

            return Ok(result);
        }

        protected async Task<IActionResult> HandleRequest<TRequest>(TRequest request) where TRequest : class
        {
            return await HandleRequest<TRequest, OperationResult>(request);
        }
    }
}
