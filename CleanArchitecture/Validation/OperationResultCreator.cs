using System;
using CleanArchitecture.Validation;

namespace CleanArchitecture.Validation
{
    public static class OperationResultCreator
    {
        public static OperationResult Success()
        {
            return new OperationResult(succeeded: true);
        }

        public static OperationResult<TResult> Success<TResult>(TResult result)
        {
            return new OperationResult<TResult>(result, success: true);
        }

        public static OperationResult Failure(params string[] errors)
        {
            return new OperationResult(succeeded: false, errors: errors);
        }

        public static OperationResult<TResult> Failure<TResult>(params string[] errors)
        {
            return new OperationResult<TResult>(value: default(TResult), success: false, errors: errors);
        }

        public static OperationResult<TResult> Failure<TResult>(TResult value, params string[] errors)
        {
            return new OperationResult<TResult>(value, success: false, errors: errors);
        }

        public static ForbiddenOperationResult Forbidden(params string[] errors)
        {
            return new ForbiddenOperationResult(false, errors);
        }

        public static bool IsForbidden(this OperationResult result)
        {
            return result is ForbiddenOperationResult;
        }
    }
}