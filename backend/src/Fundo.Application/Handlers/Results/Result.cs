namespace Fundo.Application.Handlers.Results
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }

        public bool IsFailure => !IsSuccess;

        public Error? Error { get; }

        public Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != null)
            {
                throw new ArgumentException("A successful result cannot have an error.", nameof(error));
            }
            Error = error;
            IsSuccess = isSuccess;
        }

        public static Result<T> Success<T>(T value) => new Result<T>(value);

        public static Result<T> Failure<T>(Error error) => new Result<T>(false, error);
    }

    public class Result<TValue> : Result
    {
        public TValue? Value { get; }

        public Result(TValue value) : base(true, null!)
        {
            ArgumentNullException.ThrowIfNull(value);
            Value = value;
            IsSuccess = true;
        }

        public Result(bool isSuccess, Error error) : base(isSuccess, error)
        {
        }
    }
}