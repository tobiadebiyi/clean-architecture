using AutoFixture;
using CleanArchitecture.Application;
using CleanArchitecture.Validation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.Tests
{
    public class RequestPipelineTests : BaseUnitTest
    {
        private readonly IAuthorizer<TestRequest> authorizer;
        private readonly IInteractor<TestRequest, OperationResult> interactor;
        private readonly ILogger<RequestPipeline<TestRequest, OperationResult>> logger;
        private readonly RequestPipeline<TestRequest, OperationResult> sut;
        private readonly IValidator<TestRequest> validator;

        public RequestPipelineTests()
        {
            this.interactor = Substitute.For<IInteractor<TestRequest, OperationResult>>();
            this.interactor.Handle(Arg.Any<TestRequest>()).Returns(OperationResultCreator.Success());

            this.logger = Substitute.For<ILogger<RequestPipeline<TestRequest, OperationResult>>>();

            this.authorizer = Substitute.For<IAuthorizer<TestRequest>>();
            this.authorizer.Authorize(Arg.Any<TestRequest>()).Returns(OperationResultCreator.Success());

            this.validator = Substitute.For<IValidator<TestRequest>>();
            this.validator.Validate(Arg.Any<TestRequest>()).Returns(OperationResultCreator.Success());

            this.sut = new RequestPipeline<TestRequest, OperationResult>(this.interactor, this.logger, this.authorizer, this.validator);
        }

        [Fact]
        public async Task Execute_GivenThatAllOperationsAreSucceful_ReturnsSuccessfulOperationResult()
        {
            // Arrange
            this.interactor.HasValidator.Returns(true);
            this.interactor.HasAuthorizer.Returns(true);

            var request = this.fixture.Create<TestRequest>();

            // Act
            var result = await this.sut.Execute(request);

            // Assert
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task Execute_GivenThatAuthorizationFailes_ReturnsForbiddenOperationResult()
        {
            // Arrange
            this.interactor.HasAuthorizer.Returns(true);
            this.authorizer.Authorize(Arg.Any<TestRequest>()).Returns(OperationResultCreator.Failure());

            var request = this.fixture.Create<TestRequest>();

            // Act
            var result = await this.sut.Execute(request);

            // Assert
            Assert.IsType<ForbiddenOperationResult>(result);
        }

        [Fact]
        public async Task Execute_GivenThatInteractorHasAuthorizer_CallsAuthorize()
        {
            // Arrange
            this.interactor.HasAuthorizer.Returns(true);
            var request = this.fixture.Create<TestRequest>();

            // Act
            await this.sut.Execute(request);

            // Assert
            await this.authorizer.Received(1).Authorize(request);
        }

        [Fact]
        public async Task Execute_GivenThatInteractorHasAuthorizer_WhenNoAuthorizerIsInjected_ThrowsAnException()
        {
            // Arrange
            this.interactor.HasAuthorizer.Returns(true);
            var testSubject = new RequestPipeline<TestRequest, OperationResult>(
                interactor: this.interactor,
                logger: this.logger,
                authorizer: null,
                validator: this.validator);

            var request = this.fixture.Create<TestRequest>();

            // Act Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await testSubject.Execute(request));
        }

        [Fact]
        public async Task Execute_GivenThatInteractorHasValidator_CallsValidate()
        {
            // Arrange
            this.interactor.HasValidator.Returns(true);
            var request = this.fixture.Create<TestRequest>();

            // Act
            await this.sut.Execute(request);

            // Assert
            await this.validator.Received(1).Validate(request);
        }

        [Fact]
        public async Task Execute_GivenThatInteractorHasValidator_WhenNoAuthorizerIsInjected_ThrowsAnException()
        {
            // Arrange
            this.interactor.HasValidator.Returns(true);
            var testSubject = new RequestPipeline<TestRequest, OperationResult>(
                interactor: this.interactor,
                logger: this.logger,
                authorizer: this.authorizer,
                validator: null);

            var request = this.fixture.Create<TestRequest>();

            // Act Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await testSubject.Execute(request));
        }

        [Fact]
        public async Task Execute_GivenThatIntractorFailes_ReturnsFailedOperationResult()
        {
            // Arrange
            this.interactor.Handle(Arg.Any<TestRequest>()).Returns(OperationResultCreator.Failure());

            var request = this.fixture.Create<TestRequest>();

            // Act
            var result = await this.sut.Execute(request);

            // Assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public async Task Execute_GivenThatNoInteractorIsInjected_ThrowsAnException()
        {
            // Arrange
            var testSubject = new RequestPipeline<TestRequest, OperationResult>(
                interactor: null,
                logger: this.logger,
                authorizer: this.authorizer,
                validator: this.validator);

            var request = this.fixture.Create<TestRequest>();

            // Act Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await testSubject.Execute(request));
        }

        [Fact]
        public async Task Execute_GivenThatValidationFailes_ReturnsFailedOperationResult()
        {
            // Arrange
            this.interactor.HasValidator.Returns(true);
            this.validator.Validate(Arg.Any<TestRequest>()).Returns(OperationResultCreator.Failure());

            var request = this.fixture.Create<TestRequest>();

            // Act
            var result = await this.sut.Execute(request);

            // Assert
            Assert.False(result.Succeeded);
        }
    }
}