namespace PurseAccounting.Mobile.Infrastructure.ApiResults;

public abstract record ApiResult
{
    public static ApiResult Success() => new SuccessResult();

    public static ApiResult Failure(Exception exception) => new FailureResult(exception);

    public abstract TResult Match<TResult>(Func<TResult> onSuccess, Func<Exception, TResult> onFailure);

    public abstract Task<TResult> Await<TResult>(Func<Task<TResult>> onSuccess, Func<Exception, Task<TResult>> onFailure);
}
