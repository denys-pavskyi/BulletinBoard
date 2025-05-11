namespace BulletinBoard.WebClient.Models.Other;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Value { get; private set; }
    public string? ErrorMessage { get; private set; }

    public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
    public static Result<T> Failure(string errorMessage) => new Result<T> { IsSuccess = false, ErrorMessage = errorMessage };
}

public class Result
{
    public bool IsSuccess { get; }
    public ErrorResponse? Error { get; }

    protected Result(bool isSuccess, ErrorResponse? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true);

    public static Result Failure(ErrorResponse error) => new Result(false, error);

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<ErrorResponse, TResult> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(Error!);
    }
}