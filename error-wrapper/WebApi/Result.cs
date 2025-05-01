namespace WebApi;

public sealed class Result<T>
{
    public T  Value { get; }
    public bool IsSuccess { get; private set; }
    public Error Error { get; }
    
    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
    }
    
    private Result(Error error)
    {
        Error = error;
        IsSuccess = false;
    }
    
    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);
}

public record Error(ErrorType type, string description)
{
    public static Error NotLineItems => new(ErrorType.Validation, "No line items in the order");
    public static Error NotEnoughStock => new(ErrorType.Validation, "Not enough stock");
    public static Error Unauthorized => new(ErrorType.Failure, "Unauthorized");
}

public enum ErrorType
{
    Failure = 0,
    Validation = 1
}