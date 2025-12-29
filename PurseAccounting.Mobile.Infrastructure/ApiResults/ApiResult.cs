namespace PurseAccounting.Mobile.Infrastructure.ApiResults;

public abstract record ApiResult<T>
{
    public static ApiResult<T> Success(T value) => new SuccessResult<T>(value);

    public static ApiResult<T> Failure(Exception exception) => new FailureResult<T>(exception);

    public abstract TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Exception, TResult> onFailure);
}
