using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class OperationResult<T>
    {
        public bool Succeeded { get; set; }
        public IEnumerable<ValidationError> Errors { get; set; } = new List<ValidationError>();
        public T Result { get; set; }

        public static OperationResult<T> Success(T result = default(T)) =>
            new OperationResult<T>
            {
                Succeeded = true,
                Result = result
            };

        public static OperationResult<T> Error(ValidationError error) =>
            new OperationResult<T>
            {
                Succeeded = false,
                Errors = new List<ValidationError> { error }
            };

        public static OperationResult<T> Error(IEnumerable<ValidationError> errors) =>
            new OperationResult<T>
            {
                Succeeded = false,
                Errors = errors
            };
    }

    /// <summary>
    /// Model returned from CommandHandlers for password recovery
    /// </summary>
    public class OperationResult : OperationResult<object>
    {
        public static OperationResult Success() =>
            new OperationResult
            {
                Succeeded = true
            };

        public new static OperationResult Error(ValidationError error) =>
            new OperationResult
            {
                Succeeded = false,
                Errors = new List<ValidationError> { error }
            };

        public new static OperationResult Error(IEnumerable<ValidationError> errors) =>
            new OperationResult
            {
                Succeeded = false,
                Errors = errors
            };
    }
}
