namespace BulletinBoard.BLL.Other;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public ErrorResponse? Error { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(ErrorResponse error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(ErrorResponse error) => new(error);
}