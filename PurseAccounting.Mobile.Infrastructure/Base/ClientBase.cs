using PurseAccounting.Mobile.Infrastructure.ApiResults;
using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;
using PurseAccounting.Mobile.Infrastructure.ServerResults;
using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.Base;

internal abstract class ClientBase
{
    private const int _errorBodyPrerenderMaxSize = 100;

    protected delegate Task<HttpResponseMessage> ExternalCall(string url, CancellationToken cancellationToken);

    protected delegate Task<HttpResponseMessage> ExternalCallWithRequest(string url, object request, CancellationToken cancellationToken);

    protected delegate Task<ApiResult<T>> ParseError<T>(HttpResponseMessage message, CancellationToken cancellationToken);

    protected static Task<ApiResult<T>> SafeCall<T>(ExternalCall call, string url, CancellationToken cancellationToken)
    {
        return SafeCall<T>(call, url, null, cancellationToken);
    }

    protected static Task<ApiResult<T>> SafeCall<T, TErrorCode>(ExternalCall call, string url, CancellationToken cancellationToken)
    {
        return SafeCall(call, url, ParseErrorServerResponse<T, TErrorCode>, cancellationToken);
    }

    protected static Task<ApiResult<T>> SafeCall<T>(ExternalCallWithRequest call, string url, object request, CancellationToken cancellationToken)
    {
        return SafeCall<T>(call, url, request, null, cancellationToken);
    }

    protected static Task<ApiResult<T>> SafeCall<T, TErrorCode>(ExternalCallWithRequest call, string url, object request, CancellationToken cancellationToken)
    {
        return SafeCall(call, url, request, ParseErrorServerResponse<T, TErrorCode>, cancellationToken);
    }

    protected static async Task<ApiResult> SafeCall(ExternalCall call, string url, CancellationToken cancellationToken)
    {
        try
        {
            var response = await call(url, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return ApiResult.Success();
            }

            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var message = $"Ошибка сервера: {(int)response.StatusCode}";

            if (!string.IsNullOrWhiteSpace(errorBody))
                message += $" — {errorBody[..Math.Min(_errorBodyPrerenderMaxSize, errorBody.Length)]}...";

            return ApiResult.Failure(new HttpRequestException(message));
        }
        catch (Exception ex)
        {
            return ApiResult.Failure(ex);
        }
    }

    protected static async Task<ApiResult> SafeCall(ExternalCallWithRequest call, string url, object request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await call(url, request, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return ApiResult.Success();
            }

            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var message = $"Ошибка сервера: {(int)response.StatusCode}";

            if (!string.IsNullOrWhiteSpace(errorBody))
                message += $" — {errorBody[..Math.Min(_errorBodyPrerenderMaxSize, errorBody.Length)]}...";

            return ApiResult.Failure(new HttpRequestException(message));
        }
        catch (Exception ex)
        {
            return ApiResult.Failure(ex);
        }
    }

    private static async Task<ApiResult<T>> ParseErrorServerResponse<T, TErrorCode>(HttpResponseMessage message, CancellationToken cancellationToken)
    {
        var value = await message.Content.ReadFromJsonAsync<ServerErrorResponse<TErrorCode>>(cancellationToken).ConfigureAwait(false);

        return value is null
            ? ApiResult<T>.Failure(new InvalidOperationException($"Http error response of type {nameof(TErrorCode)} deserialized to null."))
            : ApiResult<T>.Failure(new ServerException<TErrorCode>() { NoticeType = value.NoticeType });
    }

    private static async Task<ApiResult<T>> SafeCall<T>(ExternalCall call, string url, ParseError<T>? errorResponseFactory, CancellationToken cancellationToken)
    {
        try
        {
            var response = await call(url, cancellationToken).ConfigureAwait(false);

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
                message += $" — {errorBody[..Math.Min(_errorBodyPrerenderMaxSize, errorBody.Length)]}...";

            return ApiResult<T>.Failure(new HttpRequestException(message));
        }
        catch (Exception ex)
        {
            return ApiResult<T>.Failure(ex);
        }
    }

    private static async Task<ApiResult<T>> SafeCall<T>(ExternalCallWithRequest call, string url, object request, ParseError<T>? errorResponseFactory, CancellationToken cancellationToken)
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
                message += $" — {errorBody[..Math.Min(_errorBodyPrerenderMaxSize, errorBody.Length)]}...";

            return ApiResult<T>.Failure(new HttpRequestException(message));
        }
        catch (Exception ex)
        {
            return ApiResult<T>.Failure(ex);
        }
    }
}
