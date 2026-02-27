namespace PurseAccounting.Mobile.Infrastructure.ApiResults;

public record FailureResult : ApiResult
{
    public Exception Exception { get; }

    internal FailureResult(Exception exception)
    {
        Exception = exception;
    }

    public override TResult Match<TResult>(Func<TResult> onSuccess, Func<Exception, TResult> onFailure) => onFailure(Exception);

    public override Task<TResult> Await<TResult>(Func<Task<TResult>> onSuccess, Func<Exception, Task<TResult>> onFailure) => onFailure(Exception);
}
