using Newtonsoft.Json;

namespace CleanArchitecture.Validation
{
    public class OperationResult
    {
        public OperationResult(bool succeeded, params string[] errors)
        {
            this.Succeeded = succeeded;
            this.Errors = errors;
        }

        [JsonProperty]
        public bool Succeeded { get; private set; }

        [JsonProperty]
        public string[] Errors { get; private set; }

        public static OperationResult Failed => new OperationResult(false);
        public static OperationResult Success => new OperationResult(true);
    }
}
