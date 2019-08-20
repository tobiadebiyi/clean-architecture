using AutoFixture;
using CleanArchitecture.Validation;
using CleanArchitecture.Application;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Tests.Controllers
{
    public partial class BaseControllerTests : BaseUnitTest
    {
        private readonly IRequestPipelineMediator mediator;
        private readonly TestRequestController sut;

        public BaseControllerTests()
        {
            this.mediator = Substitute.For<IRequestPipelineMediator>();
            this.sut = new TestRequestController(this.mediator);
        }

        [Fact]
        public async Task HandleRequest_GivenThatMediatorExecutetionSucceded_ReturnsOkResponse()
        {
            // Arrange
            var request = this.fixture.Create<TestRequest>();
            this.mediator.Execute<TestRequest, OperationResult>(request)
                .Returns(OperationResultCreator.Success());

            // Act
            var response = await this.sut.TestHandleResuest<TestRequest, OperationResult>(request);

            // Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async Task HandleRequest_GivenThatMediatorExecutionIsForbidden_ReturnsForbiddenResponse()
        {
            // Arrange
            var request = this.fixture.Create<TestRequest>();
            this.mediator.Execute<TestRequest, OperationResult>(request)
                .Returns(OperationResultCreator.Forbidden());

            // Act
            var response = await this.sut.TestHandleResuest<TestRequest, OperationResult>(request);

            // Assert
            Assert.IsType<ForbidResult>(response);
        }

        [Fact]
        public async Task HandleRequest_GivenThatMediatorExecutionFailed_ReturnsOkResponse()
        {
            // Arrange
            var request = this.fixture.Create<TestRequest>();
            this.mediator.Execute<TestRequest, OperationResult>(request)
                .Returns(OperationResultCreator.Failure());

            // Act
            var response = await this.sut.TestHandleResuest<TestRequest, OperationResult>(request);

            // Assert
            Assert.IsType<OkObjectResult>(response);
        }
    }
}