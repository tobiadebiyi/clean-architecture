# Clean Architecture Template

## Why
I needed a way to implement and test Authorisation, Validation and Business logic in isolation and ended up with this.
If you find it painful on your project when trying to mock and stub authorization and validation logic in your business logic tests, then this may help you. Otherwise, this may not be for you.

## Setup

- Create a class library project for your usecases.
- In `Startup.cs -> ConfigureServices` add `services.AddCleanArchitecture("UsecaseProject.Assembly.Name");`.
- Add a usecase by extending `BaseInteractor<TRequest, TResponse>`.
- You can opt out of authorization and vaidation through the constructor; otherwise requests will fail if either one is not implemented in your usecase project.
- Add authorizer `IAuthorizer<in TRequest>`
- Add validator `IValidator<in TRequest>`
- Typically, a usecase's `TRequest, Interactor, [Authorizer, Validator, TResponse]` files should be co-located in a usecase folder. However, you may organise your code as you desire.

## Use
- In calling code (e.g. controller) inject `IRequestPipelineMediator` 
- Execute request `await this.instanceOfRequestPipelineMediator.Execute<TRequest, TResponse>(request);`

- Alternatively, inherit from `BaseRequestController` and 
    - execute `HandleRequest<TRequest, TResponse>(TRequest request)` 
    - or `HandleRequest<TRequest>(TRequest request)`


