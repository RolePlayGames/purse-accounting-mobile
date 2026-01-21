using PurseAccounting.Mobile.Infrastructure.ApiResults;
using PurseAccounting.Mobile.Infrastructure.ServerResults;
using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.Base;

internal abstract class ClientBase
{
    protected delegate Task<HttpResponseMessage> ExternalCall(string url, object request, CancellationToken cancellationToken);

    protected delegate Task<ApiResult<T>> ParseError<T>(HttpResponseMessage message, CancellationToken cancellationToken);

    protected static Task<ApiResult<T>> SafeCall<T>(ExternalCall call, string url, object request, CancellationToken cancellationToken)
    {
        return SafeCall<T>(call, url, request, null, cancellationToken);
    }

    protected static Task<ApiResult<T>> SafeCall<T, TErrorCode>(ExternalCall call, string url, object request, CancellationToken cancellationToken)
    {
        return SafeCall(call, url, request, ParseErrorServerResponse<T, TErrorCode>, cancellationToken);
    }

    private static async Task<ApiResult<T>> ParseErrorServerResponse<T, TErrorCode>(HttpResponseMessage message, CancellationToken cancellationToken)
    {
        var value = await message.Content.ReadFromJsonAsync<ServerErrorResponse<TErrorCode>>(cancellationToken).ConfigureAwait(false);

        if (value is null)
            return ApiResult<T>.Failure(new InvalidOperationException($"Http error response of type {nameof(T)} deserialized to null."));

        return ApiResult<T>.Failure(new ServerException<TErrorCode>() { NoticeType = value.NoticeType });
    }

    private static async Task<ApiResult<T>> SafeCall<T>(ExternalCall call, string url, object request, ParseError<T>? errorResponseFactory, CancellationToken cancellationToken)
    {
        try
        {
            var response = await call(url, request, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var value = await response.Content.ReadFromJsonAsync<T>(cancellationToken).ConfigureAwait(false);

                return value is null
                    ? ApiResult<T>.Failure(new InvalidOperationException($"Http response of type {nameof(T)} deserialized to null."))
                    : ApiResult<T>.Success(value);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity && errorResponseFactory is not null)
            {
                return await errorResponseFactory(response, cancellationToken);
            }

            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var message = $"Ошибка сервера: {(int)response.StatusCode}";

            if (!string.IsNullOrWhiteSpace(errorBody))
                message += $" — {errorBody[..Math.Min(100, errorBody.Length)]}...";

            return ApiResult<T>.Failure(new HttpRequestException(message));
        }
        catch (Exception ex)
        {
            return ApiResult<T>.Failure(ex);
        }
    }
}
