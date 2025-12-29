namespace PurseAccounting.Mobile.Infrastructure.ApiResults;

public record SuccessResult<T> : ApiResult<T>
{
    public T Value { get; }

    internal SuccessResult(T value)
    {
        Value = value;
    }

    public override TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Exception, TResult> onFailure) =>
        onSuccess(Value);
}
