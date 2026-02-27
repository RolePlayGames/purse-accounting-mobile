namespace PurseAccounting.Mobile.Infrastructure.ApiResults;

public record SuccessResult : ApiResult
{
    public override TResult Match<TResult>(Func<TResult> onSuccess, Func<Exception, TResult> onFailure) => onSuccess();

    public override Task<TResult> Await<TResult>(Func<Task<TResult>> onSuccess, Func<Exception, Task<TResult>> onFailure) => onSuccess();
}
