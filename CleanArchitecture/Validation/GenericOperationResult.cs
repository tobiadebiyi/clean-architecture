using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Validation
{
    public class OperationResult<TResult> : OperationResult
    {
        public OperationResult(TResult value, bool success, params string[] errors) : base(success, errors)
        {
            this.Value = value;
        }

        [JsonProperty]
        public TResult Value { get; private set; }
    }
}
