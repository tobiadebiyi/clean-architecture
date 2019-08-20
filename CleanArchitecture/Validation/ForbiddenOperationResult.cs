namespace CleanArchitecture.Validation
{
    public class ForbiddenOperationResult : OperationResult
    {
        public ForbiddenOperationResult(bool succeeded, params string[] errors) : base(succeeded, errors)
        {
        }
    }
}