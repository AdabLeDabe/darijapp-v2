namespace SharedKernel;

public class Result<T>
{
    public ResultStatus Status { get; init; }
    
    public T? Content { get; init; }

    public static Result<T> NotFound()
    {
        return new Result<T>() { Status = ResultStatus.NotFound };
    }

    public static Result<T> Forbidden()
    {
        return new Result<T>() { Status = ResultStatus.Forbidden };
    }

    public static Result<T> Success(T content)
    {
        return new Result<T>() { Status = ResultStatus.Success, Content = content };
    }

    public static Result<T> Invalid()
    {
        return new Result<T>() { Status = ResultStatus.Invalid };
    }
}