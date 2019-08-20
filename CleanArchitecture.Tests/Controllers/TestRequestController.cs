using CleanArchitecture.Application;
using CleanArchitecture.Controllers;
using CleanArchitecture.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.Tests.Controllers
{
    public partial class BaseControllerTests
    {
        public class TestRequestController : BaseRequestController
        {
            public TestRequestController(IRequestPipelineMediator requestHandler) : base(requestHandler)
            {
            }

            public async Task<IActionResult> TestHandleResuest<TRequest, TResponse>(TRequest request)
                where TRequest : class
                where TResponse : OperationResult
            {
                return await base.HandleRequest<TRequest, TResponse>(request);
            }
        }
    }
}