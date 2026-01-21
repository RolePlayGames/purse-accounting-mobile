namespace PurseAccounting.Mobile.Infrastructure.ApiResults;

public record FailureResult<T> : ApiResult<T>
{
    public Exception Exception { get; }

    internal FailureResult(Exception exception)
    {
        Exception = exception;
    }

    public override TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Exception, TResult> onFailure) =>
        onFailure(Exception);
}
