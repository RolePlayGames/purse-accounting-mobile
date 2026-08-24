namespace PurseAccountinng.Mobile.Presentation.Services.MidnightDistributionManagers;

public interface IMidnightDistributionManager : IAsyncDisposable
{
    /// <summary>
    /// Starts distribution check
    /// </summary>
    /// <param name="timezone">Check's timezone</param>
    Task Start(short timezone, CancellationToken cancellationToken);

    /// <summary>
    /// Stops distribution check
    /// </summary>
    Task Stop();
}
