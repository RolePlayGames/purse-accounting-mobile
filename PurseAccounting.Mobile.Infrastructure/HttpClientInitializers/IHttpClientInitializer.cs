namespace PurseAccounting.Mobile.Infrastructure.HttpClientInitializers;

public interface IHttpClientInitializer
{
    /// <summary>
    /// Initializes http client
    /// </summary>
    Task Initialize();
}
